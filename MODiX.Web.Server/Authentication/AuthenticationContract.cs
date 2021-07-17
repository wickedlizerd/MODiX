using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.PlatformServices;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Remora.Discord.API.Objects;
using Remora.Discord.Rest;

using Modix.Business;
using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Server.Authentication
{
    public class AuthenticationContract
        : IAuthenticationContract
    {
        public AuthenticationContract(
            IOptions<AuthenticationConfiguration>   authenticationConfiguration,
            DiscordHttpClient                       discordClient,
            IOptions<DiscordConfiguration>          discordConfiguration,
            ILogger<AuthenticationContract>         logger,
            IServiceScopeFactory                    serviceScopeFactory,
            ISystemClock                            systemClock)
        {
            _authenticationConfiguration    = authenticationConfiguration;
            _discordClient                  = discordClient;
            _discordConfiguration           = discordConfiguration;
            _logger                         = logger;
            _serviceScopeFactory            = serviceScopeFactory;
            _systemClock                    = systemClock;
        }

        public async Task<CompleteLoginResponse> CompleteLoginAsync(CompleteLoginRequest request, CancellationToken cancellationToken)
        {
            AuthenticationLogMessages.LoginCompleting(_logger, request);

            AuthenticationLogMessages.TokenGrantAcquiring(_logger);
            var tokenGrantResult = await _discordClient.PostAsync<OAuthTokenGrant>(
                "oauth2/token",
                requestBuilder => requestBuilder
                    .AddContent(new StringContent(_discordConfiguration.Value.ClientId),        "client_id")
                    .AddContent(new StringContent(_discordConfiguration.Value.ClientSecret),    "client_secret")
                    .AddContent(new StringContent(request.Code),                                "code")
                    .AddContent(new StringContent(request.RedirectUrl),                         "redirect_uri")
                    .AddContent(new StringContent("authorization_code"),                        "grant_type"),
                ct: cancellationToken);
            if (!tokenGrantResult.IsSuccess)
            {
                var error = tokenGrantResult.Unwrap();
                AuthenticationLogMessages.TokenGrantAcquisitionFailed(_logger, error);
                return new LoginFailure(error.Message);
            }
            AuthenticationLogMessages.TokenGrantAcquired(_logger, tokenGrantResult.Entity);

            var authenticationHeader = new AuthenticationHeaderValue(tokenGrantResult.Entity.TokenType, tokenGrantResult.Entity.AccessToken).ToString();

            AuthenticationLogMessages.UserRetrieving(_logger);
            var getUserResult = await _discordClient.GetAsync<User>(
                "users/@me",
                request => request.AddHeader("Authorization", authenticationHeader),
                ct: cancellationToken);
            if (!getUserResult.IsSuccess)
            {
                var error = getUserResult.Unwrap();
                AuthenticationLogMessages.UserRetrievalFailed(_logger, error);
                return new LoginFailure(error.Message);
            }
            AuthenticationLogMessages.UserRetrieved(_logger, getUserResult.Entity);

            RevokeTokens(tokenGrantResult.Entity.AccessToken, tokenGrantResult.Entity.RefreshToken);

            var userId  = getUserResult.Entity.ID.Value;
            var created = _systemClock.UtcNow;
            var expires = created + _authenticationConfiguration.Value.TokenLifetime;

            var response = new LoginSuccess(
                ticket:         new AuthenticationTicket(
                    userId:         userId,
                    created:        created,
                    expires:        expires),
                bearerToken:    new JwtSecurityTokenHandler().CreateJwtSecurityToken(new SecurityTokenDescriptor()
                    {
                        Claims              = new Dictionary<string, object?>()
                        {
                            [ClaimTypes.NameIdentifier] = userId,
                        },
                        Expires             = expires.DateTime,
                        IssuedAt            = created.DateTime,
                        SigningCredentials  = new(
                            new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(
                                    _authenticationConfiguration.Value.TokenSignatureSecret)),
                            SecurityAlgorithms.HmacSha256Signature)
                    })
                    .RawData);

            AuthenticationLogMessages.LoginCompleted(_logger, response);
            return response;

            async void RevokeTokens(string accessToken, string refreshToken)
            {
                try
                {
                    using var serviceScope = _serviceScopeFactory.CreateScope();

                    var discordClient = serviceScope.ServiceProvider.GetRequiredService<DiscordHttpClient>();

                    await Task.WhenAll(
                        RevokeTokenAsync(discordClient, accessToken, "access_token", cancellationToken),
                        RevokeTokenAsync(discordClient, refreshToken, "refresh_token", cancellationToken));
                }
                catch (Exception ex)
                {
                    AuthenticationLogMessages.TokenRevocationEncounteredException(_logger, ex);
                }

                async Task RevokeTokenAsync(
                    DiscordHttpClient discordClient,
                    string token,
                    string tokenTypeHint,
                    CancellationToken cancellationToken)
                {
                    AuthenticationLogMessages.TokenRevoking(_logger, tokenTypeHint);
                    var revokeTokenResult = await discordClient.PostAsync(
                            "oauth2/token/revoke",
                            request => request
                                .AddContent(new StringContent(_discordConfiguration.Value.ClientId),        "client_id")
                                .AddContent(new StringContent(_discordConfiguration.Value.ClientSecret),    "client_secret")
                                .AddContent(new StringContent(token),                                       "token")
                                .AddContent(new StringContent(tokenTypeHint),                               "token_type_hint"),
                            ct: cancellationToken);
                    if (!revokeTokenResult.IsSuccess)
                    {
                        AuthenticationLogMessages.TokenRevocationFailed(_logger, tokenTypeHint, revokeTokenResult.Unwrap());
                    }
                    else
                        AuthenticationLogMessages.TokenRevoked(_logger, tokenTypeHint);
                }
            }
        }

        public Task<StartLoginResponse> StartLoginAsync(StartLoginRequest request, CancellationToken cancellationToken)
        {
            AuthenticationLogMessages.LoginStarting(_logger, request);

            var response = new StartLoginResponse(
                QueryHelpers.AddQueryString(
                    new Uri(Constants.BaseURL, "oauth2/authorize").AbsoluteUri,
                    new Dictionary<string, string?>()
                    {
                        ["client_id"]       = _discordConfiguration.Value.ClientId,
                        ["redirect_uri"]    = request.RedirectUri,
                        ["response_type"]   = "code",
                        ["scope"]           = "identify",
                        ["state"]           = request.State
                    }));

            AuthenticationLogMessages.LoginStarted(_logger, response);
            return Task.FromResult(response);
        }

        private readonly IOptions<AuthenticationConfiguration>  _authenticationConfiguration;
        private readonly DiscordHttpClient                      _discordClient;
        private readonly IOptions<DiscordConfiguration>         _discordConfiguration;
        private readonly ILogger                                _logger;
        private readonly IServiceScopeFactory                   _serviceScopeFactory;
        private readonly ISystemClock                           _systemClock;
    }
}

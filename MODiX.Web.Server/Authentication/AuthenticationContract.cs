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
            IServiceScopeFactory                    serviceScopeFactory,
            ISystemClock                            systemClock)
        {
            _authenticationConfiguration    = authenticationConfiguration;
            _discordClient                  = discordClient;
            _discordConfiguration           = discordConfiguration;
            _serviceScopeFactory            = serviceScopeFactory;
            _systemClock                    = systemClock;
        }

        public async Task<CompleteLoginResponse> CompleteLoginAsync(CompleteLoginRequest request, CancellationToken cancellationToken)
        {
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
                return new LoginFailure(tokenGrantResult.Error.Message + ": " + tokenGrantResult.Inner?.Error?.Message);

            var authenticationHeader = new AuthenticationHeaderValue(tokenGrantResult.Entity.TokenType, tokenGrantResult.Entity.AccessToken).ToString();

            var getUserResult = await _discordClient.GetAsync<User>(
                "users/@me",
                request => request.AddHeader("Authorization", authenticationHeader),
                ct: cancellationToken);
            if (!getUserResult.IsSuccess)
                return new LoginFailure(getUserResult.Error.Message + ": " + getUserResult.Inner?.Error?.Message);

            RevokeTokens(tokenGrantResult.Entity.AccessToken, tokenGrantResult.Entity.RefreshToken);

            var userId  = getUserResult.Entity.ID.Value;
            var created = _systemClock.UtcNow;
            var expires = created + _authenticationConfiguration.Value.TokenLifetime;

            return new LoginSuccess(
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

            async void RevokeTokens(string accessToken, string refreshToken)
            {
                try
                {
                    using var serviceScope = _serviceScopeFactory.CreateScope();

                    var discordClient = serviceScope.ServiceProvider.GetRequiredService<DiscordHttpClient>();

                    var revokeAccessTokenOperation = discordClient.PostAsync(
                        "oauth2/token/revoke",
                        request => request
                            .AddContent(new StringContent(_discordConfiguration.Value.ClientId),        "client_id")
                            .AddContent(new StringContent(_discordConfiguration.Value.ClientSecret),    "client_secret")
                            .AddContent(new StringContent(accessToken),                                 "token")
                            .AddContent(new StringContent("access_token"),                              "token_type_hint"),
                        ct: cancellationToken);

                    var revokeRefreshTokenOperation = discordClient.PostAsync(
                        "oauth2/token/revoke",
                        request => request
                            .AddContent(new StringContent(_discordConfiguration.Value.ClientId),        "client_id")
                            .AddContent(new StringContent(_discordConfiguration.Value.ClientSecret),    "client_secret")
                            .AddContent(new StringContent(refreshToken),                                "token")
                            .AddContent(new StringContent("refresh_token"),                             "token_type_hint"),
                        ct: cancellationToken);

                    var results = await Task.WhenAll(revokeAccessTokenOperation, revokeRefreshTokenOperation);

                    var revokeAccessTokenResult = results[0];
                    if (!revokeAccessTokenResult.IsSuccess)
                        // TODO: Implement Logging
                        System.Diagnostics.Debug.WriteLine("Error revoking access token: " + revokeAccessTokenResult.Error.Message + ": " + revokeAccessTokenResult.Inner?.Error?.Message);

                    var revokeRefreshTokenResult = results[1];
                    if (!revokeRefreshTokenResult.IsSuccess)
                        // TODO: Implement Logging
                        System.Diagnostics.Debug.WriteLine("Error revoking access token: " + revokeRefreshTokenResult.Error.Message + ": " + revokeRefreshTokenResult.Inner?.Error?.Message);
                }
                catch(Exception ex)
                {
                    // TODO: Implement Logging
                    System.Diagnostics.Debug.WriteLine("Exception revoking authentication tokens: " + ex.Message);
                }
            }
        }

        public Task<StartLoginResponse> StartLoginAsync(StartLoginRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new StartLoginResponse(
                QueryHelpers.AddQueryString(
                    new Uri(Constants.BaseURL, "oauth2/authorize").AbsoluteUri,
                    new Dictionary<string, string?>()
                    {
                        ["client_id"]       = _discordConfiguration.Value.ClientId,
                        ["redirect_uri"]    = request.RedirectUri,
                        ["response_type"]   = "code",
                        ["scope"]           = "identify",
                        ["state"]           = request.State
                    })));

        private readonly IOptions<AuthenticationConfiguration> _authenticationConfiguration;
        private readonly DiscordHttpClient _discordClient;
        private readonly IOptions<DiscordConfiguration> _discordConfiguration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ISystemClock _systemClock;
    }
}

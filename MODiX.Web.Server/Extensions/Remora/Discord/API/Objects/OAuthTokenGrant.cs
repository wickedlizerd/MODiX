namespace Remora.Discord.API.Objects
{
    public class OAuthTokenGrant
    {
        public string AccessToken { get; init; }
            = null!;

        public string TokenType { get; init; }
            = null!;

        public int ExpiresIn { get; init; }

        public string RefreshToken { get; init; }
            = null!;

        public string Scope { get; init; }
            = null!;
    }
}

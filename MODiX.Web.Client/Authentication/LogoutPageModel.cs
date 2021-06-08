namespace Modix.Web.Client.Authentication
{
    public class LogoutPageModel
    {
        public LogoutPageModel(IAuthenticationManager authenticationManager)
            => _authenticationManager = authenticationManager;

        public void Logout()
            => _authenticationManager.OnSignedOut();

        private readonly IAuthenticationManager _authenticationManager;
    }
}

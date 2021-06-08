using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Client.Authentication
{
    public class LoginPageModel
    {
        public LoginPageModel(IAuthenticationContract authenticationContract)
        {
            _authenticationContract = authenticationContract;
            _loginUri = new BehaviorSubject<string?>(null);
        }

        public IObservable<string?> LoginUri
            => _loginUri;

        public async Task StartLoginAsync(string redirectUri, string? state)
        {
            var startLoginResponse = await _authenticationContract.StartLoginAsync(
                new(redirectUri:    redirectUri,
                    state:          state),
                CancellationToken.None);

            _loginUri.OnNext(startLoginResponse.AuthorizeUri);
        }

        private readonly IAuthenticationContract    _authenticationContract;
        private readonly BehaviorSubject<string?>   _loginUri;
    }
}

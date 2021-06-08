using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Client.Authentication
{
    public class LoginPageModel
    {
        public LoginPageModel(
            IAuthenticationContract authenticationContract,
            IAuthenticationManager authenticationManager)
        {
            _authenticationContract = authenticationContract;
            _authenticationManager  = authenticationManager;
            _failureMessage         = new(null);
            _loginUri               = new(null);
            _status                 = new(LoginStatus.Loading);
        }

        public IObservable<string?> FailureMessage
            => _failureMessage;

        public IObservable<string?> LoginUri
            => _loginUri;

        public IObservable<LoginStatus> Status
            => _status;

        public async Task CompleteLoginAsync(string code, string redirectUri)
        {
            var completLoginResponse = await _authenticationContract.CompleteLoginAsync(new(
                    code:           code,
                    redirectUri:    redirectUri),
                CancellationToken.None);

            if (completLoginResponse is LoginSuccess success)
            {
                _authenticationManager.OnSignedIn(success.BearerToken, success.Ticket);
                _status.OnNext(LoginStatus.Success);
            }
            else if (completLoginResponse is LoginFailure failure)
            {
                _failureMessage.OnNext(failure.Message);
                _status.OnNext(LoginStatus.ServerFailure);
            }
            else
                _status.OnNext(LoginStatus.ProtocolError);
        }

        public async Task StartLoginAsync(string redirectUri, string? state)
        {
            var startLoginResponse = await _authenticationContract.StartLoginAsync(
                new(redirectUri:    redirectUri,
                    state:          state),
                CancellationToken.None);

            _loginUri.OnNext(startLoginResponse.AuthorizeUri);
            _status.OnNext(LoginStatus.Idle);
        }

        private readonly IAuthenticationContract        _authenticationContract;
        private readonly IAuthenticationManager         _authenticationManager;
        private readonly BehaviorSubject<string?>       _failureMessage;
        private readonly BehaviorSubject<string?>       _loginUri;
        private readonly BehaviorSubject<LoginStatus>  _status;
    }
}

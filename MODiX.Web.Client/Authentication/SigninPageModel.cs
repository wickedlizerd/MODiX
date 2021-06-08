using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Client.Authentication
{
    public class SigninPageModel
    {
        public SigninPageModel(
            IAuthenticationManager authenticationManager,
            IAuthenticationContract authenticationContract)
        {
            _authenticationContract = authenticationContract;
            _authenticationManager = authenticationManager;
            _failureMessage = new BehaviorSubject<string?>(null);
            _status = new BehaviorSubject<SigninStatus>(SigninStatus.Pending);
        }

        public IObservable<string?> FailureMessage
            => _failureMessage;

        public IObservable<SigninStatus> Status
            => _status;

        public async Task SigninAsync(string code, string redirectUri)
        {
            var completLoginResponse = await _authenticationContract.CompleteLoginAsync(new(
                    code:           code,
                    redirectUri:    redirectUri),
                CancellationToken.None);

            if (completLoginResponse is LoginSuccess success)
            {
                _authenticationManager.OnSignedIn(success.Ticket);
                _status.OnNext(SigninStatus.Success);
            }
            else if (completLoginResponse is LoginFailure failure)
            {
                _failureMessage.OnNext(failure.Message);
                _status.OnNext(SigninStatus.ServerFailure);
            }
            else
                _status.OnNext(SigninStatus.ProtocolError);
        }

        private readonly IAuthenticationContract        _authenticationContract;
        private readonly IAuthenticationManager         _authenticationManager;
        private readonly BehaviorSubject<string?>       _failureMessage;
        private readonly BehaviorSubject<SigninStatus>  _status;
    }
}

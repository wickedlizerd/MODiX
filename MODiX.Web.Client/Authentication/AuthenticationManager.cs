using System;
using System.Reactive.Subjects;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Browser;

using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Client.Authentication
{
    public interface IAuthenticationManager
    {
        IObservable<AuthenticationTicket?> CurrentTicket { get; }

        Task<AuthenticationState> GetAuthenticationStateAsync();

        void OnSignedIn(AuthenticationTicket ticket);

        void OnSignedOut();
    }

    public class AuthenticationManager
        : AuthenticationStateProvider,
            IAuthenticationManager
    {
        public AuthenticationManager(
            ILocalStorageManager localStorageManager)
        {
            _currentState           = _unauthenticatedState;
            _currentTicket          = new BehaviorSubject<AuthenticationTicket?>(null);
            _localStorageManager    = localStorageManager;

            TryLoadFromLocalStorage();

            async void TryLoadFromLocalStorage()
            {
                try
                {
                    var ticket = await _localStorageManager.TryGetValueAsync<AuthenticationTicket>(CurrentTicketStorageKey);
                    if (ticket != null)
                        OnSignedIn(ticket);
                }
                catch (Exception ex)
                {
                    // TODO: Implement Logging
                    Console.WriteLine("Exception loading authentication data from local storage: " + ex.Message);
                }
            }
        }

        public IObservable<AuthenticationTicket?> CurrentTicket
            => _currentTicket;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => Task.FromResult(_currentState);

        public void OnSignedIn(AuthenticationTicket ticket)
        {
            SaveToLocalStorage(ticket);

            _currentTicket.OnNext(ticket);

            var state = new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        claims:             new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, ticket.UserId.ToString(), ClaimValueTypes.UInteger64)
                        },
                        authenticationType: "Modix.Web.Protocol.Authentication.AuthenticationTicket")));


            _currentState = state;
            NotifyAuthenticationStateChanged(Task.FromResult(_currentState));

            async void SaveToLocalStorage(AuthenticationTicket ticket)
            {
                try
                {
                    await _localStorageManager.SetValueAsync(CurrentTicketStorageKey, ticket);
                }
                catch (Exception ex)
                {
                    // TODO: Implement Logging
                    Console.WriteLine("Exception saving authentication data to local storage: " + ex.Message);
                }
            }
        }

        public void OnSignedOut()
        {
            ClearLocalStorage();

            _currentTicket.OnNext(null);
            _currentState = _unauthenticatedState;
            NotifyAuthenticationStateChanged(Task.FromResult(_currentState));

            async void ClearLocalStorage()
            {
                try
                {
                    await _localStorageManager.RemoveKeyAsync(CurrentTicketStorageKey);
                }
                catch (Exception ex)
                {
                    // TODO: Implement Logging
                    Console.WriteLine("Exception saving authentication data to local storage: " + ex.Message);
                }
            }
        }

        private readonly BehaviorSubject<AuthenticationTicket?> _currentTicket;
        private readonly ILocalStorageManager                   _localStorageManager;

        private AuthenticationState _currentState;

        private const string CurrentTicketStorageKey
            = "AuthenticationManager.CurrentTicket";

        private static readonly AuthenticationState _unauthenticatedState
            = new(new ClaimsPrincipal());
    }
}

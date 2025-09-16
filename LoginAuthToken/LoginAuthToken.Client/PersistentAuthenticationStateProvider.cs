using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using System.Threading.Tasks;
using LoginAuthToken.Client.Models;

namespace LoginAuthToken.Client
{
    public class PersistentAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly PersistentComponentState _state;

        private ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity()); // usuario no autenticado por defecto

        public PersistentAuthenticationStateProvider(PersistentComponentState state)
        {
            _state = state;

            // Intentar cargar usuario persistido
            if (_state.TryTakeFromJson<LoginModel>(nameof(LoginModel), out var userInfo) && userInfo != null)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userInfo.Email!)
                }, "PersistentAuth");

                _user = new ClaimsPrincipal(identity);
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_user));
        }

        public void MarkUserAsAuthenticated(LoginModel userInfo)
        {
            var identity = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, userInfo.Email!)
    }, "PersistentAuth");

            _user = new ClaimsPrincipal(identity);

            // Persistir explícitamente con tipo
            _state.PersistAsJson<LoginModel>(nameof(LoginModel), userInfo);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

        public void MarkUserAsLoggedOut()
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity());
            _state.PersistAsJson<LoginModel>(nameof(LoginModel), null! ); // eliminar persistencia
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
        }

    }
}

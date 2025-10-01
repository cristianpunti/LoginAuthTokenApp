using LoginAuthToken.LocalStorage;

namespace LoginAuthToken.Services
{
    public class UserSessionService
    {
        public string? Token { get; set; }

        private string? _clientIp;
        public string? ClientIp
        {
            get => _clientIp;
            set
            {
                _clientIp = value;
                NotifyStateChanged();
            }
        }

        public event Func<Task>? OnChange;

        private Task NotifyStateChanged()
        {
            return OnChange?.Invoke() ?? Task.CompletedTask;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(_clientIp);

        /// <summary>
        /// Carga la sesión desde memoria o LocalStorage y devuelve true si hay sesión válida
        /// </summary>
        public async Task<bool> LoadSessionAsync(LocalStorageHelper localStorageHelper)
        {
            ClientIp ??= await localStorageHelper.GetItemAsync(ProjectConstants.LocalStorageIpKey);
            Token ??= await localStorageHelper.GetItemAsync(ProjectConstants.LocalStorageTokenKey);
            return !string.IsNullOrEmpty(ClientIp) && !string.IsNullOrEmpty(Token);
        }

        /// <summary>
        /// Valida que la sesión exista y coincida con la IP
        /// </summary>
        public bool IsAuthorized(string? ip)
        {
            return !string.IsNullOrEmpty(ClientIp)
                   && !string.IsNullOrEmpty(Token)
                   && ip?.Trim() == ClientIp?.Trim();
        }
    }

}

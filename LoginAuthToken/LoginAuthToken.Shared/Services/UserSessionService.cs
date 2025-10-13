
namespace LoginAuthToken.Shared.Services
{
    public class UserSessionService
    {
        private string? _clientIp;
        private string? _token;
        private bool _apiOnline;

        public event Action? OnStatusChanged;

        public string? ClientIp
        {
            get => _clientIp;
            set
            {
                if (_clientIp != value)
                {
                    _clientIp = value;
                    NotifyStateChanged();
                }
            }
        }

        public string? Token
        {
            get => _token;
            set
            {
                if (_token != value)
                {
                    _token = value;
                    NotifyStateChanged();
                }
            }
        }

        public bool ApiOnline
        {
            get => _apiOnline;
            set
            {
                if (_apiOnline != value)
                {
                    _apiOnline = value;
                    NotifyStateChanged();
                }
            }
        }

        private void NotifyStateChanged() => OnStatusChanged?.Invoke();


        public void SetSession(string ip, string token)
        {
            ClientIp = ip;
            Token = token;
            ApiOnline = true;
            NotifyStateChanged();
        }

        public void ClearSession()
        {
            ClientIp = null;
            Token = null;
            ApiOnline = false;
            NotifyStateChanged();
        }
    }
 }
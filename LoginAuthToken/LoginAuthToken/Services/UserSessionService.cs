namespace LoginAuthToken.Services
{
    public class UserSessionService
    {
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
    }

}

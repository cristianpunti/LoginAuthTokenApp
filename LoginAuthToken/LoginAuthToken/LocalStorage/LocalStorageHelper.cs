using Blazored.LocalStorage;

namespace LoginAuthToken.LocalStorage
{
    public class LocalStorageHelper
    {
        private readonly ILocalStorageService _localStorage;

        public LocalStorageHelper(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveItemAsync(string key, string? value)
        {
            if (value == null)
                await _localStorage.RemoveItemAsync(key);
            else
                await _localStorage.SetItemAsync(key, value);
        }

        public async Task<string?> GetItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            return await _localStorage.GetItemAsync<string>(key);
        }

        public async Task RemoveItemAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            await _localStorage.RemoveItemAsync(key);
        }

        public async Task ClearUserDataAsync()
        {
            await _localStorage.ClearAsync();

        }
    }
}
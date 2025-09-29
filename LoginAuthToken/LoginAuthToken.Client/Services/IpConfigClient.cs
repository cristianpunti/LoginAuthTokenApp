using System.Net.Http.Json;
using LoginAuthToken.Client.ViewModels;

namespace LoginAuthToken.Client.Services
{
    public class IpConfigClient
    {
        private readonly HttpClient _http;

        public IpConfigClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<IpConfigViewModel>> GetIpsAsync()
        {
            return await _http.GetFromJsonAsync<List<IpConfigViewModel>>("api/ipconfig") ?? new();
        }
    }
}

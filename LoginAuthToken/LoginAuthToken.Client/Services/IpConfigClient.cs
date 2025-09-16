using System.Net.Http.Json;
using LoginAuthToken.Client.Models;

namespace LoginAuthToken.Client.Services
{
    public class IpConfigClient
    {
        private readonly HttpClient _http;

        public IpConfigClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<IpConfig>> GetIpsAsync()
        {
            return await _http.GetFromJsonAsync<List<IpConfig>>("api/ipconfig") ?? new();
        }
    }
}

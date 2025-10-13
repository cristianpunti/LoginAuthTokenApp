
using Blazored.LocalStorage;
using LoginAuthToken.Shared.LocalStorage;
using LoginAuthToken.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


namespace LoginAuthToken.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7269/") });
            // ? HttpClient apuntando a la API de Azure
            //builder.Services.AddScoped(sp => new HttpClient
            //{
            //    BaseAddress = new Uri("https://iisauth-e3hkhka0hba2dea6.eastus-01.azurewebsites.net") // <- tu URL de Azure
            //});
            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();

            // Registrar correctamente tu provider
            builder.Services.AddSingleton<PersistentAuthenticationStateProvider>();
            builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<PersistentAuthenticationStateProvider>());

            // ? Servicios propios (idénticos a los que usas en Server)
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<LocalStorageHelper>();
            builder.Services.AddSingleton<UserSessionService>();

            // ? Logging
            builder.Logging.SetMinimumLevel(LogLevel.Debug);

            await builder.Build().RunAsync();


        }
    }
}

using LoginAuthToken;
using LoginAuthToken.Components;
using LoginAuthToken.Server.Services;

//using LoginWithoutIdenty.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.EntityFrameworkCore;

namespace LoginAuthtoken
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            //// Registrar DbContext
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Construir la ruta absoluta del XML
            var xmlPath = Path.Combine(builder.Environment.ContentRootPath, "FileConfigs.xml");

            // Crear la instancia de IpConfigService y registrarla
            var ipConfigService = new IpConfigService(xmlPath);
            builder.Services.AddSingleton<IpConfigService>(ipConfigService);

            // Registrar controllers
            builder.Services.AddControllers(); // <--- OBLIGATORIO

            // Registrar HttpClient para Blazor Server
            builder.Services.AddScoped(sp =>
            {
                var nav = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
                return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
            });

            // Servicios
          //  builder.Services.AddSingleton<IpConfigService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

#if DEBUG
            // Agregar consola y depuración
            builder.Logging.AddDebug();
            builder.Logging.AddConsole();
#endif

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();


            app.UseRouting();


            // Registrar LoggerFactory
            //builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
            //builder.Services.AddScoped(typeof(ILogger<>), typeof(Logger<>));

            //app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapControllers(); // <- Esto es necesario para que /api/auth/login funcione
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(LoginAuthToken.Client._Imports).Assembly);
            

            app.Run();
        }
    }
}

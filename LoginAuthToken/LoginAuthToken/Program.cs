using Blazored.LocalStorage;
using LoginAuthToken;
using LoginAuthToken.Components;
using LoginAuthToken.Shared.Services;
using LoginAuthToken.Shared.LocalStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using LoginAuthToken.Server.Services;

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
            var xmlPath = Path.Combine(builder.Environment.ContentRootPath, "IpConfigs.xml");

            // Crear la instancia de IpConfigService y registrarla
            var ipConfigService = new IpConfigService(xmlPath);
            builder.Services.AddSingleton<IpConfigService>(ipConfigService);

            // 1️⃣ Añadir política de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient",
                    policy => policy
                        .WithOrigins("https://localhost:7288") // la URL del cliente WebAssembly
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

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

            builder.Services.AddServerSideBlazor()
           .AddCircuitOptions(options =>
           {
               options.DetailedErrors = true;
               options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
           });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDistributedMemoryCache(); // Necesario para almacenar session en memoria
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo que dura la sesión
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddSingleton<UserSessionService>();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<LocalStorageHelper>();

            //builder.Services.AddScoped(sp =>
            //{
            //    return new HttpClient
            //    {
            //        //BaseAddress = new Uri("https://localhost:7086/") // URL de tu API
            //        BaseAddress = new Uri("https://iisauth-e3hkhka0hba2dea6.eastus-01.azurewebsites.net")
            //    };
            //});

            // Named client pointing at the API host (base address = site root)
            builder.Services.AddHttpClient("ExternalApi", client =>
            {
                client.BaseAddress = new Uri("https://iisauth-e3hkhka0hba2dea6.eastus-01.azurewebsites.net/");
                client.Timeout = TimeSpan.FromSeconds(15);
            });

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

           // 2️⃣ Aplicar CORS **después de UseRouting y antes de MapControllers**
            app.UseCors("AllowBlazorClient");


            app.UseSession();

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

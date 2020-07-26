using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;
using Registry.IDP.Models;
using Registry.IDP.Validators;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using Registry.IDP.Extensions;

namespace Registry.IDP
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddScoped<IExtensionGrantValidator, DelegationGrantValidator>();

            var httpsEndpointConfig = new EndpointConfiguration();
            Configuration.GetSection("HttpServer:Https").Bind(httpsEndpointConfig);

            var builder = services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(KestrelServerOptionsExtensions.LoadCertificate(httpsEndpointConfig, CurrentEnvironment)))
                .AddTestUsers(Config.GetUsers());

            var identityConnectionString = Configuration.GetConnectionString("IdentityDatabase");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            builder.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(identityConnectionString, (options) =>
                {
                    options.MigrationsAssembly(migrationsAssembly);
                });
            });

            builder.AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(identityConnectionString, (options) =>
                {
                    options.MigrationsAssembly(migrationsAssembly);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Since this is a demo env, we can call a method to initialize some seed data
            // For production envs, consider using SQL scripts
            InitializeDatabase(app);

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider
                    .GetRequiredService<ConfigurationDbContext>();

                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}

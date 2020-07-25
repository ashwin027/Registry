using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalog.Common.Models;
using ProductCatalog.Repository;
using ProductCatalog.Shared;
using ProductCatalog.Shared.Extensions;

namespace ProductCatalog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductContext>();
            services.Configure<Config>(Configuration.GetSection(nameof(Config)));

            var config = new IdentityConfiguration()
            {
                Authority = Configuration["oidc:authority"],
                ApiName = Configuration["oidc:apiname"]
            };
            services.AddProductAuthentication(config);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

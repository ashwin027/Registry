using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reviews.Grpc.Services;
using Reviews.Models;
using Reviews.Repository;
using Reviews.Shared;
using Reviews.Shared.Extensions;
using static ProductCatalog.Grpc.Product;

namespace Reviews.Grpc
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ReviewContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ReviewDatabase"));
            });

            var config = new IdentityConfiguration()
            {
                Authority = Configuration["oidc:authority"],
                ApiName = Configuration["oidc:apiname"]
            };
            services.Configure<IdentityConfiguration>(Configuration.GetSection(IdentityConfiguration.Oidc));

            services.AddGrpcClient<ProductClient>(o =>
            {
                o.Address = new Uri(Configuration["ProductEndpoint"]);
            });

            services.AddReviewAuthentication(config);
            services.AddAuthorization();

            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddHttpClient();

            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReviewContext dataContext)
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
                endpoints.MapGrpcService<ReviewService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

            // Migrate the DB
            dataContext.Database.Migrate();
        }
    }
}

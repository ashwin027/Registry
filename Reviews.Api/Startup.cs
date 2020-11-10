using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reviews.Models;
using Reviews.Repository;
using Reviews.Shared;
using Reviews.Shared.Extensions;
using static ProductCatalog.Grpc.Product;

namespace Reviews.Api
{
    public class Startup
    {
        private const string productEndPointkey = "ProductEndpoint";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReviewContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Migrate the DB
            dataContext.Database.Migrate();
        }
    }
}

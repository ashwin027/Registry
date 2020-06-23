using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Shared.Extensions
{
    public static class Authentication
    {
        public static void AddProductAuthentication(this IServiceCollection services, IdentityConfiguration config)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = config.Authority;
                        options.ApiName = config.ApiName;
                        options.RequireHttpsMetadata = true;
                    });
        }
    }
}

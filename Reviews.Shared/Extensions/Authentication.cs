using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Reviews.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reviews.Shared.Extensions
{
    public static class Authentication
    {
        public static void AddAuthentication(this IServiceCollection services, Config config)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = config.Identity.Authority;
                        options.ApiName = config.Identity.ApiName;
                        options.RequireHttpsMetadata = true;
                    });
        }
    }
}

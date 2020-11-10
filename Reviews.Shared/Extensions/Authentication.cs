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
        public static void AddReviewAuthentication(this IServiceCollection services, IdentityConfiguration config)
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

using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Reviews.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Reviews.Shared.Extensions
{
    public static class HttpClientExtended
    {
        public static async Task<string> GetDelegatedProductTokenAsync(this HttpClient client, IdentityConfiguration config, string userToken)
        {
            var endpoint = config.Authority;

            if (!endpoint.Trim().EndsWith("/"))
            {
                endpoint = $"{endpoint}/";
            }
            endpoint = $"{endpoint}connect/token";

            // send custom grant to token endpoint, return response
            var tokenResponse = await client.RequestTokenAsync(new TokenRequest
            {
                Address = endpoint,
                GrantType = Constants.DelegatedGrantType,

                ClientId = "Reviews",
                ClientSecret = "7666393",

                Parameters = { { "scope", "productcatalog" }, { "token", userToken } }
            });

            return tokenResponse.AccessToken;
        }
    }
}

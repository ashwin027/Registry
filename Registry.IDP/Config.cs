using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Registry.IDP
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "6f10a389-afe7-4b05-9d73-520b69f2ecce",
                    Username = "Admin",
                    Password = "admin",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Admin"),
                        new Claim("family_name", "Admin"),
                        new Claim("role", "Admin")
                    }
                },
                new TestUser
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "Homer",
                    Password = "duff",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Homer"),
                        new Claim("family_name", "Simpson"),
                        new Claim("role", "User")
                    }
                },
                new TestUser
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "Marge",
                    Password = "blue",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Marge"),
                        new Claim("family_name", "Simpson"),
                        new Claim("role", "User")
                    }
                },
                new TestUser
                {
                    SubjectId = "15ccc750-3d80-4839-b85f-bfe62c45a44f",
                    Username = "Bart",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Bart"),
                        new Claim("family_name", "Simpson"),
                        new Claim("role", "User")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new List<string>(){ JwtClaimTypes.Role })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("productcatalog", "Product catalog API"),
                new ApiResource("reviews", "Reviews API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>() {
                new Client
                {
                    ClientName = "Registry Client",
                    ClientId = "Registry",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "http://localhost:4000/signin-oidc"
                    },
                    ClientSecrets =
                    {
                        new Secret("7699023".Sha256())
                    },
                    AllowedScopes = new List<string>()
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "productcatalog",
                        "reviews",
                        "roles"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4000/signout-callback-oidc"
                    }
                },
                new Client
                {
                    ClientId = "Reviews",
                    AllowedGrantTypes = { "delegation" },
                    ClientSecrets =
                    {
                        new Secret("7666393".Sha256())
                    },
                    AllowedScopes = new List<string>()
                    {
                        "productcatalog"
                    }
                }
            };
        }
    }
}

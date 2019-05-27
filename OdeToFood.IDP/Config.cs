using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace OdeToFood.IDP
{
    public class Config
    {
        public static List<TestUser> GetUsers()
        {
            var list = new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = "924527A8-E0F5-46ED-B492-62A9C4EBD3C3",
                    Username = "artem",
                    Password = "password",
                    Claims = new List<Claim>()
                    {
                        new Claim("given_name", "Artem"),
                        new Claim("family_name", "Uznazov"),
                        new Claim("address", "117-48 Peremohi av"),
                        new Claim("role", "FreeUser"),
                        new Claim("subscriptionlevel", "FreeUser"),
                        new Claim("country", "Ukraine")
                    }
                },
                 new TestUser()
                {
                    SubjectId = "F6BECFE4-9EFB-4E57-BE27-A37537EB3D0F",
                    Username = "stepan",
                    Password = "password",
                    Claims = new List<Claim>()
                    {
                        new Claim("given_name", "Stepan"),
                        new Claim("family_name", "Stepanov"),
                        new Claim("address", "37-3 Belove str, ap 30 "),
                        new Claim("role", "PayingUser"),
                        new Claim("subscriptionlevel", "PayingUser"),
                        new Claim("country", "Ukraine")
                    }
                }
                            };
            return list;
        }

        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource("roles", "Your role(s)", new List<string>(){ "role"}),
                new IdentityResource("country", "Your country", new List<string>(){ "country"}),
                new IdentityResource("subscriptionlevel", "Your subscription level", new List<string>(){ "subscriptionlevel"})
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "odetofoodclient",
                    ClientName = "OdeToFood",
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    Enabled = true,

                    AllowedGrantTypes = { GrantType.Hybrid },
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44326/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44326/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "odetofoodapi",
                        "country",
                        "subscriptionlevel"
                    },
                    //AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("odetofoodapi", "OdeToFood API", new List<string>(){"role"})
            };
        }

    }
}

// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace QuickstartIdentityServer
{
    public class Config
    {
        public class MyIdentityResource : IdentityResource
        {
            public MyIdentityResource()
            {
                Name = "customscope";
                DisplayName = "Custom identity resource";
                Emphasize = true;
                UserClaims.Add("toto");
                UserClaims.Add(ClaimTypes.Role);
                UserClaims.Add(ClaimTypes.Name);
                UserClaims.Add(ClaimTypes.NameIdentifier);
                UserClaims.Add(ClaimTypes.WindowsAccountName);
                UserClaims.Add(ClaimTypes.GroupSid);
                UserClaims.Add(ClaimTypes.Email);
            }
        }

        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new MyIdentityResource()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    ClientUri = "http://identityserver.io",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireClientSecret = true,

                    ClientSecrets = { new Secret("secretcusto".Sha256()) },

                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowedCorsOrigins =     { "http://localhost:7017" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "customscope"
                    }
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.be"),
                        new Claim("toto", "hello!")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "3",
                    ProviderSubjectId = "TMA-W8\\Thomas",                    
                    Username = "TMA-W8\\Thomas",
                    ProviderName = "Windows",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Thomas"),
                        new Claim("website", "http://tmadev.be"),
                        new Claim("toto", "Coucou mec!")
                    }
                }
            };
        }
    }
}
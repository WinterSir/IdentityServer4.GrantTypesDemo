using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Authentication
{
    public class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            //new IdentityResources.Email(),
            //new IdentityResources.Address(),
            //new IdentityResources.Phone()
        };
        /// <summary>
        /// ApiResource 资源列表
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("UserApiResource", "获取用户信息API")
                {
                    Scopes={ "UserScope" }
                },
                new ApiResource("ProductApiResource", "获取商品信息API")
                {
                    Scopes={ "ProductScope" }
                }
            };
        }
        /// <summary>
        /// ApiScopes 作用域
        /// </summary>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[]
            {
                new ApiScope("UserScope"),
                new ApiScope("ProductScope")
            };
        }
        /// <summary>
        /// Client 客户端
        /// </summary>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                //客户端模式
                new Client
                {
                    ClientId = "ClientCredentials",
                    ClientName = "ClientCredentials",
                    ClientSecrets = new [] { new Secret("ClientCredentials".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = new [] { "UserScope" }
                },
                //密码模式
                new Client
                {
                    ClientId = "ResourceOwnerPasswordCredentials",
                    ClientName = "ResourceOwnerPasswordCredentials",
                    ClientSecrets = new [] { new Secret("ResourceOwnerPasswordCredentials".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new []
                    {
                        "ProductScope",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },
                //简化模式
                new Client
                {
                    ClientId = "Implicit",
                    ClientName = "Implicit",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                    RequireConsent = true,
                    AllowedScopes = new []{
                        "UserScope",
                        "ProductScope",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },
                //授权码模式
                new Client
                {
                    ClientId = "AuthorizationCode",
                    ClientName = "AuthorizationCode",
                    ClientSecrets = new [] { new Secret("AuthorizationCode".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                    RequireConsent = true,
                    AllowedScopes = new []{
                        "UserScope",
                        "ProductScope",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                },
                //混合模式
                new Client
                {
                    ClientId = "Hybrid",
                    ClientName = "Hybrid",
                    ClientSecrets = new [] { new Secret("Hybrid".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                    RequireConsent = true,
                    RequirePkce = false,
                    AllowedScopes = new []{
                        "UserScope",
                        "ProductScope",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //IdentityServerConstants.StandardScopes.Email,
                        //IdentityServerConstants.StandardScopes.Address,
                        //IdentityServerConstants.StandardScopes.Phone
                    }
                },
            };
        }
        public static List<TestUser> GetTestUser()
        {
            return new List<TestUser>(){
                new TestUser
                {
                    SubjectId = "1",
                    Username = "WinterSir",
                    Password = "WinterSir",
                    Claims =
                    {
                         new Claim(JwtClaimTypes.Name,"WinterSir"),
                         new Claim(JwtClaimTypes.GivenName,"WinterSir"),
                         new Claim(JwtClaimTypes.FamilyName,"WinterSir-FamilyName"),
                         new Claim(JwtClaimTypes.Email,"641187567@qq.com"),
                         new Claim(JwtClaimTypes.EmailVerified,"true", ClaimValueTypes.Boolean),
                         new Claim(JwtClaimTypes.WebSite,"http://WinterSir.com"),
                         new Claim(JwtClaimTypes.Address,
                            @" [ 'street_address': 'Chang Ping', 'locality': 'BeiJing' ,'postal_code’: 102206,'country': 'China'}",
                         IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }
}

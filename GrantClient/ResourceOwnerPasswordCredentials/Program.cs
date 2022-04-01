using IdentityModel.Client;
using System;
using System.Net.Http;

namespace ResourceOwnerPasswordCredentials
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***************** 密码模式（Resource Owner Password credentials）***************** ");
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("https://localhost:5000/").Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            var tokenResponse = client.RequestPasswordTokenAsync(new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = "ResourceOwnerPasswordCredentials",
                ClientSecret = "ResourceOwnerPasswordCredentials",
                UserName = "WinterSir",
                Password = "WinterSir",
                Scope = "ProductScope",
            }).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine("\nToken: " + tokenResponse.AccessToken);

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = apiClient.GetAsync("https://localhost:9000/Product/Get").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("\n结果: " + content);
            }

            Console.ReadLine();
        }
    }
}

using IdentityModel.Client;
using System;
using System.Net.Http;

namespace ClientCredentials
{
    class Program
    {
        /// <summary>
        /// 客户端模式（Client Credentials）
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("***************** 客户端模式（Client Credentials）*****************");
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("https://localhost:5000/").Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ClientCredentials",
                ClientSecret = "ClientCredentials",
                Scope = "UserScope"
            }).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine("\nToken: " + tokenResponse.AccessToken);

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = apiClient.GetAsync("https://localhost:8000/User/Get").Result;
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

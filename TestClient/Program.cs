using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // discover endpoints from metadata
            var disco = DiscoveryClient.GetAsync("http://localhost:5000/identity").Result;

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("api1").Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = client.GetAsync("http://localhost:5000/api/identity").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }

            // request token
            var roTokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var roTokenResponse = roTokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1").Result;

            if (roTokenResponse.IsError)
            {
                Console.WriteLine(roTokenResponse.Error);
                return;
            }

            Console.WriteLine(roTokenResponse.Json);

            // call api
            var roClient = new HttpClient();
            roClient.SetBearerToken(roTokenResponse.AccessToken);

            var roResponse = roClient.GetAsync("http://localhost:5000/api/identity").Result;
            if (!roResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(roResponse.StatusCode);
            }
            else
            {
                var content = roResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }

            Console.WriteLine("\n\n");

            Console.ReadKey();
        }
    }
}

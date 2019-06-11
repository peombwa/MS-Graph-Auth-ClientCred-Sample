using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace MSGraphAuthClientCredSample
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }

        public static async Task RunAsync()
        {
            try
            {
                var graphClient = GetGraphServiceClient();
                var users = await graphClient.Users.Request().GetAsync();
                var me = await graphClient.Users[users.FirstOrDefault().Id].Request().GetAsync();
                Console.WriteLine($"me -> {me.DisplayName} - {me.Id}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static GraphServiceClient GetGraphServiceClient()
        {
            string clientId = "CLIENT_ID";
            string clientSecret = "CLIENT_SECRET";
            string tennatId = "TENNANT_ID";
            var clientApplication = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tennatId}"))
                .Build();

            TokenStorageProvider.Initialize(clientApplication.AppTokenCache);

            var authProvider = new ClientCredentialProvider(clientApplication);

            return new GraphServiceClient(authProvider);
        }
    }
}

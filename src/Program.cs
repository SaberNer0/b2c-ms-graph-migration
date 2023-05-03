using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using Azure.Identity;

namespace b2c_ms_graph
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            //<ms_docref_set_auth_provider>
            // Read application settings from appsettings.json (tenant ID, app ID, client secret, etc.)
            AppSettings config = AppSettingsFile.ReadFromJsonFile();

            // Initialize the client credential auth provider
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var clientSecretCredential = new ClientSecretCredential(config.TenantId, config.AppId, config.ClientSecret);
            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            try
            {
                await UserService.BulkCreate(config, graphClient);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex}");
                Console.ResetColor();
            }
            Console.ReadLine();
        }
    }
}

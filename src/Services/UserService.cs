using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using File = System.IO.File;

namespace b2c_ms_graph
{
    public class UserService
    {        
        public static async Task BulkCreate(AppSettings config, GraphServiceClient graphClient)
        {
            // Get the users to import
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = System.IO.Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string dataFilePathCsv = Path.Combine(projectDirectory, config.UsersFileNameCsv);

            // Verify and notify on file existence
            if (!File.Exists(dataFilePathCsv))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"File '{dataFilePathCsv}' not found.");
                Console.ResetColor();
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Starting bulk create operation...");

            // Read the data file and convert to object
            UsersModel users = UsersModel.ParseFromCsv(File.ReadAllText(dataFilePathCsv), config.B2cExtensionAppClientId);

            foreach (var user in users.Users)
            {
                user.SetB2CProfile(config.TenantId);

                try
                {
                    //Create the user account in the directory
                    User user1 = await graphClient.Users
                                    .Request()
                                    .AddAsync(user);

                    Console.WriteLine($"User '{user.DisplayName}' successfully created.");
                }
                catch (Exception ex)
                {
                    File.AppendAllText("ErrorLogs.txt", $"=============================================================================================\n{user.DisplayName}| {user.Identities.First().IssuerAssignedId}: {ex.Message}");
                   
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }
        }
        
    }
}

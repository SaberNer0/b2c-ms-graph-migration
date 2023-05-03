using ChoETL;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace b2c_ms_graph
{
    public class UsersModel
    {
        public UsersModel(IEnumerable<UserModel> users)
        {
            Users = users;
        }

        [JsonPropertyName("users")]
        public IEnumerable<UserModel> Users { get; set; }

        public static UsersModel ParseFromJson(string json, string b2cExtensionAppClientId)
        {
            var rawUers = JsonSerializer.Deserialize<IEnumerable<UserModel>>(json);

            const string customAttributeName1 = "Role";
            const string customAttributeName2 = "StudioKey";

            // Get the complete name of the custom attribute (Azure AD extension)
            Helpers.B2cCustomAttributeHelper helper = new Helpers.B2cCustomAttributeHelper(b2cExtensionAppClientId);
            string roleAttributeName = helper.GetCompleteAttributeName(customAttributeName1);
            string studioAttributeName = helper.GetCompleteAttributeName(customAttributeName2);

            foreach (var rawUser in rawUers)
            {
                int studioId = int.Parse(rawUser.AdditionalData.FirstOrDefault(x => x.Key == "Key").Value.ToString());

                // Fill custom attributes
                IDictionary<string, object> extensionInstance = new Dictionary<string, object>();
                extensionInstance.Add(roleAttributeName, "photographer");
                extensionInstance.Add(studioAttributeName, studioId);

                rawUser.Identities = new List<ObjectIdentity>()
                { 
                    new ObjectIdentity{ 
                        SignInType = "emailAddress",
                        IssuerAssignedId = rawUser.AdditionalData.FirstOrDefault(x=>x.Key == "Email").Value.ToString()
                    }
                };
                rawUser.DisplayName = rawUser.AdditionalData.FirstOrDefault(x => x.Key == "StudioName").Value.ToString();
                rawUser.AdditionalData = extensionInstance;
            }

            //remove dublicate acconuts
            rawUers = rawUers.GroupBy(x => x.Identities.First().IssuerAssignedId).Select(x => x.First()).ToList();
            return new UsersModel(rawUers);
        }

        public static UsersModel ParseFromCsv(string csvFile, string b2cExtensionAppClientId)
        {
            StringBuilder sb = new StringBuilder();
            using (var p = ChoCSVReader.LoadText(csvFile).WithFirstLineHeader())
            {
                using (var w = new ChoJSONWriter(sb))
                    w.Write(p);
            }
           
            return ParseFromJson(sb.ToString(), b2cExtensionAppClientId);
        }
    }
}
using SMSGateway.Base.Providers.IProviders;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SMSGateway.Base.Providers
{
    public class ConnectionStringProviders:IConnectionStringProviders
    {
        public ConnectionStringProviders() {
            var cs=GetConnectionString();
            UpdateAppSettings(cs);
        }
        public string GetConnectionString()
        {
            // Read config.xml and create connection string
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "", "config.xml");
            XDocument doc = XDocument.Load(configFilePath);

            var optionNode = doc.Root?.Element("option");
            var dbName = optionNode?.Element("db")?.Value;
            var serverName = optionNode?.Element("server")?.Value;
            var username = optionNode?.Element("user")?.Value;
            var password = optionNode?.Element("pass")?.Value;

            return $"Data Source={serverName}; User ID={username}; Password={password}; Initial Catalog={dbName}; max pool size=500000; MultipleActiveResultSets=true;";
            //return $"Server={serverName};Database={dbName};User Id={username};Password={password};";
        }
        public void UpdateAppSettings(string connectionString)
        {
/*for appsettings.json file in bin folder*/
            // Load appsettings.json
            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            var json = File.ReadAllText(appSettingsPath);
            var appSettings = JsonConvert.DeserializeObject<JObject>(json);

            // Update connection string
            appSettings["ConnectionStrings"]["AppDbContextConnection"] = connectionString;

            // Save back to appsettings.json
            File.WriteAllText(appSettingsPath, appSettings.ToString());

            /*for appsettings.json file in bin folder*/

            /*for appsettings.json file in project folder*/
            // Load appsettings.json from project folder
            var appSettingsPathProject = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (File.Exists(appSettingsPathProject))
            {
                var jsonProject = File.ReadAllText(appSettingsPathProject);
                var appSettingsProject = JsonConvert.DeserializeObject<JObject>(jsonProject);

                // Update connection string in project folder appsettings.json
                appSettingsProject["ConnectionStrings"]["AppDbContextConnection"] = connectionString;

                // Save back to appsettings.json in project folder
                File.WriteAllText(appSettingsPathProject, appSettingsProject.ToString());
            }
            /*for appsettings.json file in project folder*/


        }
    }
}

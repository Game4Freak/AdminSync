using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Game4Freak.AdminSync
{
    public class AdminSyncConfiguration : IRocketPluginConfiguration
    {
        public string databaseAddress;
        public string databaseUsername;
        public string databasePassword;
        public string databaseName;
        public string databaseTableName;
        public int databasePort;

        public string messageColor;

        public void LoadDefaults()
        {
            databaseAddress = "127.0.0.1";
            databaseUsername = "unturned";
            databasePassword = "password";
            databaseName = "unturned";
            databaseTableName = "adminsync";
            databasePort = 3306;

            messageColor = "cyan";
        }
    }
}

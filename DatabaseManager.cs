using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game4Freak.AdminSync
{
    public class DatabaseManager
    {
        internal DatabaseManager()
        {
            new I18N.West.CP1250(); //Workaround for database encoding issues with mono
            CheckSchema();
        }

        private MySqlConnection createConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (AdminSync.Instance.Configuration.Instance.databasePort == 0) AdminSync.Instance.Configuration.Instance.databasePort = 3306;
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", AdminSync.Instance.Configuration.Instance.databaseAddress, AdminSync.Instance.Configuration.Instance.databaseName, AdminSync.Instance.Configuration.Instance.databaseUsername, AdminSync.Instance.Configuration.Instance.databasePassword, AdminSync.Instance.Configuration.Instance.databasePort));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return connection;
        }

        public bool isAdmin(string id)
        {
            bool isAdmin = false;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `isadmin` from `" + AdminSync.Instance.Configuration.Instance.databaseTableName + "` where `steamId` = '" + id.ToString() + "';";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    if (result.ToString() == "True")
                        isAdmin = true;
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return isAdmin;
        }

        public void setAdmin(string id, bool isAdmin)
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                int admin = 0;
                if (isAdmin)
                    admin = 1;
                command.CommandText = "update `" + AdminSync.Instance.Configuration.Instance.databaseTableName + "` set `isadmin` = (" + admin.ToString() + ") where `steamId` = '" + id.ToString() + "'; select `isadmin` from `" + AdminSync.Instance.Configuration.Instance.databaseTableName + "` where `steamId` = '" + id.ToString() + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public bool AccountExists(string id)
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                int exists = 0;
                command.CommandText = "SELECT EXISTS(SELECT 1 FROM `" + AdminSync.Instance.Configuration.Instance.databaseTableName + "` WHERE `steamId` ='" + id + "' LIMIT 1);";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Int32.TryParse(result.ToString(), out exists);
                connection.Close();

                if (exists != 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public void CheckSetupAccount(Steamworks.CSteamID id)
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                int exists = 0;
                command.CommandText = "SELECT EXISTS(SELECT 1 FROM `" + AdminSync.Instance.Configuration.Instance.databaseTableName + "` WHERE `steamId` ='" + id + "' LIMIT 1);";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Int32.TryParse(result.ToString(), out exists);
                connection.Close();

                if (exists == 0)
                {
                    command.CommandText = "insert ignore into `" + AdminSync.Instance.Configuration.Instance.databaseTableName + "` (isadmin,steamId,lastUpdated) values(" + 0.ToString() + ",'" + id.ToString() + "',now())";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }

        internal void CheckSchema()
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "show tables like '" + AdminSync.Instance.Configuration.Instance.databaseTableName + "'";
                connection.Open();
                object test = command.ExecuteScalar();

                if (test == null)
                {
                    command.CommandText = "CREATE TABLE `" + AdminSync.Instance.Configuration.Instance.databaseTableName + "` (`steamId` varchar(32) NOT NULL,`isadmin` tinyint(1) NOT NULL DEFAULT '0',`lastUpdated` timestamp NOT NULL DEFAULT NOW() ON UPDATE CURRENT_TIMESTAMP,PRIMARY KEY (`steamId`)) ";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}


using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Game4Freak.AdminSync
{
    public class AdminSync : RocketPlugin<AdminSyncConfiguration>
    {
        public static AdminSync Instance;
        public DatabaseManager Database;
        public const string VERSION = "1.0.1.0";

        protected override void Load()
        {
            Instance = this;
            Database = new DatabaseManager();
            Logger.Log("AdminSync v" + VERSION);

            U.Events.OnPlayerConnected += onPlayerConnected;
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= onPlayerConnected;
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"invalid", "Invalid! Try {0} {1}" },
                    {"no_database_entry", "There is no database entry for {0}" },
                    {"add_admin", "Added: {0} to synced admin" },
                    {"remove_admin", "Removed: {0} from synced admin" },
                };
            }
        }

        private void onPlayerConnected(UnturnedPlayer player)
        {
            Database.CheckSetupAccount(player.CSteamID);
            player.Admin(Database.isAdmin(player.Id));
        }

        public static Int32 getCurrentTime()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime GetDateTime(Int32 timestamp)
        {
            return new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        }
    }
}
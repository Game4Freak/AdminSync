using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Core;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Player;
using UnityEngine;
using Steamworks;
using Logger = Rocket.Core.Logging.Logger;

namespace Game4Freak.AdminSync
{
    public class CommandAdminSync : IRocketCommand
    {
        public string Name
        {
            get { return "sadmin"; }
        }
        public string Help
        {
            get { return "sync admins"; }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Both;
            }
        }

        public string Syntax
        {
            get { return "<add|remove> <playername|playerid>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { "syncadmin", "adminsync" }; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "adminsync" };
            }
        }

        public void Execute(IRocketPlayer caller, params string[] command)
        {
            if (command.Length < 2)
            {
                UnturnedChat.Say(caller, AdminSync.Instance.Translate("invalid", "/"+Name, Syntax), Color.red);
                return;
            }
            UnturnedPlayer target = UnturnedPlayer.FromName(command[1]);
            if (target != null && target.Player != null)
            {
                if (command[0].ToLower() == "add")
                {
                    AdminSync.Instance.Database.setAdmin(target.Id, true);
                    target.Admin(true);
                    UnturnedChat.Say(caller, AdminSync.Instance.Translate("add_admin", target.DisplayName), UnturnedChat.GetColorFromName(AdminSync.Instance.Configuration.Instance.messageColor, Color.green));
                    Logger.Log(AdminSync.Instance.Translate("add_admin", target.DisplayName), ConsoleColor.Yellow);
                    return;
                }
                else if (command[0].ToLower() == "remove")
                {
                    AdminSync.Instance.Database.setAdmin(target.Id, false);
                    target.Admin(false);
                    UnturnedChat.Say(caller, AdminSync.Instance.Translate("remove_admin", target.DisplayName), UnturnedChat.GetColorFromName(AdminSync.Instance.Configuration.Instance.messageColor, Color.green));
                    Logger.Log(AdminSync.Instance.Translate("remove_admin", target.DisplayName), ConsoleColor.Yellow);
                    return;
                }
                else
                {
                    UnturnedChat.Say(caller, AdminSync.Instance.Translate("invalid", "/" + Name, Syntax), Color.red);
                    return;
                }
            }
            else
            {
                if (!AdminSync.Instance.Database.AccountExists(command[1]))
                {
                    UnturnedChat.Say(caller, AdminSync.Instance.Translate("no_database_entry", command[1]), Color.red);
                    return;
                }
                if (command[0].ToLower() == "add")
                {
                    AdminSync.Instance.Database.setAdmin(command[1], true);
                    UnturnedChat.Say(caller, AdminSync.Instance.Translate("add_admin", command[1]), UnturnedChat.GetColorFromName(AdminSync.Instance.Configuration.Instance.messageColor, Color.green));
                    Logger.Log(AdminSync.Instance.Translate("add_admin", command[1]), ConsoleColor.Yellow);
                    return;
                }
                else if (command[0].ToLower() == "remove")
                {
                    AdminSync.Instance.Database.setAdmin(command[1], false);
                    UnturnedChat.Say(caller, AdminSync.Instance.Translate("remove_admin", command[1]), UnturnedChat.GetColorFromName(AdminSync.Instance.Configuration.Instance.messageColor, Color.green));
                    Logger.Log(AdminSync.Instance.Translate("remove_admin", command[1]), ConsoleColor.Yellow);
                    return;
                }
                else
                {
                    UnturnedChat.Say(caller, AdminSync.Instance.Translate("invalid", "/" + Name, Syntax), Color.red);
                    return;
                }
            }
        }
    }
}

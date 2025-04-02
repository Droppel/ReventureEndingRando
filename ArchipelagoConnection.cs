using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using System;
using System.Collections.Generic;

namespace ReventureEndingRando
{
    class ArchipelagoConnection
    {
        public static ArchipelagoSession session;

        public static int requiredEndings;
        public static int treasureRoomSword;
        public static int gemsRandomized;
        public static int gemsAmount;
        public static int gemsRequired;
        public static int hardJumps;
        public static int hardCombat;
        public static int experimentalRegionGraph;

        // Regiongraph info
        public static string spawn;
        public static List<string> itemLocations;


        private readonly string slot;
        private readonly string server;

        public ArchipelagoConnection(string host, string slot)
        {
            string[] hostSplit = host.Split(':');
            session = ArchipelagoSessionFactory.CreateSession(hostSplit[0], int.Parse(hostSplit[1]));
            this.slot = slot;
            this.server = host;
        }

        public bool Connect()
        {
            LoginResult result;

            try
            {
                string password = Plugin.currentPassword;
                if (password.Equals("")) {
                    password = null;
                }
                result = session.TryConnectAndLogin("Reventure", slot, ItemsHandlingFlags.AllItems, password: password);
            } catch (Exception e)
            {
                result = new LoginFailure(e.GetBaseException().Message);
            }

            if (!result.Successful)
            {
                LoginFailure failure = (LoginFailure)result;
                string errorMessage = $"Failed to Connect to {server} as {slot}:";
                foreach (string error in failure.Errors)
                {
                    errorMessage += $"\n    {error}";
                }
                foreach (ConnectionRefusedError error in failure.ErrorCodes)
                {
                    errorMessage += $"\n    {error}";
                }

                Plugin.PatchLogger.LogInfo(errorMessage);
                return false; // Did not connect, show the user the contents of `errorMessage`
            }

            var slotData = session.DataStorage.GetSlotData(ArchipelagoConnection.session.ConnectionInfo.Slot);
            requiredEndings = int.Parse(slotData["endings"].ToString());
            gemsRandomized = int.Parse(slotData["randomizeGems"].ToString());
            gemsAmount = int.Parse(slotData["gemsInPool"].ToString());
            gemsRequired = int.Parse(slotData["gemsRequired"].ToString());
            treasureRoomSword = int.Parse(slotData["treasureSword"].ToString());
            hardJumps = int.Parse(slotData["hardjumps"].ToString());
            hardCombat = int.Parse(slotData["hardcombat"].ToString());

            // Regiongraph info
            experimentalRegionGraph = int.Parse(slotData["experimentalRegionGraph"].ToString());

            if (experimentalRegionGraph != 0) {
                spawn = slotData["spawn"].ToString();
                itemLocations = new List<string>();
                var locationSlotDataNames = new List<string> { "item_Sword", "item_SwordElder", "item_Shovel", "item_Bomb", "item_Shield", "item_MrHugs", "item_Lava Trinket", "item_Hook", "item_Nuke", "item_Whistle" };
                foreach (string name in slotData["itemlocations"].ToString().Split(',')) {
                    itemLocations.Add(name);
                }
            }

            session.Items.ItemReceived += (receivedItemsHelper) => {
                Plugin.itemManager.ReceiveItem();
            };

            //Synchronize
            Plugin.itemManager.Synchronize();

            // Successfully connected, `ArchipelagoSession` (assume statically defined as `session` from now on) can now be used to interact with the server and the returned `LoginSuccessful` contains some useful information about the initial connection (e.g. a copy of the slot data as `loginSuccess.SlotData`)
            var loginSuccess = (LoginSuccessful)result;
            return true;
        }

        public static async void Check_Send_completion()
        {
            var statusUpdatePacket = new StatusUpdatePacket {
                Status = ArchipelagoClientState.ClientGoal
            };
            await session.Socket.SendPacketAsync(statusUpdatePacket);
        }
    }
}

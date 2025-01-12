using Atto;
using System.Collections.Generic;
using ReventureEndingRando.EndingEffects;

namespace ReventureEndingRando {
    public class ItemManager {

        Dictionary<long, int> itemsReceived;
        private int lastItemReceived;

        private readonly ISaveSlotService saveService;
        private readonly int currentSlot;

        public ItemManager(int saveSlot) {
            saveService = Core.Get<ISaveSlotService>();
            currentSlot = saveSlot;

            itemsReceived = new Dictionary<long, int>();
            saveService.Load<Dictionary<long, int>>(currentSlot, "unlockedItems", new Dictionary<long, int>()).Then((dict) => {
                itemsReceived = dict;
            });

            saveService.Load<int>(currentSlot, "lastItemReceived", -1).Then((last) => {
                lastItemReceived = last;
            });
        }

        public void Synchronize() {

            for (int i = 0; i < lastItemReceived; i++) {
                var seen = ArchipelagoConnection.session.Items.DequeueItem();
            }


            while (ReceiveItem()) { }
        }

        public bool ReceiveItem() {
            var itemsQueue = ArchipelagoConnection.session.Items;
            if (!itemsQueue.Any()) {
                return false;
            }

            var item = itemsQueue.DequeueItem();
            string itemName = ArchipelagoConnection.session.Items.GetItemName(item.Item);
            string playerName = ArchipelagoConnection.session.Players.GetPlayerAlias(item.Player);
            Plugin.DisplayText($"Received {itemName} from {playerName}!");

            ItemManager itemManager = Plugin.itemManager;
            itemManager.AddItem(item.Item);
            return true;
        }

        private void AddItem(long itemID) {
            if (itemsReceived.ContainsKey(itemID)) {
                itemsReceived[itemID] += 1;
            } else {
                itemsReceived.Add(itemID, 1);
            }
            
            lastItemReceived += 1;

            saveService.Save<int>(currentSlot, "lastItemReceived", lastItemReceived);
            saveService.Save<Dictionary<long, int>>(currentSlot, "unlockedItems", itemsReceived);

            EndingEffect ee = EndingEffect.InitFromEnum((EndingEffectsEnum)(itemID - Plugin.reventureItemOffset));
            try {
                ee.ActivateEffect(itemsReceived[itemID], false);
            } catch {  }
            return;
        }

        public int GetItemCount(long id) {
            if (itemsReceived.TryGetValue(id, out int val)) {
                return val;
            }
            return 0;
        }
        public int GetItemCount(EndingEffects.EndingEffectsEnum effect) {
            return GetItemCount(Plugin.reventureItemOffset + (long)effect);
        }
    }
}

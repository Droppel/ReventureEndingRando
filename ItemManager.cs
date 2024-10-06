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
            saveService.Load<Dictionary<long, int>>(currentSlot, "unlockedItems").Then((dict) => {
                itemsReceived = dict;
            });

            saveService.Load<int>(currentSlot, "lastItemReceived").Then((last) => {
                lastItemReceived = last;
            });
        }
                
        public void AddItem(long itemID, int itemIndex) {
            if (itemIndex <= lastItemReceived) {
                return;
            }

            if (itemsReceived.ContainsKey(itemID)) {
                itemsReceived[itemID] += 1;
            } else {
                itemsReceived.Add(itemID, 1);
            }
            
            lastItemReceived = itemIndex;

            saveService.Save<int>(currentSlot, "lastItemReceived", lastItemReceived);
            saveService.Save<Dictionary<long, int>>(currentSlot, "unlockedItems", itemsReceived);

            EndingEffect ee = EndingEffect.InitFromEnum((EndingEffectsEnum)(itemID - Plugin.reventureItemOffset));
            ee.ActivateEffect(itemsReceived[itemID], false);
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

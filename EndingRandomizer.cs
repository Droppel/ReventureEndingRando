using Archipelago.MultiClient.Net.Models;
using Newtonsoft.Json;
using ReventureEndingRando.EndingEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace ReventureEndingRando
{
    public class EndingRandomizer
    {
        public Dictionary<EndingTypes, EndingEffectsEnum> randomization;

        public EndingRandomizer()
        {
            randomization = new Dictionary<EndingTypes, EndingEffectsEnum>();
            //if (true)
            if (!LoadState())
            {
                Randomize();
                StoreState();
            }
        }

        public void Randomize()
        {
            Seed seed = new Seed();
            randomization = seed.randomization;
            Plugin.PatchLogger.LogInfo("Finished generating Seed");
        }

        public List<EndingEffectsEnum> UpdateWorld(IProgressionService progression)
        {
            List<EndingEffectsEnum> enabledEffect = new List<EndingEffectsEnum>();
            foreach (KeyValuePair<EndingTypes, EndingEffectsEnum> entry in randomization)
            {
                EndingEffect ee = EndingEffect.InitFromEnum(entry.Value);
                bool endingAchieved = progression.IsEndingUnlocked(entry.Key);
                if (ee != null) {
                    ee.ActivateEffect(endingAchieved);
                    if (endingAchieved)
                    {
                        enabledEffect.Add(entry.Value);
                    }
                }
            }

            //Update UI
            GameObject versionTextObj = GameObject.Find("Canvasses/OverlayCanvas/GamePanel/ZonePanel/zoneText/versionText");
            TextMeshProUGUI versionText = versionTextObj.GetComponent<TextMeshProUGUI>();
            versionText.SetText($"{versionText.text}; Rando: {MyPluginInfo.PLUGIN_VERSION}");

            //Show Last Unlocks


            return enabledEffect;
        }

        public List<EndingEffectsEnum> UpdateWorldArchipelago()
        {
            List<EndingEffectsEnum> enabledEffect = new List<EndingEffectsEnum>();

            List<long> receivedIDs = new List<long>();
            foreach (NetworkItem item in ArchipelagoConnection.session.Items.AllItemsReceived)
            {
                receivedIDs.Add(item.Item);
            }

            foreach (EndingEffectsEnum effect in Enum.GetValues(typeof(EndingTypes)).Cast<EndingTypes>().ToList())
            {
                EndingEffect ee = EndingEffect.InitFromEnum(effect);
                bool effectReceived = receivedIDs.Contains(10000 + (int) effect);
                if (ee != null)
                {
                    ee.ActivateEffect(effectReceived);
                    if (effectReceived)
                    {
                        enabledEffect.Add(effect);
                    }
                }
            }

            //Update UI
            GameObject versionTextObj = GameObject.Find("Canvasses/OverlayCanvas/GamePanel/ZonePanel/zoneText/versionText");
            TextMeshProUGUI versionText = versionTextObj.GetComponent<TextMeshProUGUI>();
            versionText.SetText($"{versionText.text}; Rando: {MyPluginInfo.PLUGIN_VERSION}");

            //Show Last Unlocks


            return enabledEffect;
        }

        public bool LoadState()
        {
            try
            {
                randomization = JsonConvert.DeserializeObject<Dictionary<EndingTypes, EndingEffectsEnum>>(File.ReadAllText("randomizer.txt"));
                return true;
            } catch (Exception e)
            {
                Plugin.PatchLogger.LogInfo($"{e}");
                return false;
            }
        }

        public void StoreState()
        {
            string json = JsonConvert.SerializeObject(randomization);
            File.WriteAllText("randomizer.txt", json);
        }

        public override string ToString()
        {
            string o = "";
            foreach (KeyValuePair<EndingTypes, EndingEffectsEnum> entry in randomization)
            {
                o += $"{entry.Key}: {entry.Value}\n";
            }
            return o;
        }
    }
}

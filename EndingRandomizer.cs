using Archipelago.MultiClient.Net.Models;
using Atto;
using Newtonsoft.Json;
using ReventureEndingRando.EndingEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        }

        public static void UpdateWorldArchipelago()
        {
            ItemManager itemManager = Plugin.itemManager;

            foreach (EndingEffectsEnum effect in Enum.GetValues(typeof(EndingEffectsEnum)).Cast<EndingEffectsEnum>().ToList())
            {
                EndingEffect ee = EndingEffect.InitFromEnum(effect);

                int effectReceived = itemManager.GetItemCount(effect);
                if (ee != null)
                {
                    ee.ActivateEffect(effectReceived, true);
                } else {
                    //Plugin.PatchLogger.LogError($"EE is null for {effect}");
                }
            }

            // Handle Gems
            if (ArchipelagoConnection.gemsRandomized == 1)
            {
                //Randomized Gems, Disable Vanilla Locations
                GameObject.Find("World/Items/TetraGems/EarthGem").SetActive(false);
                GameObject.Find("World/Items/TetraGems/WindGem").SetActive(false);
                GameObject.Find("World/Items/TetraGems/WaterGem").SetActive(false);
                GameObject.Find("World/Items/TetraGems/FireGem").SetActive(false);
            }

            //Update UI
            GameObject versionTextObj = GameObject.Find("Canvasses/OverlayCanvas/GamePanel/ZonePanel/zoneText/versionText");
            TextMeshProUGUI versionText = versionTextObj.GetComponent<TextMeshProUGUI>();
            versionText.SetText($"{versionText.text}; Rando: {MyPluginInfo.PLUGIN_VERSION}");

            //Permanent changes
            //Disable cannon ending requirement and Add the missing ones to castle cannon
            Cannon townToShopCannon = GameObject.Find("World/Interactables/Cannons/TownToShopCannon/33_ShootCannonballToShop_End").GetComponent<Cannon>();
            ((EndingCountRequirement) townToShopCannon.requirementsToFail[0]).endingsUnlockedCount = 0;
            Cannon fortressToTownCannon = GameObject.Find("World/Interactables/Cannons/FortressToTownCannon/34_ShootCannonballToTown_End").GetComponent<Cannon>();
            ((EndingCountRequirement) fortressToTownCannon.requirementsToFail[0]).endingsUnlockedCount = 0;
            Cannon shopToFortress = GameObject.Find("World/Interactables/Cannons/ShopToFortressCannon/32_ShootCannonballToCastle_End").GetComponent<Cannon>();
            ((EndingCountRequirement) shopToFortress.requirementsToFail[2]).endingsUnlockedCount = 0;
            Cannon castleToDarkCastle = GameObject.Find("World/Interactables/Cannons/CastleToFortressCannon/CannonContents").GetComponent<Cannon>();
            castleToDarkCastle.requirementsToFail = shopToFortress.requirementsToFail;

            // Change Ultimate Door signs texts
            DisplayChat[] signLeft = Resources.FindObjectsOfTypeAll<DisplayChat>().Where(obj => obj.transform.position.x == 194.0 && obj.transform.position.y == -24.0).ToArray();
            DisplayChat[] signRight= Resources.FindObjectsOfTypeAll<DisplayChat>().Where(obj => obj.transform.position.x == 201.0 && obj.transform.position.y == -24.0).ToArray();
            //Perseverance and Patience yield the ultimate reward.
            if (ArchipelagoConnection.requiredEndings == 0)
            {
                signLeft[0].textToDisplay = "Perseverance and Patience yield... Wait ZERO? REALLY?";
            } else if (ArchipelagoConnection.requiredEndings == 1)
            {
                signLeft[0].textToDisplay = "Perseverance and Patience yield the ultimate reward. Or 1 Ending will do to";
            } else
            {
                signLeft[0].textToDisplay = "Perseverance and Patience yield the ultimate reward. Or " + ArchipelagoConnection.requiredEndings +" Endings will do to";
            }
            signLeft[0].useString = true;
            //This temple is guarded by the 4 gems that keep nature in balance: Earth, Water, Wind and Fire
            int requiredGems = (ArchipelagoConnection.gemsAmount * ArchipelagoConnection.gemsRequired) / 100;
            string gemText = "";

            string[] gemNames = { "Earth", "Water", "Wind", "Fire", "Air", "Normal", "Electric", "Grass", "Ice", "Fighting",
                "Poison", "Ground", "Flying", "Psychic", "Bug", "Rock", "Ghost", "Dragon", "Dark", "Steel",
                "Fairy", "Stellar", "Metal", "Eldritch", "Aura", "Mind", "Light", "Death", "Energy", "Magic",
                "Void", "Life", "Crystal", "Stone", "Leaf", "Tree", "Plant", "Shadow", "Sky", "Ocean"};
            if (requiredGems == 0)
            {
                gemText = "This temple is not guarded.";

            } else if (requiredGems == 1)
            {
                string randomElement = gemNames[UnityEngine.Random.RandomRangeInt(0, 40)];
                gemText = "This temple is guarded by the 1 gem that keeps nature in balance: " + randomElement;
            } else
            {
                List<string> selectedGems = gemNames.OrderBy(x => UnityEngine.Random.Range(0, 40)).Take(requiredGems).ToList();
                gemText = "This temple is guarded by the " + requiredGems + " gems that keep nature in balance:";
                for (int i = 0; i < requiredGems - 1; i++)
                {
                    gemText += " " + selectedGems[i] + ",";
                }
                gemText = gemText.Substring(0, gemText.Length - 1) + " and " + selectedGems[requiredGems - 1];
            }
            signRight[0].textToDisplay = gemText;
            signRight[0].useString = true;
            return;
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

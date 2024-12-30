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
    public class EndingRandomizer {
        private static Dictionary<string, Vector2> spawnLocations = new Dictionary<string, Vector2>{
                {"LonksHouse", new Vector2(132.5f, 7.5f)},
                {"Elder", new Vector2(108.5f, 20.5f)},
                {"Chicken", new Vector2(108.5f, 27.5f)},
                {"Shovel", new Vector2(105.5f, 8.5f)},
                {"CastleFirstFloor", new Vector2(160.5f, 7.5f)},
                {"CastleShieldChest", new Vector2(169.5f, 10.5f)},
                {"CastleMapChest", new Vector2(155.5f, 10.5f)},
                {"CastleRoof", new Vector2(160.5f, 18.5f)},
                {"PrincessRoom", new Vector2(160.5f, 13.5f)},
                {"VolcanoTopExit", new Vector2(82.5f, 24.5f)},
                {"LavaTrinket", new Vector2(96.5f, 11.5f)},
                {"VolcanoDropStone", new Vector2(78.5f, -3.5f)},
                {"VolcanoBridge", new Vector2(96.5f, 0.5f)},
                {"BelowVolcanoBridge", new Vector2(102.5f, -8.5f)},
                {"Sewer", new Vector2(157.5f, 4.5f)},
                // {"leftOfDragon", new Vector2()}, // Cannot escape from here as soon as dragon spawns (So logically never)
                {"RightOfDragon", new Vector2(151.5f, -22.5f)},
                {"GoldRoom", new Vector2(144.5f, -18.5f)},
                {"SewerPipe", new Vector2(144.5f, -8.5f)},
                {"VolcanoGeyser", new Vector2(173.5f, -26.5f)},
                {"UltimateDoor", new Vector2(190.5f, -24.5f)},
                {"CastleMinions", new Vector2(186.5f, 7.5f)},
                {"Cloud", new Vector2(188.5f, 51.5f)},
                {"BelowCastleBridge", new Vector2(174.5f, 1.5f)},
                {"SecretPathMoatWell", new Vector2(178.5f, 4.5f)},
                {"CastleMoat", new Vector2(176.5f, -6.5f)},
                // {"barn", new Vector2()},// Cannot escape from here
                {"BehindShopBush", new Vector2(70.5f, 0.5f)},
                {"Shop", new Vector2(33.5f, 0.5f)},
                {"ShopRoof", new Vector2(28.5f, 12.5f)},
                {"ShopLake", new Vector2(54.5f, 0.5f)},
                {"Ocean", new Vector2(10.5f, 17.5f)},
                {"NukeStorage", new Vector2(36.5f, 4.5f)},
                {"HookArea", new Vector2(205.5f, 7.5f)},
                {"AboveHook", new Vector2(205.5f, 13.5f)},
                {"AboveAboveHook", new Vector2(210.5f, 21.5f)},
                {"CastleCannonToShop", new Vector2(199.5f, 21.5f)},
                {"Altar", new Vector2(235.5f, 25.5f)},
                {"Bomb", new Vector2(215.5f, 7.5f)},
                {"FishingBridge", new Vector2(211.5f, -4.5f)},
                {"BelowFishingBridge", new Vector2(202.5f, -6.5f)},
                {"FishingRod", new Vector2(212.5f, -0.5f)},
                {"MountainLeftOutcrop", new Vector2(219.5f, 44.5f)},
                {"MountainTop", new Vector2(256.5f, 73.5f)},
                {"MountainTreasure", new Vector2(285.5f, 55.5f)},
                {"Levers", new Vector2(282.5f, 25.5f)},
                {"GreatWaterfall", new Vector2(261.5f, 15.5f)},
                {"GreatWaterfallBottom", new Vector2(271.5f, -2.5f)},
                {"FortressMoat", new Vector2(328.5f, -7.5f)},
                {"FairyFountain", new Vector2(345.5f, -13.5f)},
                {"FortressBridgeButton", new Vector2(345.5f, 2.5f)},
                {"SecretAboveBomb", new Vector2(233.5f, 20.5f)},
                {"WaterFalls", new Vector2(254.5f, -15.5f)},
                {"AboveWaterfalls", new Vector2(255.5f, -4.5f)},
                {"Whistle", new Vector2(281.5f, 14.5f)},
                {"WhistleAltar", new Vector2(303.5f, 7.5f)},
                {"BelowLeapOfFaith", new Vector2(300.5f, 29.5f)},
                {"Elevator", new Vector2(368.5f, 7.5f)},
                {"FortressRoof", new Vector2(411.5f, 61.5f)},
                {"Anvil", new Vector2(384.5f, 42.5f)},
                {"Princess", new Vector2(419.5f, 42.5f)},
                {"FireEscape", new Vector2(376.5f, 29.5f)},
                {"FortressTreasure", new Vector2(403.5f, 22.5f)},
                {"RightOfFortress", new Vector2(424.5f, -4.5f)},
                // {"darkstone", new Vector2()}, // Cannot escape from here, because the lever is not pulled
            };

        public EndingRandomizer()
        {
        }

        public static void UpdateWorldArchipelago()
        {
            GameObject hero = GameObject.Find("Hero");
            Plugin.PatchLogger.LogInfo(ArchipelagoConnection.spawn);
            hero.transform.position = spawnLocations[ArchipelagoConnection.spawn];

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
    }
}

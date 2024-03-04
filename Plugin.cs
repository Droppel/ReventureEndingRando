using Archipelago.MultiClient.Net.Models;
using Atto;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ReventureEndingRando.EndingEffects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ReventureEndingRando
{

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {

        public static long reventureItemOffset = 900270000;
        public static long reventureEndingOffset = 900271000;

        public static bool isRandomizer = false;
        public static string lastUnlocksText = "Last Unlocked: ";

        public static GameObject archipelagoSettings;
        public static bool archipelagoSettingsActive = false;
        public static string currentHost;
        public static string currentSlot;

        private int lastItemListSize = 0;

        Harmony harmony;
        public static ManualLogSource PatchLogger;

        public static List<EndingEffectsEnum> endingEffects;

        public static Dictionary<int, string> saves;

        private void Awake()
        {
            // Plugin startup logic
            PatchLogger = Logger;

            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Logger.LogInfo($"Randomized: {randomizer.ToString()}");

            //Read saves
            saves = new Dictionary<int, string>();
            if (File.Exists("randosaves"))
            {
                string saveFile = File.ReadAllText("randosaves");
                foreach (string line in saveFile.Split('\n'))
                {
                    if (line == "")
                    {
                        continue;
                    }
                    string[] lineSplit = line.Split('=');
                    saves.Add(int.Parse(lineSplit[0]), lineSplit[1]);
                }
            }

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} Version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (archipelagoSettingsActive)
                {
                    //Plugin.PatchLogger.LogInfo(Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text);
                    currentHost = Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;
                    currentSlot = Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;
                } else
                {
                    GameObject archipelagoPanelOptions = Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
                    GameObject archipelagoHostOption = archipelagoPanelOptions.transform.GetChild(0).gameObject;
                    archipelagoHostOption.SetActive(true);

                    GameObject archipelagoSlotOption = archipelagoPanelOptions.transform.GetChild(1).gameObject;
                    archipelagoSlotOption.SetActive(true);
                }
                archipelagoSettingsActive = !archipelagoSettingsActive;
                archipelagoSettings.SetActive(archipelagoSettingsActive);
            }

            if (ArchipelagoConnection.session == null)
            {
                return;
            }
            // Only run when connected
            int currentItemListSize = ArchipelagoConnection.session.Items.AllItemsReceived.Count();
            if (currentItemListSize > lastItemListSize)
            {
                lastItemListSize = currentItemListSize;

                //Update Text
                lastUnlocksText = "Last unlocked: ";
                foreach (NetworkItem item in ArchipelagoConnection.session.Items.AllItemsReceived.Skip(Math.Max(0, currentItemListSize - 3)))
                {
                    lastUnlocksText += ((EndingEffectsEnum) (item.Item - reventureItemOffset)).ToString() + ",";
                }
                lastUnlocksText = lastUnlocksText.Remove(lastUnlocksText.Length - 1);

                TextMeshProUGUI text = GameObject.Find("Canvasses/OverlayCanvas/GamePanel/ZonePanel/zoneText").GetComponent<TextMeshProUGUI>();
                text.text = lastUnlocksText;
            }
        }

        private void OnDestroy()
        {
            Destroy(archipelagoSettings);
            ArchipelagoConnection.session.Socket.DisconnectAsync();
            harmony.UnpatchSelf();
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} unloaded!");
        }

        public static void WriteConnectionInfos()
        {
            string output = "";
            foreach (int slot in saves.Keys)
            {
                output += slot + "=" + saves[slot] + "\n";
            }
            output = output.Substring(0, output.Length - 1);
            File.WriteAllText("randosaves", output);
        }
    }

    // Allow skipping endings
    [HarmonyPatch(typeof(EndingProvider))]
    public class EndingProviderPatch
    {
        [HarmonyPatch("LoadEnding", new Type[] { typeof(EndingCinematicConfiguration), typeof(bool), typeof(Action) })]
        private static bool Prefix(ref EndingCinematicConfiguration configuration)
        {
            if (!Plugin.isRandomizer)
            {
                return true;
            }

            configuration.skippable = true;

            //Report to Archipelago
            ArchipelagoConnection.session.Locations.CompleteLocationChecks(Plugin.reventureEndingOffset + (long) configuration.ending);
            if (configuration.ending == EndingTypes.UltimateEnding)
            {
                ArchipelagoConnection.Check_Send_completion();
            }
            return true;
        }
    }

    // Allow skipping of initial Text
    [HarmonyPatch(typeof(SessionProvider))]
    public class SessionProviderPatch
    {
        [HarmonyPatch("IsInitialTextReaded", new Type[] { typeof(EndingTypes) })]
        private static bool Prefix(ref bool __result)
        {
            if (!Plugin.isRandomizer)
            {
                return true;
            }

            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(GameplayDirector))]
    public class GameplayDirectorPatch
    {
        [HarmonyPatch("Start", new Type[] { })]
        private static void Postfix()
        {
            if (!Plugin.isRandomizer)
            {
                return;
            }

            Plugin.endingEffects = EndingRandomizer.UpdateWorldArchipelago();
            return;
        }
    }

    [HarmonyPatch(typeof(UltimateDoor))]
    public class UltimateDoorPatch
    {
        [HarmonyPatch("Start", new Type[] { })]
        private static void Postfix(UltimateDoor __instance)
        {
            if (!Plugin.isRandomizer)
            {
                return;
            }

            IProgressionService progression = Core.Get<IProgressionService>();

            var uDoorEndingsUnlocked = typeof(UltimateDoor).GetField("unlockedEndings", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            uDoorEndingsUnlocked.SetValue(__instance, progression.UnlockedEndingsCount >= ArchipelagoConnection.requiredEndings ? 99 : 6);

            return;
        }
    }

    [HarmonyPatch(typeof(CameraZoneText))]
    public class CameraZoneTextPatch
    {
        [HarmonyPatch("RefreshText", new Type[] { })]
        private static bool Prefix(CameraZoneText __instance)
        {
            if (!Plugin.isRandomizer)
            {
                return true;
            }

            var field = typeof(CameraZoneText).GetField("cameraZone", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            TextMeshProUGUI cameraZone = (TextMeshProUGUI) field.GetValue(__instance);
            cameraZone.text = Plugin.lastUnlocksText;
            return false;
        }
    }

    [HarmonyPatch(typeof(SuperLonkTransformationTrigger))]
    public class SuperLonkTransformationTriggerPatch
    {
        [HarmonyPatch("RunMetamorphosis", new Type[] { })]
        private static void Postfix()
        {
            if (!Plugin.isRandomizer)
            {
                return;
            }

            GameObject goodCrops = GameObject.Find("World/PersistentElements/GoodCrops");
            goodCrops.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(SaveSlotController))]
    public class SaveSlotSelectPatch
    {
        [HarmonyPatch("OnClick", new Type[] { })]
        private static bool Prefix(SaveSlotController __instance)
        {
            if (ArchipelagoConnection.session != null)
            {
                ArchipelagoConnection.session.Socket.DisconnectAsync();
            }
            int slotNumber = __instance.GetDisplaySlotNumber() - 1;

            var isEmptyField = typeof(SaveSlotController).GetField("isEmpty", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            if (!(bool)isEmptyField.GetValue(__instance))
            {
                if (Plugin.saves.ContainsKey(slotNumber))
                {
                    Plugin.isRandomizer = true;
                    //foreach (int key in Plugin.saves.Keys)
                    //{
                    //    Plugin.PatchLogger.LogInfo($"{key}: {Plugin.saves[key]}");
                    //}
                    string[] connectionInfo = Plugin.saves[slotNumber].Split(';');
                    ArchipelagoConnection archipelagoConnection = new ArchipelagoConnection(connectionInfo[0], connectionInfo[1]);
                    archipelagoConnection.Connect();
                } else
                {
                    Plugin.isRandomizer = false;
                }
                return true;
            } else
            {
                // Open Connection menu
                string host = Plugin.currentHost;
                string slot = Plugin.currentSlot;
                Plugin.isRandomizer = true;
                ArchipelagoConnection archipelagoConnection = new ArchipelagoConnection(host, slot);
                archipelagoConnection.Connect();
                Plugin.saves[slotNumber] = host + ";" + slot;
                Plugin.WriteConnectionInfos();
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(TitleDirector))]
    public class TitleDirectorPatch
    {
        [HarmonyPatch("Start", new Type[] { })]
        private static void Postfix(GameObject __instance)
        {
            // Change Options menu
            if (Plugin.archipelagoSettings != null)
            {
                GameObject.DestroyImmediate(Plugin.archipelagoSettings);
            }
            GameObject globalCanvas = GameObject.Find("GlobalCanvas(Clone)");
            Plugin.archipelagoSettings = GameObject.Instantiate(globalCanvas.transform.GetChild(2).gameObject, globalCanvas.transform);
            Plugin.archipelagoSettings.name = "Archipelago";
            Plugin.archipelagoSettings.SetActive(true);
            GameObject.DestroyImmediate(Plugin.archipelagoSettings.GetComponent<OptionsController>());
            GameObject archipelagoPanel = Plugin.archipelagoSettings.transform.GetChild(0).gameObject; // Options Panel
            GameObject archipelagoPanelTabs = archipelagoPanel.transform.GetChild(0).gameObject; // Tabs
            GameObject.DestroyImmediate(archipelagoPanelTabs.GetComponent<OptionTabs>());
            archipelagoPanelTabs.transform.GetChild(3) // Stream
                .GetChild(0) // Text
                .gameObject.GetComponent<TextMeshProUGUI>().SetText("Archipelago");
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelTabs.transform.GetChild(0) // Stream
                .GetComponent<OptionTabElement>());
            GameObject archipelagoPanelPanels = archipelagoPanel.transform.GetChild(1).gameObject;
            GameObject.DestroyImmediate(archipelagoPanelPanels.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelPanels.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelPanels.transform.GetChild(0).gameObject);

            GameObject archipelagoPanelOptions = archipelagoPanelPanels.transform.GetChild(0).gameObject; // Stream Options
            archipelagoPanelOptions.SetActive(true);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(0).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);
            GameObject.DestroyImmediate(archipelagoPanelOptions.transform.GetChild(2).gameObject);

            GameObject archipelagoHostOption = archipelagoPanelOptions.transform.GetChild(0).gameObject;
            archipelagoHostOption.name = "Host Option";
            GameObject.DestroyImmediate(archipelagoHostOption.GetComponent<OptionInputParam>());
            GameObject.DestroyImmediate(archipelagoHostOption.GetComponent<OptionActiveWatcher>());
            GameObject.DestroyImmediate(archipelagoHostOption.GetComponent<OptionActiveWatcher>());
            archipelagoHostOption.SetActive(true);
            archipelagoHostOption.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("Host/Port");
            archipelagoHostOption.transform.GetChild(1) //Options Container
                .GetChild(0) //Input
                .GetChild(0) //Text Area
                .GetChild(1) //Placeholder
                .gameObject.GetComponent<TextMeshProUGUI>().SetText("host:port");

            GameObject archipelagoSlotOption = archipelagoPanelOptions.transform.GetChild(1).gameObject;
            archipelagoSlotOption.name = "Slot Option";
            GameObject.DestroyImmediate(archipelagoSlotOption.GetComponent<OptionInputParam>());
            GameObject.DestroyImmediate(archipelagoSlotOption.GetComponent<OptionActiveWatcher>());
            GameObject.DestroyImmediate(archipelagoSlotOption.GetComponent<OptionActiveWatcher>());
            archipelagoSlotOption.SetActive(true);
            archipelagoSlotOption.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("Slot");
            archipelagoSlotOption.transform.GetChild(1) //Options Container
                .GetChild(0) //Input
                .GetChild(0) //Text Area
                .GetChild(1) //Placeholder
                .gameObject.GetComponent<TextMeshProUGUI>().SetText("slot");

            GameObject buttonGO = archipelagoPanel.transform.GetChild(2).GetChild(0).gameObject;
            GameObject.DestroyImmediate(buttonGO.GetComponent<ButtonContentPusher>());
            buttonGO.SetActive(true);
            Button button = buttonGO.AddComponent<Button>();
            button.onClick.AddListener(() => {
                Plugin.currentHost = Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;
                Plugin.currentSlot = Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;

                Plugin.archipelagoSettingsActive = false;
                Plugin.archipelagoSettings.SetActive(false);
            });
            Plugin.archipelagoSettings.SetActive(false);
        }
    }

    //[HarmonyPatch(typeof(GameObject))]
    //public class GameObjectPatch
    //{
    //    [HarmonyPatch("SetActive", new Type[] { typeof(bool) })]
    //    private static void Postfix(GameObject __instance)
    //    {
    //        if (__instance.name == "TreasureChest_Map" || __instance.name == "TreasureChest_Compass")
    //        {
    //            Plugin.PatchLogger.LogInfo($"Gameobject: {__instance.name}");
    //            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
    //            Plugin.PatchLogger.LogInfo(t.ToString());
    //        }
    //        return;
    //    }
    //}
}
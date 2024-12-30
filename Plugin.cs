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
using UnityEngine.EventSystems;
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

        public static GameObject archipelagoMenu;
        public static bool archipelagoSettingsActive = false;
        public static string currentHost = "";
        public static string currentSlot = "";
        public static string currentPassword = "";

        public static bool inMenu = false;

        private int lastItemListSize = 0;

        Harmony harmony;
        public static ManualLogSource PatchLogger;

        public static Dictionary<int, string> saves;

        public static ItemManager itemManager;
        public static Logic logic;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Not unused, will be executed by Unity")]
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

            logic = new Logic();
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} Version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Not unused, will be executed by Unity")]
        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.F5))
            //{
            //    if (archipelagoSettingsActive)
            //    {
            //        //Plugin.PatchLogger.LogInfo(Plugin.archipelagoSettings.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text);
            //        currentHost = Plugin.archipelagoMenu.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;
            //        currentSlot = Plugin.archipelagoMenu.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>().text;
            //    } else
            //    {
            //        if (Plugin.archipelagoMenu == null) {
            //            ReventureGUI.SetupLoginGUINative();
            //        }
            //        GameObject archipelagoPanelOptions = Plugin.archipelagoMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
            //        GameObject archipelagoHostOption = archipelagoPanelOptions.transform.GetChild(0).gameObject;
            //        archipelagoHostOption.SetActive(true);

            //        GameObject archipelagoSlotOption = archipelagoPanelOptions.transform.GetChild(1).gameObject;
            //        archipelagoSlotOption.SetActive(true);
            //    }
            //    archipelagoSettingsActive = !archipelagoSettingsActive;
            //    archipelagoMenu.SetActive(archipelagoSettingsActive);
            //}

            //if (Input.GetKeyDown(KeyCode.F7)) {

            //    ILocalizationParametersService paramservice= Core.Get<ILocalizationParametersService>();
            //    Plugin.PatchLogger.LogInfo(paramservice[LocalizationParameterKeys.hero]);

            //    paramservice[LocalizationParameterKeys.hero] = "Hallelujah";
            //}

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

        public void OnGUI() {
            ReventureGUI.SetupLoginGUIIMGUI();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Not unused, will be executed by Unity")]
        private void OnDestroy()
        {
            harmony.UnpatchSelf();
            if (archipelagoMenu != null) {
                Destroy(archipelagoMenu);
            }
            if (ArchipelagoConnection.session != null) {
                ArchipelagoConnection.session.Socket.DisconnectAsync();
            }
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

    //Disable Extrea swords, if one is picked up
    [HarmonyPatch(typeof(TreasureItem))]
    public class TreasureItemPatch
    {
        [HarmonyPatch("OnItemPicked", new Type[] { })]
        private static void Postfix(TreasureItem __instance)
        {
            if (__instance.ItemGrantedPrefab.ItemType == ItemTypes.Boomerang)
            {
                // Fix Boomerang Audio
                GameObject boomerangItem = GameObject.Find("Hero/Items/Boomerang(Clone)");
                Plugin.PatchLogger.LogInfo("asdasd");
                Plugin.PatchLogger.LogInfo(boomerangItem);
                AudioSource boomerangAudio = boomerangItem.GetComponent<AudioSource>();
                Plugin.PatchLogger.LogInfo(boomerangAudio);
                AudioSource audioHero = GameObject.Find("Hero").GetComponent<AudioSource>();
                Plugin.PatchLogger.LogInfo(audioHero);
                Plugin.PatchLogger.LogInfo(audioHero.outputAudioMixerGroup);
                Plugin.PatchLogger.LogInfo(boomerangAudio.outputAudioMixerGroup);
                boomerangAudio.outputAudioMixerGroup = audioHero.outputAudioMixerGroup;
                Plugin.PatchLogger.LogInfo(boomerangAudio.outputAudioMixerGroup);
                return;
            }

            if (__instance.ItemGrantedPrefab.ItemType != ItemTypes.Sword)
            {
                return;
            }
            Plugin.PatchLogger.LogInfo($"{ __instance.ItemGrantedPrefab }");
            //Treasureroom Sword
            GameObject treasureSword = GameObject.Find("World/PersistentElements/TreasureLonk/Item Sword");
            if (treasureSword != null)
            {
                treasureSword.SetActive(false);
            }

            //Mountain Sword
            GameObject itemSword = GameObject.Find("World/Items/Sword Item Pedestal/Item Sword");
            if (itemSword != null)
            {
                itemSword.SetActive(false);
            }

            //Home Sword
            GameObject swordChest = GameObject.Find("World/Items/SwordAtHome/TreasureChest_Sword");
            if (swordChest != null)
            {
                TreasureChest tChest = swordChest.GetComponent<TreasureChest>();
                tChest.Open();
            }
        }
    }

    // Allow skipping endings
    [HarmonyPatch(typeof(EndingProvider))]
    public class EndingProviderPatch
    {
        [HarmonyPatch("LoadEnding", new Type[] { typeof(EndingCinematicConfiguration), typeof(bool), typeof(Action)})]
        private static bool Prefix(ref EndingCinematicConfiguration configuration)
        {
            if (!Plugin.isRandomizer)
            {
                return true;
            }

            if (Core.Get<IProgressionService>().EndingsEnabled > 0) {
                configuration.skippable = true;
            }
            return true;
        }
    }

    // Report Unlocked Endings to AP
    [HarmonyPatch(typeof(ProgressionProvider))]
    public class ProgressionProviderPatch {
        [HarmonyPatch("UnlockEnding", new Type[] { typeof(EndingTypes) })]
        private static bool Prefix(ref ProgressionProvider __instance, ref EndingTypes ending) {
            if (!Plugin.isRandomizer) {
                return true;
            }

            if (__instance.IsEndingUnlocked(ending)) {
                return true;
            }

            //Report to Archipelago
            ArchipelagoConnection.session.Locations.CompleteLocationChecks(Plugin.reventureEndingOffset + (long)ending);
            if (ending == EndingTypes.UltimateEnding) {
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
            Plugin.inMenu = false;

            EndingRandomizer.UpdateWorldArchipelago();
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
            ArchipelagoConnection.Check_Send_completion();
        }
    }

    [HarmonyPatch(typeof(SaveSlotController))]
    public class SaveSlotSelectPatch {
        [HarmonyPatch("OnClick", new Type[] { })]
        private static bool Prefix(SaveSlotController __instance) {
            if (ArchipelagoConnection.session != null) {
                ArchipelagoConnection.session.Socket.DisconnectAsync();
            }
            int slotNumber = __instance.GetDisplaySlotNumber() - 1;

            bool connectResult = false;
            var isEmptyField = typeof(SaveSlotController).GetField("isEmpty", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            if (!(bool)isEmptyField.GetValue(__instance)) {
                if (Plugin.saves.ContainsKey(slotNumber)) {
                    Plugin.isRandomizer = true;
                    Plugin.itemManager = new ItemManager(slotNumber);
                    string[] connectionInfo = Plugin.saves[slotNumber].Split(';');
                    ArchipelagoConnection archipelagoConnection = new ArchipelagoConnection(connectionInfo[0], connectionInfo[1]);
                    connectResult = archipelagoConnection.Connect();
                    UnlockEndings(slotNumber);
                } else {
                    Plugin.isRandomizer = false;
                }
                return connectResult;
            } else {
                string host = Plugin.currentHost;
                string slot = Plugin.currentSlot;
                Plugin.isRandomizer = true;
                Plugin.itemManager = new ItemManager(slotNumber);
                ArchipelagoConnection archipelagoConnection = new ArchipelagoConnection(host, slot);
                connectResult = archipelagoConnection.Connect();
                UnlockEndings(slotNumber);
                Plugin.saves[slotNumber] = host + ";" + slot;
                Plugin.WriteConnectionInfos();
                return connectResult;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("Update", new Type[] { })]
        private static bool PrefixUpdate(SaveSlotController __instance) {
            Plugin.inMenu = true;
            //return true;
            if (Input.GetKeyDown(KeyCode.F4)) {
                var buttonVar = typeof(SaveSlotController).GetField("button", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                if (EventSystem.current.currentSelectedGameObject != ((Button)buttonVar.GetValue(__instance)).gameObject) {
                    return true;
                }
                var deleteSlot = typeof(SaveSlotController).GetMethod("DeleteSlot", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                deleteSlot.Invoke(__instance, new object[] { __instance.GetDisplaySlotNumber() - 1 });
            }
            return true;
        }
        private static void UnlockEndings(int saveSlot)
        {
            ISaveSlotService saveService = Core.Get<ISaveSlotService>();
            IEnumerable<EndingTypes> unlockedEndings = ArchipelagoConnection.session.Locations.AllLocationsChecked.Select(location => (EndingTypes)(location - Plugin.reventureEndingOffset));
            saveService.Save<List<global::EndingTypes>>(saveSlot, "unlockedEndingsList", unlockedEndings.ToList());
        }
    }

    [HarmonyPatch(typeof(TitleDirector))]
    public class TitleDirectorPatch
    {
        [HarmonyPatch("Start", new Type[] { })]
        private static void Postfix() {
            //ReventureGUI.SetupLoginGUINative();
            Plugin.inMenu = true;
        }
    }

    [HarmonyPatch(typeof(EndingCellController))]
    public class EndingCellControllerPatch {
        [HarmonyPatch("Setup", new Type[] { typeof(EndingTypes), typeof(GalleryController), typeof(CellStatus) })]
        private static void Postfix(EndingCellController __instance) {
            if (!Plugin.isRandomizer) {
                return;
            }

            if (Core.Get<IProgressionService>().IsEndingUnlocked(__instance.Ending)) {
                return;
            }

            bool available = Plugin.logic.rulesDict[__instance.Ending].Invoke();

            GameObject background = __instance.transform.GetChild(1).gameObject;
            background.SetActive(available);
            if (available) {
                GameObject hinticon = __instance.transform.GetChild(1).GetChild(2).gameObject;
                hinticon.SetActive(true);
                GameObject frame = __instance.transform.GetChild(1).GetChild(4).gameObject;
                frame.SetActive(false);

                var field = typeof(EndingCellController).GetField("cellStatus", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                field.SetValue(__instance, CellStatus.Unlocked);
            }
            return;
        }
    }

    // This fixes the NPEs that happen do to the weird UI stuff
    [HarmonyPatch(typeof(OptionTabs))]
    public class OptionTabsPatch {
        [HarmonyPatch("RefreshNavigation", new Type[] { })]
        private static bool Prefix(OptionTabs __instance) {
            return __instance != null;
        }
    }
}
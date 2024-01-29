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
using Random = UnityEngine.Random;

namespace ReventureEndingRando
{

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {

        public static long reventureItemOffset = 900270000;
        public static long reventureEndingOffset = 900271000;

        Harmony harmony;
        public static ManualLogSource PatchLogger;

        public static EndingRandomizer randomizer;

        public static List<EndingEffectsEnum> endingEffects;

        public static Queue<EndingEffectsEnum> lastUnlocks;

        private void Awake()
        {
            // Plugin startup logic
            PatchLogger = Logger;

            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            randomizer = new EndingRandomizer();
            //Logger.LogInfo($"Randomized: {randomizer.ToString()}");

            //Read settings file
            string contents = File.ReadAllText("connectioninfo.txt");
            string[] contentSplit = contents.Split(';');
            string host = contentSplit[0];
            int port = int.Parse(contentSplit[1]);
            string slot = contentSplit[2];

            ArchipelagoConnection archipelagoConnection = new ArchipelagoConnection(host, port, slot);
            archipelagoConnection.Connect();

            lastUnlocks = new Queue<EndingEffectsEnum>();

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} Version {MyPluginInfo.PLUGIN_VERSION} is loaded!");
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.F5))
            //{
            //    Logger.LogInfo($"Available Effects:");
            //    foreach (EndingEffectsEnum ee in endingEffects)
            //    {
            //        Logger.LogInfo($"{ee}");
            //    }
            //}

            //if (Input.GetKeyDown(KeyCode.F7))
            //{
            //    randomizer.Randomize();
            //    randomizer.StoreState();
            //}
        }

        private void OnDestroy()
        {
            ArchipelagoConnection.session.Socket.DisconnectAsync();
            harmony.UnpatchSelf();
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} unloaded!");
        }
    }

    // Allow skipping endings
    [HarmonyPatch(typeof(EndingProvider))]
    public class EndingProviderPatch
    {
        [HarmonyPatch("FinalizeRun", new Type[] { typeof(float), typeof(EndingCinematicConfiguration), typeof(bool) })]
        private static bool Prefix(ref EndingCinematicConfiguration configuration)
        {
            configuration.skippable = true;
            //EndingEffectsEnum ee = Plugin.randomizer.randomization[configuration.ending];
            //Plugin.lastUnlocks.Enqueue(ee);
            //if (Plugin.lastUnlocks.Count > 3)
            //{
            //    Plugin.lastUnlocks.Dequeue();
            //}
            //Plugin.PatchLogger.LogInfo($"{configuration.ending} unlocked {ee}!");

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
            //First Run
            //IProgressionService progression = Core.Get<IProgressionService>();
            //Plugin.endingEffects = Plugin.randomizer.UpdateWorld(progression);
            Plugin.endingEffects = Plugin.randomizer.UpdateWorldArchipelago();
            return;
        }
    }

    [HarmonyPatch(typeof(UltimateDoor))]
    public class UltimateDoorPatch
    {
        [HarmonyPatch("Start", new Type[] { })]
        private static void Postfix(UltimateDoor __instance)
        {
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
            string lastUnlocksText = "Last Unlocks: ";
            foreach (EndingEffectsEnum ee in Plugin.lastUnlocks.AsEnumerable())
            {
                lastUnlocksText += $"{ee}, ";
            }

            var field = typeof(CameraZoneText).GetField("cameraZone", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            TextMeshProUGUI cameraZone = (TextMeshProUGUI) field.GetValue(__instance);
            cameraZone.text = lastUnlocksText;
            return false;
        }
    }

    [HarmonyPatch(typeof(SuperLonkTransformationTrigger))]
    public class SuperLonkTransformationTriggerPatch
    {
        [HarmonyPatch("RunMetamorphosis", new Type[] { })]
        private static void Postfix()
        {
            GameObject goodCrops = GameObject.Find("World/PersistentElements/GoodCrops");
            goodCrops.SetActive(true);
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
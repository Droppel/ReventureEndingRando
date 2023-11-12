using Atto;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ReventureEndingRando.EndingEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ReventureEndingRando
{

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        Harmony harmony;
        public static ManualLogSource PatchLogger;

        public static EndingRandomizer randomizer;

        public static List<EndingEffectsEnum> endingEffects;

        private void Awake()
        {
            // Plugin startup logic
            PatchLogger = Logger;

            harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            randomizer = new EndingRandomizer();
            Logger.LogInfo($"Randomized: {randomizer.ToString()}");

            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Logger.LogInfo($"Available Effects:");
                foreach (EndingEffectsEnum ee in endingEffects)
                {
                    Logger.LogInfo($"{ee}");
                }
            }
        }

        private void OnDestroy()
        {
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
            Plugin.PatchLogger.LogInfo($"{configuration.ending} unlocked {Plugin.randomizer.randomization[configuration.ending]}!");
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
            IProgressionService progression = Core.Get<IProgressionService>();
            Plugin.endingEffects = Plugin.randomizer.UpdateWorld(progression);
            return;
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
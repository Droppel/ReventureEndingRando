using Atto;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
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
                randomizer.Randomize();
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
            bool success = Plugin.randomizer.randomization.TryGetValue(configuration.ending, out EndingEffects.EndingEffectsEnum ee);
            if (success)
            {
                Plugin.PatchLogger.LogInfo($"{configuration.ending} unlocked nothing!");
            } else
            {
                Plugin.PatchLogger.LogInfo($"{configuration.ending} unlocked {ee}!");
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
            IProgressionService progression = Core.Get<IProgressionService>();
            Plugin.randomizer.UpdateWorld(progression);
            return;
        }
    }
}
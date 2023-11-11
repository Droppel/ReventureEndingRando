using ReventureEndingRando.EndingEffects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

namespace ReventureEndingRando
{
    class Logic
    {

        Dictionary<EndingTypes, Requirement> endingLogic;

        public void ParseLogic()
        {
            endingLogic = new Dictionary<EndingTypes, Requirement>();
            string logicFile = File.ReadAllText(@"logic.txt");
            Plugin.PatchLogger.LogInfo($"Read File");

            Dictionary<string, string> makros = new Dictionary<string, string>();

            foreach (string line in logicFile.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
            {
                if (line.Length == 0)
                    continue;
                Plugin.PatchLogger.LogInfo($"Line: {line}");

                if (line[0] == '#')
                {
                    string[] makroLineSplit = line.Substring(1).Split('=');
                    makros.Add(makroLineSplit[0], makroLineSplit[1]);
                    continue;
                }

                string[] lineSplit = line.Split(new string[] { ": " }, StringSplitOptions.None);

                Enum.TryParse(lineSplit[0], out EndingTypes et);

                string reqs = lineSplit[1];
                Plugin.PatchLogger.LogInfo($"reqs is: {reqs}");
                foreach (KeyValuePair<string, string> makro in makros)
                {
                    string dollarMakro = "$" + makro.Key;
                    string makroValue = makro.Value;
                    reqs = reqs.Replace(dollarMakro, makroValue);
                }
                Plugin.PatchLogger.LogInfo($"Replaced makros: {reqs}");

                endingLogic.Add(et, new Requirement(reqs));
            }
        }

        public bool isReachable(EndingTypes et, List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount)
        {
            return endingLogic[et].IsFulfilled(effects, unlockedEndings, weightedItemCount);
        }
    }

    class Seed {

        List<EndingTypes> filledEndings;
        List<EndingTypes> availableEndings;

        List<EndingEffectsEnum> availableEffects;
        List<EndingEffectsEnum> usedEndingEffects;

        List<EndingEffectsEnum> weightedItems;
        int weightedItemsCount = 0;

        public Dictionary<EndingTypes, EndingEffectsEnum> randomization;

        public Seed()
        {
            //TestLogic();
            //return;

            bool success = Init();
            while (!success)
            {
                Plugin.PatchLogger.LogInfo(success);
                Plugin.PatchLogger.LogInfo($"Missing effects: {availableEffects.Count}");
                Plugin.PatchLogger.LogInfo($"Missing endings: {availableEndings.Count}:");
                Plugin.PatchLogger.LogInfo($"Missing effects:");
                foreach (EndingEffectsEnum ee in availableEffects)
                {
                    if (ee != EndingEffectsEnum.Nothing)
                    {
                        Plugin.PatchLogger.LogInfo($"{ee}");
                    }
                }
                Plugin.PatchLogger.LogInfo($"=========Missing endings:===========");
                foreach (EndingTypes et in availableEndings)
                {
                    Plugin.PatchLogger.LogInfo($"{et}");
                }
                success = Init();
            }
        }

        public void TestLogic()
        {
            List<EndingTypes> testEndings = new List<EndingTypes>();
            testEndings.Add(EndingTypes.StabPrincess);

            List<EndingEffectsEnum> availableEffects = new List<EndingEffectsEnum>();
            availableEffects.Add(EndingEffectsEnum.SpawnSwordChest);
            availableEffects.Add(EndingEffectsEnum.SpawnPrincessItem);
            availableEffects.Add(EndingEffectsEnum.SpawnHookChest);

            int unlockedEndings = 80;
            int weightedItemCount = 4;

            Logic logic = new Logic();
            logic.ParseLogic();

            foreach (EndingTypes et in testEndings)
            {
                bool reachable = logic.isReachable(et, availableEffects, unlockedEndings, weightedItemCount);
                Plugin.PatchLogger.LogInfo($"Ending {et} is reachable: {reachable}");
            }
        }

        private bool Init()
        {
            //Random.InitState(42);

            Plugin.PatchLogger.LogInfo("====================================================================");
            Plugin.PatchLogger.LogInfo("Initializing Logic");
            Logic logic = new Logic();
            logic.ParseLogic();
            randomization = new Dictionary<EndingTypes, EndingEffectsEnum>();
            Plugin.PatchLogger.LogInfo("Logic initialized");

            filledEndings = new List<EndingTypes>();
            usedEndingEffects = new List<EndingEffectsEnum>();

            weightedItems = new List<EndingEffectsEnum>();
            //These two are covered separately
            //weightedItems.Add(EndingEffectsEnum.SpawnSwordChest);
            //weightedItems.Add(EndingEffectsEnum.SpawnSwordPedestalItem);
            weightedItems.Add(EndingEffectsEnum.SpawnShovelChest);
            weightedItems.Add(EndingEffectsEnum.SpawnShieldChest);
            weightedItems.Add(EndingEffectsEnum.SpawnBombsChest);
            weightedItems.Add(EndingEffectsEnum.SpawnHookChest);
            weightedItems.Add(EndingEffectsEnum.SpawnLavaTrinketChest);
            weightedItems.Add(EndingEffectsEnum.SpawnWhistleChest);

            availableEndings = Enum.GetValues(typeof(EndingTypes)).Cast<EndingTypes>().ToList();
            // Remove None, ThankyouForPlaying (150%) and Ultimate Ending (Aka finishing the game)
            availableEndings.Remove(EndingTypes.None);
            availableEndings.Remove(EndingTypes.UltimateEnding);
            availableEndings.Remove(EndingTypes.ThankYouForPlaying);

            availableEffects = Enum.GetValues(typeof(EndingEffectsEnum)).Cast<EndingEffectsEnum>().ToList();
            // -1 because there is already a nothing effect in the list
            int nothingEffectCount = 99 - availableEffects.Count;
            Plugin.PatchLogger.LogInfo("Setup Lists");

            //This is horrible, don't do this! (It effectively runs the loop i times)
            int i = nothingEffectCount;
            while (i --> 0)
            {
                availableEffects.Add(EndingEffectsEnum.Nothing);
            }
            Plugin.PatchLogger.LogInfo("Added Nothing Effects");

            bool successfull = true;
            int iteration = 0;
            while (availableEndings.Count != 0)
            {
                Plugin.PatchLogger.LogInfo($"Starting iteration {iteration}");
                // Get all reachable Endings
                List<EndingTypes> reachableEndings = new List<EndingTypes>();
                foreach (EndingTypes et in availableEndings)
                {
                    if (logic.isReachable(et, usedEndingEffects, filledEndings.Count, weightedItemsCount))
                    {
                        reachableEndings.Add(et);
                    }
                }

                if (reachableEndings.Count == 0)
                {
                    successfull = false;
                    break;
                }

                foreach (EndingTypes et in reachableEndings)
                {
                    availableEndings.Remove(et);
                    filledEndings.Add(et);

                    int randInd = Random.RandomRangeInt(0, availableEffects.Count);
                    EndingEffectsEnum ee = availableEffects[randInd];
                    Plugin.PatchLogger.LogInfo($"Ending {et} got {ee}");
                    availableEffects.RemoveAt(randInd);
                    usedEndingEffects.Add(ee);
                    if (weightedItems.Contains(ee))
                    {
                        weightedItemsCount++;
                    }

                    randomization.Add(et, ee);
                }
                if (usedEndingEffects.Contains(EndingEffectsEnum.SpawnSwordChest) || usedEndingEffects.Contains(EndingEffectsEnum.SpawnSwordPedestalItem))
                {
                    weightedItemsCount++;
                }
                iteration++;
            }
            return successfull;
        }
    }

    class Requirement
    {
        RequirementNode rootNode;

        public Requirement(string _req)
        {
            rootNode = new RequirementNode(_req);
        }

        public bool IsFulfilled(List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount)
        {
            //Plugin.PatchLogger.LogInfo($"Resolving Requirements, effects: {effects}, unlockedEndingCount: {unlockedEndings}");
            return rootNode.Resolve(effects, unlockedEndings, weightedItemCount);
        }
    }

    class RequirementNode {
        RequirementNode A;
        RequirementNode B;
        Func<List<EndingEffectsEnum>, int, int, bool> resolve;

        public RequirementNode(string _req)
        {
            Plugin.PatchLogger.LogInfo($"Calculating Req for: {_req}");
            //Remove trailing spaces
            _req = _req.Trim();
            //Remove outer brackets
            int bracketCount = 0;
            int openBracketIndex = 0;
            if (_req[_req.Length - 1] == ')')
            {
                bracketCount++;
                for (int i = _req.Length - 2; i >= 0; i--)
                {
                    if (bracketCount == 0)
                    {
                        openBracketIndex = i;
                        break;
                    }

                    if (_req[i] == '(')
                    {
                        bracketCount--;
                    } else if (_req[i] == ')')
                    {
                        bracketCount++;
                    }
                }
                if (openBracketIndex == 0)
                {
                    _req = _req.Substring(1, _req.Length - 2);
                }
            }
            Plugin.PatchLogger.LogInfo($"Req after trimming: {_req}");

            if (_req == "free")
            {
                Plugin.PatchLogger.LogInfo($"Ending is free");
                this.resolve = (List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount) => { return true; };
                return;
            }

            char splitter = ' ';
            int splitterIndex = 0;

            bracketCount = 0;

            for (int i = _req.Length - 1; i >= 0; i--)
            {
                if (bracketCount == 0)
                {
                    if (_req[i] == '+' || _req[i] == '|')
                    {
                        splitter = _req[i];
                        splitterIndex = i;
                        break;
                    } else if (_req[i] == ')')
                    {
                        bracketCount++;
                    }
                } else
                {
                    if (_req[i] == ')')
                    {
                        bracketCount++;
                    } else if (_req[i] == '(')
                    {
                        bracketCount--;
                    }
                }
            }

            Plugin.PatchLogger.LogInfo($"Splitter is {splitter}");
            Plugin.PatchLogger.LogInfo($"Splitterindex is {splitterIndex}");

            if (splitter == ' ')
            {
                Plugin.PatchLogger.LogInfo($"LeafNode");
                bool isEndingCount = int.TryParse(_req, out int endingCount);
                if (isEndingCount)
                {
                    Plugin.PatchLogger.LogInfo($"Endingcount for {endingCount}");
                    this.resolve = (List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount) => { return unlockedEndings >= endingCount; };
                    return;
                }

                //Weighted Items
                if (_req[0] == '%')
                {
                    int itemCount = int.Parse(_req.Substring(1));
                    this.resolve = (List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount) => { return weightedItemCount >= itemCount; };
                    return;
                }

                Plugin.PatchLogger.LogInfo($"Needs effect");
                this.resolve = (List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount) => {
                    bool success = Enum.TryParse(_req, out EndingEffectsEnum ee);
                    return success && effects.Contains(ee);
                };
                return;
            } else
            {
                string[] splitReq = _req.Split(splitter);
                string aString = _req.Substring(0, splitterIndex);
                string bString = _req.Substring(splitterIndex+1);
                Plugin.PatchLogger.LogInfo($"A side is {aString}");
                Plugin.PatchLogger.LogInfo($"B side is {bString}");
                A = new RequirementNode(aString);
                B = new RequirementNode(bString);
                if (aString == _req || bString == _req)
                {
                    int zero = 0;
                    int crash = 1 / zero;
                }
                if (splitter == '+')
                {
                    this.resolve = (List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount) => { return A.Resolve(effects, unlockedEndings, weightedItemCount) && B.Resolve(effects, unlockedEndings, weightedItemCount); };
                    return;
                } else if (splitter == '|')
                {
                    this.resolve = (List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount) => { return A.Resolve(effects, unlockedEndings, weightedItemCount) || B.Resolve(effects, unlockedEndings, weightedItemCount); };
                    return;
                }
            }
        }

        public bool Resolve(List<EndingEffectsEnum> effects, int unlockedEndings, int weightedItemCount)
        {
            return this.resolve(effects, unlockedEndings, weightedItemCount);
        }

    }
}

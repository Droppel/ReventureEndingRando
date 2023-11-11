using Newtonsoft.Json;
using ReventureEndingRando.EndingEffects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public void UpdateWorld(IProgressionService progression)
        {
            foreach (KeyValuePair<EndingTypes, EndingEffectsEnum> entry in randomization)
            {
                EndingEffect ee = EndingEffect.InitFromEnum(entry.Value);
                bool endingAchieved = progression.IsEndingUnlocked(entry.Key);
                if (ee != null) {
                    ee.ActivateEffect(endingAchieved);
                }
            }
        }

        public bool LoadState()
        {
            try
            {
                randomization = JsonConvert.DeserializeObject<Dictionary<EndingTypes, EndingEffectsEnum>>(File.ReadAllText("randomizer.txt"));
                return true;
            } catch (Exception e)
            {
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

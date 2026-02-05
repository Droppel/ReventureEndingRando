using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ReventureEndingRando
{
    public enum NonStopLevels
    {
        None,
        Canonical,
        Logical,
        Everything,
        NotImplemented,
    }

    public class NonStopEnding
    {
        public NonStopLevels level;
        public virtual void CleanUp()
        {
            Hero.instance.gameObject.SetActive(true);
            var inputStateField = typeof(InputManipulator).GetField("state", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            inputStateField.SetValue(Hero.instance.inputManipulator, HeroInputState.Enabled);
            Hero.instance.character.InputEnabled = true;
		    GameplayDirector.instance.PauseDisabled = false;
            return;
        }
    }

    public class NonStopEndingTreasureRoom : NonStopEnding
    {
        public override void CleanUp()
        {
            base.CleanUp();
            GameObject.Find("World/PersistentElements/TreasureLonk").SetActive(true);
            GameObject.Find("World/PersistentElements/OpenedTreasureChest").SetActive(true);
            GameObject.Find("World/PersistentElements/ClosedTreasureChest").SetActive(false);
        }
    }

    public class NonStopEndings
    {
        public static Dictionary<EndingTypes, NonStopEnding> nonstopEndings = new Dictionary<EndingTypes, NonStopEnding>
        {
            { EndingTypes.PickupAnvil, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO Softlocks you because it takes to long
            { EndingTypes.CrushedByOwnStuff, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.DestroyAllPots, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.StabDragon, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.FindTreasure, new NonStopEndingTreasureRoom { level = NonStopLevels.Canonical } },
            { EndingTypes.FindFishingRod, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.FallIntoLava, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows walking through lava
            { EndingTypes.JumpOffTheCliff, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.HugBoulder, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows skipping past the boulder so it could break logic
            { EndingTypes.DigIntoBottomlessPit, new NonStopEnding { level = NonStopLevels.Everything } }, // You're stuck anyways
            { EndingTypes.FaultyCannonShot, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO Cannon endings are weird
            { EndingTypes.CaughtByOwnBomb, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.RoastedByDragon, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.HugDragon, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.TakePrincessBackToTown, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows going through the castle with the princess
            { EndingTypes.KidnapPrincess, new NonStopEnding { level = NonStopLevels.Everything } }, // You're stuck anyways
            { EndingTypes.TakeTheDayOff, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.StabElder, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.HugDarkLord, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.JumpIntoPiranhaLake, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.ElevatorCrush, new NonStopEnding { level = NonStopLevels.Everything } }, // Clips you through the ground into the arena room
            { EndingTypes.FallIntoSpikes, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows walking through the spikes
            { EndingTypes.LeapOfFaithFromTheMountain, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.AirDuctsAccident, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.SacrificePrincess, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows using the princess as an item for ending 71
            { EndingTypes.StabBoulder, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows removing the boulder
            { EndingTypes.WaterfallsBottom, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows going to the bottom of waterfalls
            { EndingTypes.WrongLever, new NonStopEnding { level = NonStopLevels.Everything } }, // You're stuck anyways
            { EndingTypes.ClimbMountain, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.DarkStoneToAltar, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.JumpOffTheBalcony, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.ShootCannonballToCastle, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO Cannon endings are weird
            { EndingTypes.ShootCannonballToShop, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO Cannon endings are weird
            { EndingTypes.ShootCannonballToTown, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO Cannon endings are weird
            { EndingTypes.FallWithPrincessToAnvilPit, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.StabGuard, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.HugPrincess, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.HugElder, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.HugShopkeeper, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.HugGuard, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.RescueCat, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.StabPrincess, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.JumpOffTheBalconyWithPrincess, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows going to the right side of the fortress with just the princess
            { EndingTypes.KilledByDarkLord, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.StabDarkLord, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.GiveDarkStoneToDarkLord, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.DragonWithFireTrinket, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.MakeBabiesWithPrincess, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.DatePrincessAndDragon, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.StabShopKeeper, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.KillTheKing, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.HugTheKing, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.KilledByMinion, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO broken in all sorts of ways
            { EndingTypes.HugMinion, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.FaceDarkLordWithShield, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.FallIntoWaterfallWithPrincess, new NonStopEnding { level = NonStopLevels.Everything } }, // Allows going to the bottom of waterfalls
            { EndingTypes.DragonWithShield, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.DragonWithShieldAndFireTrinket, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.EatenByFakePrincess, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.ShotgunFakePrincess, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.KillAllDevsHell, new NonStopEnding { level = NonStopLevels.Everything } }, // You're stuck anyways
            { EndingTypes.TakePrincessToBed, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.PlaceBombUnderCastle, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.StabMinionMultipleTimes, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.HarakiriSuicide, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.SelfDestructFortress, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.ShootPrincessToTown, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO Cannon endings are weird
            { EndingTypes.HundredMinionsMassacre, new NonStopEnding { level = NonStopLevels.Everything } },
            { EndingTypes.WrongLever2, new NonStopEnding { level = NonStopLevels.Everything } }, // Gives access to burger/darkstone
            { EndingTypes.KilledByDarkArenaMinion, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO broken in all sorts of ways
            { EndingTypes.GetIntoThePipe, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.DarkLordComicStash, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.FakePrincessInsideChest, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.DesertEnd, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.LonkFaceplant, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.MultipleDesertJumps, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.GetIntoBigChest, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.TakePrincessToLonksHouse, new NonStopEnding { level = NonStopLevels.Canonical } }, // This does allow going through lonks house with the princess but that is equivalent to going over the house
            { EndingTypes.EnterTheChimney, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.DontKillMinions, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.TakeChickenToDarkAltar, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.BreakSpaceTimeContinuum, new NonStopEnding { level = NonStopLevels.NotImplemented } }, // TODO Whistle strikes once again
            { EndingTypes.SwimIntoTheOcean, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.FeedTheMimic, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.StayInTheWater, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.FindAlienLarvae, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.AboardPirateShip, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.CrushedAtUltimateDoor, new NonStopEnding { level = NonStopLevels.Everything } }, // Traps you beneath the floor
            { EndingTypes.KillAllFairies, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.OverhealByFairies, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.KillChicken, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.HugChicken, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.TakePrincessToDarkAltar, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.GetIntoTheCloud, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.TriggerTrollSpikes, new NonStopEnding { level = NonStopLevels.Logical } },
            { EndingTypes.StayAfk, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.PrincessToDesertGate, new NonStopEnding { level = NonStopLevels.Everything } }, // Traps you beneath the floor
            { EndingTypes.FeedTheKing, new NonStopEnding { level = NonStopLevels.Canonical } },
            { EndingTypes.SacrificeEveryItem, new NonStopEnding { level = NonStopLevels.Canonical } },
        };
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using EE = ReventureEndingRando.EndingEffects.EndingEffectsEnum;

namespace ReventureEndingRando {
    public class Logic {
        private static bool Has(EE effect, int reqAmount = 1) {
            int effectAmount;
            if (Plugin.endingEffects.TryGetValue(effect, out effectAmount)) {
                effectAmount = 0;
            }
            return effectAmount >= reqAmount;
        }

        private static bool HasAll(params EE[] effects) {
            foreach (var e in effects) {
                if (!Has(e)) {
                    return false;
                }
            }
            return true;
        }
        private static bool HasAny(params EE[] effects) {
            foreach (var e in effects) {
                if (Has(e)) {
                    return true;
                }
            }
            return false;
        }

        private static bool HasBurger() {
            return Has(EE.SpawnBurgerChest) && Has(EE.DarkstoneLeverMiddle);
        }

        private static bool HasDarkStone() {
            return HasAll(EE.SpawnDarkstoneChest, EE.DarkstoneLeverMiddle);
        }

        private static bool HasChicken() {
            return Has(EE.GrowChicken, 4);
        }

        private static bool HasNuke() {
            return HasAll(EE.SpawnNukeItem, EE.SpawnHookChest);
        }

        private static bool HasSword() {
            if (ArchipelagoConnection.treasureRoomSword == 1) {
                if (Has(EE.ProgressiveSword, 2)) {
                    return true;
                } else if (Has(EE.ProgressiveSword, 1)) {
                    return Has(EE.SpawnShovelChest) && HasAny(EE.SpawnHookChest, EE.UnlockGeyserWaterfall);
                }
                return false;
            } else {
                return Has(EE.ProgressiveSword);
            }
        }

        private static bool HasWeight(int req) {
            int weightedItems = 0;
            EE[] validItems = { EE.SpawnShovelChest, EE.SpawnShieldChest, EE.SpawnBombsChest, EE.SpawnHookChest, EE.SpawnLavaTrinketChest, EE.SpawnWhistleChest, EE.SpawnBoomerang };
            foreach (EE effect in validItems) {
                if (Has(effect)) {
                    weightedItems += 1;
                }
            }
            if (HasSword()) {
                weightedItems += 1;
            }
            if (HasNuke()) {
                weightedItems += 1;
            }
            if (HasDarkStone()) {
                weightedItems += 1;
            }
            if (HasChicken()) {
                weightedItems += 1;
            }
            return weightedItems >= req;
        }

        private static bool HasItems(int req) {
            int items = 0;
            EE[] validItems = { EE.SpawnShovelChest, EE.SpawnShieldChest, EE.SpawnBombsChest, EE.SpawnHookChest, EE.SpawnLavaTrinketChest, EE.SpawnWhistleChest, EE.SpawnBoomerang,
                                    EE.SpawnMrHugsChest, EE.SpawnMapChest, EE.SpawnCompassChest};
            foreach (EE effect in validItems) {
                if (Has(effect)) {
                    items += 1;
                }
            }
            if (HasSword()) {
                items += 1;
            }
            if (HasNuke()) {
                items += 1;
            }
            if (HasDarkStone()) {
                items += 1;
            }
            if (HasChicken()) {
                items += 1;
            }
            if (HasBurger()) {
                items += 1;
            }
            return items >= req;
        }

        private static bool CanReachPrincessportalWithItem() {
            return Has(EE.UnlockMirrorPortal) && (Has(EE.GrowVine)
                || (HasChicken()
                && (HasSword()
                || HasAny(EE.SpawnHookChest, EE.UnlockGeyserWaterfall))));
        }

        private static bool CanPassCastleWithItem() {
            return HasSword() || (HasChicken() && Has(EE.BuildStatue)) || (HasAny(EE.SpawnHookChest, EE.SpawnShovelChest, EE.UnlockShopCannon, EE.UnlockGeyserVolcanoe, EE.OpenCastleFloor, EE.UnlockFairyPortal));
        }

        private static bool CanReachPrincessWithItem() {
            return Has(EE.SpawnPrincessItem) && (CanReachPrincessportalWithItem() || HasAny(EE.SpawnHookChest, EE.UnlockElevatorButton, EE.UnlockCallElevatorButtons));
        }

        public Dictionary<EndingTypes, Func<bool>> rulesDict;

        public Logic() {

            rulesDict = new Dictionary<EndingTypes, Func<bool>>() {
                [EndingTypes.StabElder] = () => HasSword() && Has(EE.SpawnElder),
                [EndingTypes.LonkFaceplant] = () => Has(EE.UnlockFacePlantStone),
                [EndingTypes.KillTheKing] = () => true,
                [EndingTypes.StabGuard] = () => HasSword(),
                [EndingTypes.KillTheKing] = () => HasSword() && Has(EE.SpawnKing),
                [EndingTypes.FallIntoLava] = () => true,
                [EndingTypes.JumpIntoPiranhaLake] = () => true,
                [EndingTypes.WaterfallsBottom] = () => true,
                [EndingTypes.StabShopKeeper] = () => HasSword() && Has(EE.SpawnShopkeeper),
                [EndingTypes.DigIntoBottomlessPit] = () => Has(EE.SpawnShovelChest),
                [EndingTypes.RescueCat] = () => Has(EE.SpawnMrHugsChest),
                [EndingTypes.FindFishingRod] = () => Has(EE.SpawnFishingRodChest),
                [EndingTypes.HugMinion] = () => Has(EE.SpawnMrHugsChest),
                [EndingTypes.RoastedByDragon] = () => Has(EE.SpawnDragon),
                [EndingTypes.KilledByDarkArenaMinion] = () => true,
                [EndingTypes.StabDragon] = () => HasSword() && Has(EE.SpawnDragon),
                [EndingTypes.FaultyCannonShot] = () => HasAny(EE.UnlockShopCannon, EE.UnlockCastleToShopCannon, EE.UnlockDarkCastleCannon, EE.UnlockCastleToDarkCastleCannon),
                [EndingTypes.HugTheKing] = () => HasAll(EE.SpawnMrHugsChest, EE.SpawnKing),
                [EndingTypes.HugGuard] = () => Has(EE.SpawnMrHugsChest),
                [EndingTypes.TakeTheDayOff] = () => true,
                [EndingTypes.FallIntoSpikes] = () => true,
                [EndingTypes.PickupAnvil] = () => Has(EE.SpawnAnvilItem),
                [EndingTypes.EatenByFakePrincess] = () => Has(EE.SpawnMimic),
                [EndingTypes.ClimbMountain] = () => Has(EE.SpawnStrawberry),
                [EndingTypes.StabMinionMultipleTimes] = () => HasSword(),
                [EndingTypes.CrushedByOwnStuff] = () => HasWeight(5) && Has(EE.SpawnHookChest),
                [EndingTypes.ShootCannonballToCastle] = () => HasNuke() && Has(EE.UnlockShopCannon),
                [EndingTypes.CaughtByOwnBomb] = () => Has(EE.SpawnBombsChest),
                [EndingTypes.DragonWithShield] = () => HasAll(EE.SpawnDragon, EE.SpawnShieldChest),
                [EndingTypes.EnterTheChimney] = () => true,
                [EndingTypes.DestroyAllPots] = () => HasSword(),
                [EndingTypes.StabBoulder] = () => HasSword() && Has(EE.SpawnBoulderNPC),
                [EndingTypes.LeapOfFaithFromTheMountain] = () => true,
                [EndingTypes.ElevatorCrush] = () => HasAny(EE.UnlockElevatorButton, EE.UnlockCallElevatorButtons, EE.SpawnPrincessItem),
                [EndingTypes.GetIntoThePipe] = () => Has(EE.OpenSewerPipe),
                [EndingTypes.DragonWithFireTrinket] = () => HasAll(EE.SpawnDragon, EE.SpawnLavaTrinketChest),
                [EndingTypes.HugShopkeeper] = () => HasAll(EE.SpawnShopkeeper, EE.SpawnMrHugsChest),
                [EndingTypes.WrongLever] = () => Has(EE.DarkstoneLeverLeft),
                [EndingTypes.GetIntoBigChest] = () => true,
                [EndingTypes.HugElder] = () => HasAll(EE.SpawnMrHugsChest, EE.SpawnElder),
                [EndingTypes.DragonWithShieldAndFireTrinket] = () => HasAll(EE.SpawnDragon, EE.SpawnLavaTrinketChest, EE.SpawnShieldChest),
                [EndingTypes.AirDuctsAccident] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.HugDragon] = () => HasAll(EE.SpawnDragon, EE.SpawnMrHugsChest),
                [EndingTypes.WrongLever2] = () => Has(EE.DarkstoneLeverRight),
                [EndingTypes.TakePrincessToBed] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.JumpOffTheCliff] = () => true,
                [EndingTypes.HarakiriSuicide] = () => HasSword(),
                [EndingTypes.SelfDestructFortress] = () => ArchipelagoConnection.hardJumps == 1 || Has(EE.SpawnHookChest) || (CanPassCastleWithItem() && HasChicken()),
                [EndingTypes.HundredMinionsMassacre] = () => ArchipelagoConnection.hardCombat == 1 ? HasSword() : HasSword() && Has(EE.SpawnShieldChest),
                [EndingTypes.KilledByDarkLord] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.TakePrincessBackToTown] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.ShootPrincessToTown] = () => HasAll(EE.SpawnPrincessItem, EE.UnlockDarkCastleCannon),
                [EndingTypes.FallWithPrincessToAnvilPit] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.HugBoulder] = () => HasAll(EE.SpawnMrHugsChest, EE.SpawnBoulderNPC),
                [EndingTypes.JumpOffTheBalconyWithPrincess] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.ShootCannonballToShop] = () => HasNuke() && Has(EE.UnlockCastleToShopCannon) && CanPassCastleWithItem(),
                [EndingTypes.HugPrincess] = () => HasAll(EE.SpawnMrHugsChest, EE.SpawnPrincessItem),
                [EndingTypes.JumpOffTheBalcony] = () => true,
                [EndingTypes.StayAfk] = () => true,
                [EndingTypes.PlaceBombUnderCastle] = () => Has(EE.SpawnBombsChest),
                [EndingTypes.DontKillMinions] = () => ArchipelagoConnection.hardJumps == 1 || Has(EE.SpawnHookChest) || (CanPassCastleWithItem() && HasChicken()),
                [EndingTypes.FindTreasure] = () => Has(EE.SpawnShovelChest) && (HasSword() || HasAny(EE.SpawnHookChest, EE.UnlockGeyserWaterfall)),
                [EndingTypes.KillChicken] = () => HasSword() && HasChicken(),
                [EndingTypes.StabPrincess] = () => HasSword() && CanReachPrincessWithItem(),
                [EndingTypes.OverhealByFairies] = () => true,
                [EndingTypes.DarkStoneToAltar] = () => HasDarkStone(),
                [EndingTypes.CrushedAtUltimateDoor] = () => true,
                [EndingTypes.DarkLordComicStash] = () => ArchipelagoConnection.hardJumps == 1 || Has(EE.SpawnPrincessItem),
                [EndingTypes.StabDarkLord] = () => HasSword() && CanReachPrincessWithItem(),
                [EndingTypes.SacrificeEveryItem] = () => HasItems(6) &&
                    (HasSword() ||
                     HasAny(EE.SpawnShovelChest, EE.UnlockGeyserWaterfall) ||
                     (Has(EE.SpawnBombsChest) && HasAny(EE.UnlockShopCannon, EE.UnlockGeyserVolcanoe, EE.OpenCastleFloor, EE.UnlockFairyPortal))),
                [EndingTypes.TriggerTrollSpikes] = () => true,
                [EndingTypes.SacrificePrincess] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.HugDarkLord] = () => HasAll(EE.SpawnMrHugsChest, EE.SpawnPrincessItem),
                [EndingTypes.ShotgunFakePrincess] = () => false, // TODO
                [EndingTypes.FakePrincessInsideChest] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.TakePrincessToDarkAltar] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.GetIntoTheCloud] = () => Has(EE.GrowVine) || (HasChicken() && (HasSword() || HasAny(EE.SpawnHookChest, EE.UnlockGeyserWaterfall) ||
                    (CanPassCastleWithItem() && Has(EE.UnlockGeyserWaterfall)))),
                [EndingTypes.KidnapPrincess] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.HugChicken] = () => HasChicken() && Has(EE.SpawnMrHugsChest),
                [EndingTypes.TakeChickenToDarkAltar] = () => HasChicken() && CanPassCastleWithItem(),
                [EndingTypes.PrincessToDesertGate] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.FallIntoWaterfallWithPrincess] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.BreakSpaceTimeContinuum] = () => Has(EE.SpawnWhistleChest),
                [EndingTypes.ShootCannonballToTown] = () => HasNuke() && Has(EE.UnlockDarkCastleCannon) && CanPassCastleWithItem(),
                [EndingTypes.KillAllFairies] = () => HasSword() || Has(EE.SpawnMrHugsChest) || Has(EE.SpawnBoomerang),
                [EndingTypes.MakeBabiesWithPrincess] = () => HasSword() && Has(EE.SpawnPrincessItem),
                [EndingTypes.KillAllDevsHell] = () => CanReachPrincessWithItem() && ((HasSword() && (Has(EE.SpawnHookChest) || (HasChicken() && Has(EE.SpawnShovelChest))) ||
                    (Has(EE.SpawnBoomerang) && HasAny(EE.SpawnHookChest, EE.SpawnShovelChest)))),
                [EndingTypes.DesertEnd] = () => HasWeight(4) && HasAll(EE.SpawnHookChest, EE.UnlockGeyserDesert2),
                [EndingTypes.FindAlienLarvae] = () => Has(EE.SpawnPrincessItem) || (Has(EE.SpawnShovelChest) && (ArchipelagoConnection.hardJumps == 1 ||
                    Has(EE.SpawnLavaTrinketChest))),
                [EndingTypes.FaceDarkLordWithShield] = () => Has(EE.SpawnShieldChest) && CanReachPrincessWithItem() && CanPassCastleWithItem(),
                [EndingTypes.MultipleDesertJumps] = () => HasWeight(4) && Has(EE.SpawnHookChest),
                [EndingTypes.DatePrincessAndDragon] = () => HasAll(EE.SpawnPrincessItem, EE.SpawnDragon),
                [EndingTypes.GiveDarkStoneToDarkLord] = () => HasDarkStone() && Has(EE.SpawnPrincessItem) &&
                    HasAny(EE.SpawnHookChest, EE.UnlockElevatorButton, EE.UnlockCallElevatorButtons),
                [EndingTypes.TakePrincessToLonksHouse] = () => Has(EE.SpawnPrincessItem),
                [EndingTypes.StayInTheWater] = () => true,
                [EndingTypes.AboardPirateShip] = () => true,
                [EndingTypes.SwimIntoTheOcean] = () => true,
                [EndingTypes.FeedTheMimic] = () => HasBurger() && Has(EE.SpawnMimic) &&
                    (HasAny(EE.SpawnHookChest, EE.UnlockElevatorButton, EE.UnlockCallElevatorButtons) || HasAll(EE.GrowVine, EE.UnlockMirrorPortal)),
                [EndingTypes.FeedTheKing] = () => HasBurger() && Has(EE.SpawnKing),
                [EndingTypes.UltimateEnding] = () => false, // TODO
            };
        }
    }
}

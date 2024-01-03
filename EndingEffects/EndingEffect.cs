using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ReventureEndingRando.EndingEffects
{
    abstract class EndingEffect
    {
        public abstract void ActivateEffect(bool endingAchieved);

        public static EndingEffect InitFromEnum(EndingEffectsEnum e)
        {
            switch (e)
            {
                case EndingEffectsEnum.SpawnSwordPedestalItem:
                    return new SpawnSwordPedestal();
                case EndingEffectsEnum.SpawnSwordChest:
                    return new SpawnSwordChest();
                case EndingEffectsEnum.SpawnShovelChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Shovel");
                case EndingEffectsEnum.SpawnBoomerang:
                    return new SpawnItemSimple("World/Items/Item Boomerang");
                case EndingEffectsEnum.SpawnMapChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Map");
                case EndingEffectsEnum.SpawnCompassChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Compass");
                case EndingEffectsEnum.SpawnWhistleChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_WhistleOfTime");
                case EndingEffectsEnum.SpawnBurgerChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Pizza");
                case EndingEffectsEnum.SpawnDarkstoneChest:
                    return new SpawnItemSimple("World/Interactables/Levers/RightLeversPlatform/TreasureChest_DarkStone");
                case EndingEffectsEnum.SpawnHookChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Hook");
                case EndingEffectsEnum.SpawnFishingRodChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_FishingRod");
                case EndingEffectsEnum.SpawnLavaTrinketChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Trinket");
                case EndingEffectsEnum.SpawnMrHugsChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_MrHugs");
                case EndingEffectsEnum.SpawnBombsChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Bomb");
                case EndingEffectsEnum.SpawnShieldChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Shield");
                case EndingEffectsEnum.SpawnNukeItem:
                    return new SpawnItemSimple("World/Items/TreasureChest_Cannonball");
                case EndingEffectsEnum.SpawnPrincessItem:
                    return new SpawnPrincess();
                case EndingEffectsEnum.SpawnAnvilItem:
                    return new SpawnItemSimple("World/Items/AnvilRope/SwingRope/Item Anvil");
                case EndingEffectsEnum.SpawnStrawberry:
                    return new SpawnItemSimple("World/Items/29_ClimbMountain_End");
                case EndingEffectsEnum.UnlockShopCannon:
                    return new SpawnItemSimple("World/Interactables/Cannons/ShopToFortressCannon");
                case EndingEffectsEnum.UnlockCastleToShopCannon:
                    return new SpawnItemSimple("World/Interactables/Cannons/TownToShopCannon");
                case EndingEffectsEnum.UnlockDarkCastleCannon:
                    return new SpawnItemSimple("World/Interactables/Cannons/FortressToTownCannon");
                case EndingEffectsEnum.UnlockCastleToDarkCastleCannon:
                    return new SpawnItemSimple("World/Interactables/Cannons/CastleToFortressCannon");
                case EndingEffectsEnum.UnlockGeyserVolcanoe:
                    return new SpawnItemSimple("World/Interactables/Cannons/CraterCannons/ShootFromCraterCannon");
                case EndingEffectsEnum.UnlockGeyserWaterfall:
                    return new SpawnItemSimple("World/Interactables/Cannons/CraterCannons/ShootFromCraterCannon (1)");
                case EndingEffectsEnum.UnlockGeyserDesert1:
                    return new SpawnItemSimple("World/Interactables/TrollCannons/CannonToDesertStart");
                case EndingEffectsEnum.UnlockGeyserDesert2:
                    return new SpawnItemSimple("World/Interactables/TrollCannons/CannonToTop");
                case EndingEffectsEnum.UnlockElevatorButton:
                    return new SpawnItemSimple("World/Interactables/Elevator Zone/CastleElevator/ElevatorButton");
                case EndingEffectsEnum.UnlockCallElevatorButtons:
                    return new SpawnItemSimple("World/Interactables/Elevator Zone/CallElevatorButtons");
                case EndingEffectsEnum.UnlockMirrorPortal:
                    return new SpawnItemSimple("World/PersistentElements/PrincessWardrove/PrincessPortal");
                case EndingEffectsEnum.UnlockFairyPortal:
                    return new SpawnItemSimple("World/PersistentElements/TownPortals");
                case EndingEffectsEnum.GrowVine:
                    return new SpawnVine();
                case EndingEffectsEnum.OpenCastleFloor:
                    return new OpenCastleHole();
                case EndingEffectsEnum.SpawnDragon:
                    return new SpawnDragon();
                case EndingEffectsEnum.SpawnShopkeeper:
                    return new SpawnItemSimple("World/NPCs/Shopkeepers");
                case EndingEffectsEnum.SpawnMimic:
                    return new SpawnMimic();
                case EndingEffectsEnum.SpawnKing:
                    return new SpawnKing();
                default:
                    return null;
            }
        }
    }

    class SpawnItemSimple : EndingEffect
    {
        string name;

        public SpawnItemSimple(string _name)
        {
            name = _name;
        }

        public override void ActivateEffect(bool endingAchieved)
        {
            GameObject simpleItem = GameObject.Find(name);
            AlterWithRestrictions awRestrictions = simpleItem.GetComponent<AlterWithRestrictions>();
            if (awRestrictions != null)
            {
                GameObject.Destroy(awRestrictions);
            }
            simpleItem.SetActive(endingAchieved);
        }
    }

    class SpawnKing : EndingEffect
    {
        public override void ActivateEffect(bool endingAchieved)
        {
            GameObject king = GameObject.Find("World/NPCs/KindomNPCs/TheKing");
            GameObject feedEnding = GameObject.Find("World/Interactables/98_FeedTheKing_End");
            king.SetActive(endingAchieved);
            feedEnding.SetActive(endingAchieved);
        }
    }
    class SpawnDragon : EndingEffect
    {
        public override void ActivateEffect(bool endingAchieved)
        {
            GameObject dragon = GameObject.Find("World/NPCs/Dragon");
            GameObject roastedEnding = GameObject.Find("World/EndTriggers/13_RoastedByDragon_End");
            GameObject fireTrinketEnding = GameObject.Find("World/EndTriggers/47_DragonWithFireTrinket_End");
            GameObject dateEnding = GameObject.Find("World/EndTriggers/49_DatePrincessAndDragon_End");
            GameObject shieldEnding= GameObject.Find("World/EndTriggers/57_DragonWithShield_End");
            GameObject shieldTrinketEnding = GameObject.Find("World/EndTriggers/58_DragonWithShieldAndFireTrinket_End");
            dragon.SetActive(endingAchieved);
            roastedEnding.SetActive(endingAchieved);
            fireTrinketEnding.SetActive(endingAchieved);
            dateEnding.SetActive(endingAchieved);
            shieldEnding.SetActive(endingAchieved);
            shieldTrinketEnding.SetActive(endingAchieved);
        }
    }

    class SpawnMimic : EndingEffect
    {
        public override void ActivateEffect(bool endingAchieved)
        {
            GameObject mimic = GameObject.Find("World/NPCs/FakePrincess");
            GameObject feedEnding = GameObject.Find("World/EndTriggers/84_FeedTheMimic_End");
            mimic.SetActive(endingAchieved);
            feedEnding.SetActive(endingAchieved);
        }
    }

    class SpawnPrincess : EndingEffect
    {
        public override void ActivateEffect(bool endingAchieved)
        {
            GameObject princess = GameObject.Find("World/NPCs/Item Princess");
            GameObject ventEnding = GameObject.Find("World/EndTriggers/24_AirDuctsAccident_End");
            princess.SetActive(endingAchieved);
            ventEnding.SetActive(endingAchieved);
        }
    }

    class SpawnSwordPedestal: EndingEffect
    {
        public override void ActivateEffect(bool endingAchieved)
        {
            //TODO remove chest completely
            GameObject itemSword = GameObject.Find("World/Items/Sword Item Pedestal/Item Sword");
            GameObject pedestal = GameObject.Find("World/Items/Sword Item Pedestal");
            itemSword.SetActive(endingAchieved);
            pedestal.SetActive(endingAchieved);
        }
    }

    class SpawnSwordChest : EndingEffect
    {
        public override void ActivateEffect(bool endingAchieved)
        {
            //TODO remove chest completely
            GameObject swordChest = GameObject.Find("World/Items/SwordAtHome/TreasureChest_Sword");
            GameObject openChest = GameObject.Find("World/Items/SwordAtHome/OpenChest");
            GameObject swordAtHome = GameObject.Find("World/Items/SwordAtHome");
            swordAtHome.SetActive(endingAchieved);
            openChest.SetActive(!endingAchieved);
            swordChest.SetActive(endingAchieved);
        }
    }

    class SpawnVine : EndingEffect
    {
        public override void ActivateEffect(bool endingAchieved)
        {
            //TODO remove chest completely
            GameObject badCrops = GameObject.Find("World/BackgroundElements/BadCrops");
            GameObject goodCrops = GameObject.Find("World/PersistentElements/GoodCrops");
            GameObject cropsCloud = GameObject.Find("World/PersistentElements/CropsClouds");
            badCrops.SetActive(!endingAchieved);
            goodCrops.SetActive(endingAchieved);
            cropsCloud.SetActive(endingAchieved);
        }
    }

    class OpenCastleHole : EndingEffect
    {
        public override void ActivateEffect(bool endingAchieved)
        {
            //TODO remove chest completely
            GameObject castleHole = GameObject.Find("World/PersistentElements/Castlehole");
            GameObject boulderUnderCastle = GameObject.Find("World/Boulders/BoulderUnderCastle");
            castleHole.SetActive(!endingAchieved);
            boulderUnderCastle.SetActive(!endingAchieved);
        }
    }

    public enum EndingEffectsEnum
    {
        Nothing,
        //Item Locations
        SpawnSwordPedestalItem,
        SpawnSwordChest,
        SpawnShovelChest,
        SpawnBoomerang,
        SpawnMapChest,
        SpawnCompassChest,
        SpawnWhistleChest,
        SpawnBurgerChest,
        SpawnDarkstoneChest,
        SpawnHookChest,
        SpawnFishingRodChest,
        SpawnLavaTrinketChest,
        SpawnMrHugsChest,
        SpawnBombsChest,
        SpawnShieldChest,
        SpawnNukeItem,
        SpawnPrincessItem,
        SpawnAnvilItem,
        SpawnStrawberry,
        //Transportation/Paths
        UnlockShopCannon,
        UnlockCastleToShopCannon,
        UnlockDarkCastleCannon,
        UnlockCastleToDarkCastleCannon,
        UnlockGeyserDesert1,
        UnlockGeyserDesert2,
        UnlockGeyserVolcanoe,
        UnlockGeyserWaterfall,
        UnlockElevatorButton,
        UnlockCallElevatorButtons,
        UnlockMirrorPortal,
        UnlockFairyPortal,
        GrowVine,
        OpenCastleFloor,
        //NPCs
        SpawnDragon,
        SpawnShopkeeper,
        SpawnMimic,
        SpawnKing,
        ////Cosmetic
        //EnableNamechange,
        //EnablePrincessNamechange,
        //EnableDarkLordNamechange,
        //EnableCloset,
        //BuildStatue,
        //AddPC,
        //SpawnDolphins,
        //SpawnMimicPet,
    }
}

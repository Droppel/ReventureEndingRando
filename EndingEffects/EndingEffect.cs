using Atto;
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
        public abstract void ActivateEffect(int effectsReceived);

        public void RemoveAlterWithObjects(GameObject go)
        {
            AlterWithRestrictions awRestrictions = go.GetComponent<AlterWithRestrictions>();
            if (awRestrictions != null)
            {
                GameObject.Destroy(awRestrictions);
            }
            AlterWithEnding awEnding = go.GetComponent<AlterWithEnding>();
            if (awEnding != null)
            {
                GameObject.Destroy(awEnding);
            }
        }

        public static EndingEffect InitFromEnum(EndingEffectsEnum e)
        {
            switch (e)
            {
                case EndingEffectsEnum.ProgressiveSword:
                    return new SpawnSword();
                case EndingEffectsEnum.SpawnShovelChest:
                    return new SpawnItemSimple("World/Items/TreasureChest_Shovel");
                case EndingEffectsEnum.SpawnBoomerang:
                    return new SpawnBoomerang();
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
                case EndingEffectsEnum.OpenSewerPipe:
                    return new SpawnItemSimple("World/PersistentElements/PipeGrate", true);
                case EndingEffectsEnum.DarkstoneLeverLeft:
                    return new SpawnItemSimple("World/Interactables/Levers/ActiveLevers/LeftLever");
                case EndingEffectsEnum.DarkstoneLeverMiddle:
                    return new SpawnItemSimple("World/Interactables/Levers/MiddleLever");
                case EndingEffectsEnum.DarkstoneLeverRight:
                    return new SpawnItemSimple("World/Interactables/Levers/ActiveLevers/RightLever");
                case EndingEffectsEnum.SpawnDragon:
                    return new SpawnDragon();
                case EndingEffectsEnum.SpawnShopkeeper:
                    return new SpawnItemSimple("World/NPCs/Shopkeepers");
                case EndingEffectsEnum.SpawnMimic:
                    return new SpawnMimic();
                case EndingEffectsEnum.SpawnKing:
                    return new SpawnKing();
                case EndingEffectsEnum.GrowChicken:
                    return new GrowChicken();
                case EndingEffectsEnum.SpawnElder:
                    return new SpawnItemSimple("World/NPCs/Elder");
                case EndingEffectsEnum.SpawnBoulderNPC:
                    return new SpawnBoulderNPC();
                case EndingEffectsEnum.EnableCloset:
                    return new SpawnItemSimple("World/PersistentElements/Wardrove");
                case EndingEffectsEnum.BuildStatue:
                    return new SpawnItemSimple("World/PersistentElements/PrincessStatue");
                case EndingEffectsEnum.AddPC:
                    return new AddPC();
                case EndingEffectsEnum.SpawnDolphins:
                    return new SpawnItemSimple("World/PersistentElements/Dolphins");
                case EndingEffectsEnum.SpawnMimicPet:
                    return new SpawnItemSimple("World/PersistentElements/MimicKennel");
                case EndingEffectsEnum.UnlockFacePlantStone:
                    return new UnlockFacePlantStone();
                case EndingEffectsEnum.Gem:
                    return new UnlockGems();
                default:
                    return null;
            }
        }
    }

    class SpawnItemSimple : EndingEffect
    {
        string name;
        bool inverted;

        public SpawnItemSimple(string _name, bool _inverted=false)
        {
            name = _name;
            inverted = _inverted;
        }

        public override void ActivateEffect(int effectsReceived)
        {
            GameObject simpleItem = GameObject.Find(name);
            RemoveAlterWithObjects(simpleItem);
            simpleItem.SetActive(inverted != effectsReceived > 0);
        }
    }

    class SpawnKing : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject king = GameObject.Find("World/NPCs/KindomNPCs/TheKing");
            GameObject feedEnding = GameObject.Find("World/Interactables/98_FeedTheKing_End");
            king.SetActive(effectsReceived > 0);
            feedEnding.SetActive(effectsReceived > 0);
        }
    }
    class SpawnBoomerang: EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            if (effectsReceived == 0)
            {
                return;
            }
            GameObject boomerang = GameObject.Find("World/Items/Item Boomerang");
            GameObject shovelChest = GameObject.Find("World/Items/TreasureChest_Shovel");
            GameObject boomerangChest = GameObject.Instantiate(shovelChest, new Vector2(297, -2.5f), Quaternion.identity, shovelChest.transform.parent);
            boomerangChest.name = "TreasureChest_Boomerang";
            TreasureChest boomerangTreasureChest = boomerangChest.GetComponent<TreasureChest>();
            boomerangTreasureChest.content = boomerang;
            boomerangTreasureChest.item = ItemTypes.Boomerang;
            boomerang.transform.position = new Vector3(297, 5, 1);
            boomerangChest.SetActive(true);
            boomerang.SetActive(true);

            GameObject extraPlatform = GameObject.Instantiate(GameObject.Find("World/PersistentElements/Castlehole/BreakableBlock (9)"), new Vector2(323.5f, -3.5f), Quaternion.identity);
            extraPlatform.name = "BoomerangExtraPlatform";
            extraPlatform.SetActive(true);
        }
    }

    class SpawnDragon : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject dragon = GameObject.Find("World/NPCs/Dragon");
            GameObject roastedEnding = GameObject.Find("World/EndTriggers/13_RoastedByDragon_End");
            GameObject fireTrinketEnding = GameObject.Find("World/EndTriggers/47_DragonWithFireTrinket_End");
            GameObject dateEnding = GameObject.Find("World/EndTriggers/49_DatePrincessAndDragon_End");
            GameObject shieldEnding= GameObject.Find("World/EndTriggers/57_DragonWithShield_End");
            GameObject shieldTrinketEnding = GameObject.Find("World/EndTriggers/58_DragonWithShieldAndFireTrinket_End");
            dragon.SetActive(effectsReceived > 0);
            roastedEnding.SetActive(effectsReceived > 0);
            fireTrinketEnding.SetActive(effectsReceived > 0);
            dateEnding.SetActive(effectsReceived > 0);
            shieldEnding.SetActive(effectsReceived > 0);
            shieldTrinketEnding.SetActive(effectsReceived > 0);
        }
    }

    class SpawnMimic : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject mimic = GameObject.Find("World/NPCs/FakePrincess");
            GameObject feedEnding = GameObject.Find("World/EndTriggers/84_FeedTheMimic_End");
            mimic.SetActive(effectsReceived > 0);
            feedEnding.SetActive(effectsReceived > 0);
        }
    }

    class SpawnPrincess : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject princess = GameObject.Find("World/NPCs/Item Princess");
            GameObject ventEnding = GameObject.Find("World/EndTriggers/24_AirDuctsAccident_End");
            princess.SetActive(effectsReceived > 0);
            ventEnding.SetActive(effectsReceived > 0);
        }
    }

    class SpawnSword : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            // If the treasureRoomSword is not in logic, simply increase the itemcount by one
            if (ArchipelagoConnection.treasureRoomSword == 0)
            {
                effectsReceived += 1;
            }
            //Treasureroom Sword
            GameObject treasureSword = GameObject.Find("World/PersistentElements/TreasureLonk/Item Sword");
            RemoveAlterWithObjects(treasureSword);
            treasureSword.SetActive(effectsReceived > 0);

            //Mountain Sword
            GameObject itemSword = GameObject.Find("World/Items/Sword Item Pedestal/Item Sword");
            GameObject pedestal = GameObject.Find("World/Items/Sword Item Pedestal");
            itemSword.SetActive(effectsReceived > 1);
            pedestal.SetActive(effectsReceived > 1);

            //Home Sword
            //TODO remove chest completely
            GameObject swordChest = GameObject.Find("World/Items/SwordAtHome/TreasureChest_Sword");
            GameObject openChest = GameObject.Find("World/Items/SwordAtHome/OpenChest");
            GameObject swordAtHome = GameObject.Find("World/Items/SwordAtHome");
            openChest.SetActive(effectsReceived <= 2);
            swordAtHome.SetActive(effectsReceived > 2);
            swordChest.SetActive(effectsReceived > 2);
        }
    }

    class SpawnVine : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject badCrops = GameObject.Find("World/BackgroundElements/BadCrops");
            GameObject goodCrops = GameObject.Find("World/PersistentElements/GoodCrops");
            GameObject cropsCloud = GameObject.Find("World/PersistentElements/CropsClouds");
            badCrops.SetActive(effectsReceived == 0);
            goodCrops.SetActive(effectsReceived > 0);
            cropsCloud.SetActive(effectsReceived == 0);
        }
    }

    class OpenCastleHole : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject castleHole = GameObject.Find("World/PersistentElements/Castlehole");
            castleHole.SetActive(effectsReceived == 0);
            // Keep this enabled, by vanilla settings. The boulder has no collision, but the ending is still possible
            //GameObject boulderUnderCastle = GameObject.Find("World/Boulders/BoulderUnderCastle");
            //boulderUnderCastle.SetActive(effectsReceived == 0);
        }
    }
    class UnlockFacePlantStone : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject ending = GameObject.Find("Cinematics/75_LonkFaceplant_End");
            EndTrigger trigger = ending.GetComponent<EndTrigger>();
            EndingCountRequirement endingReq = (EndingCountRequirement) trigger.triggerRequirements[2];
            endingReq.endingsUnlockedCount = 0;
            ending.SetActive(effectsReceived > 0);
        }
    }


    class AddPC : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject pc = GameObject.Find("World/PersistentElements/Lonk's PC");
            GameObject pcalt = GameObject.Find("World/PersistentElements/Lonk's PC Alt");
            pc.SetActive(effectsReceived > 0);
            pcalt.SetActive(effectsReceived > 0);
        }
    }

    class GrowChicken : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject phase0 = GameObject.Find("World/PersistentElements/ChickenNest/Phase0");
            GameObject phase1 = GameObject.Find("World/PersistentElements/ChickenNest/Phase1");
            GameObject phase2 = GameObject.Find("World/PersistentElements/ChickenNest/Phase2");
            GameObject phase3 = GameObject.Find("World/PersistentElements/ChickenNest/Phase3");
            GameObject.Destroy(phase0.GetComponent<AlterWithRestrictions>());
            GameObject.Destroy(phase1.GetComponent<AlterWithRestrictions>());
            GameObject.Destroy(phase2.GetComponent<AlterWithRestrictions>());
            GameObject.Destroy(phase3.GetComponent<AlterWithRestrictions>());

            phase0.SetActive(effectsReceived == 1);
            phase1.SetActive(effectsReceived == 2);
            phase2.SetActive(effectsReceived == 3);
            phase3.SetActive(effectsReceived == 4);
        }
    }

    class SpawnBoulderNPC : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            GameObject boulder = GameObject.Find("World/Boulders/NonNPC Boulder");
            GameObject.Destroy(boulder.GetComponent<AlterWithRestrictions>());
            GameObject boulderNPC = GameObject.Find("World/Boulders/NPC Boulder");
            GameObject.Destroy(boulderNPC.GetComponent<AlterWithRestrictions>());

            boulder.SetActive(effectsReceived == 0);
            boulderNPC.SetActive(effectsReceived > 0);
        }
    }

    class UnlockMilestone : EndingEffect
    {
        MilestoneTypes milestone;

        public UnlockMilestone(MilestoneTypes _milestone)
        {
            milestone = _milestone;
        }

        public override void ActivateEffect(int effectsReceived)
        {
            if (effectsReceived > 0)
            {
                IProgressionService progression = Core.Get<IProgressionService>();
                progression.UnlockMilestone(milestone);
            }
        }
    }

    class UnlockGems : EndingEffect
    {
        public override void ActivateEffect(int effectsReceived)
        {
            int requiredGems = (ArchipelagoConnection.gemsAmount * ArchipelagoConnection.gemsRequired) / 100;
            if (effectsReceived >= requiredGems)
            {
                IProgressionService progression = Core.Get<IProgressionService>();
                progression.UnlockMilestone(MilestoneTypes.GotEarthGem);
                progression.UnlockMilestone(MilestoneTypes.GotWaterGem);
                progression.UnlockMilestone(MilestoneTypes.GotFireGem);
                progression.UnlockMilestone(MilestoneTypes.GotWindGem);
            }
        }
    }

    public enum EndingEffectsEnum
    {
        Nothing,
        //Item Locations
        ProgressiveSword,
        UNUSED, //Unused
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
        UnlockFacePlantStone,
        OpenSewerPipe,
        DarkstoneLeverLeft,
        DarkstoneLeverMiddle,
        DarkstoneLeverRight,
        //NPCs
        SpawnDragon,
        SpawnShopkeeper,
        SpawnMimic,
        SpawnKing,
        GrowChicken,
        SpawnElder,
        SpawnBoulderNPC,
        //Cosmetic
        EnableCloset,
        BuildStatue,
        AddPC,
        SpawnDolphins,
        SpawnMimicPet,
        //Gems
        Gem
    }

    //Selected for implementation

    //Effect ideas
    //schornstein World/EndTriggers/79_EnterTheChimney_End
    //EnableNamechange,
    //EnablePrincessNamechange
    //EnableDarkLordNamechange
    //desert druckplatte
    //mimic kiste
    //princess bed
    //own bed
    //Altar
    //River Grate Button
    //Selfdestruct Button
    //Princessgate Elevator
    //Xray Goggles
}

# ReventureEndingRando

## Installation
1. Download [BepinEx](https://github.com/BepInEx/BepInEx/releases)https://github.com/BepInEx/BepInEx/releases you need the x86 version
2. Extract the files in the .zip into the Reventure Folder, so they are all next to the Reventure.exe
3. Start Reventure once, if Bepinex is installed correctly, there should now be multiple folders in /BepInEx/
4. Download the files from the current release
5. Put the ReventureEndingRando.dll into the /BepInEx/plugins folder
6. Put the Archipelago.MultiClient.Net.dllinto /BepInEx/core folder
7. Create a file named "connectioninfo.txt" in the games main folder (So next to Reventure.exe)
8. In this file add the connection info like so: <host>;<port>;<slotname> (e.g: localhost;38281;ReventureSlot or archipelago.gg;56372;DroppelReventure) 
9. Start the game and start a new file (Saveslots can be delted by holding down "delete" for 10 seconds)


## World Generation
Currently the only setting is "endings". This can be any Value between 0 and 99. It reflects the amount of endings required, to trigger the final ending.

## Changes to Vanilla
All normaly accesible items are now disabled by default. This includes all chest items, the strawberry, the anvil and the princess.
The various cannons and geysers need to be unlocked first.
The buttons to call/use the elevator need to be unlocked.
The two portals are now unlocked as AP Items instead of by their respective endings
The stone you trip over, opening the castle floor and growing the vine are all AP Items
Various NPCs only spawn after getting their respective AP item. This includes the chicken which requires all 4 GrowChicken items to be available.
The 4 Gems are now AP items. These are always required to finish the game. So for now consider adding them to the starting_inventory if you want to guerantee a short game.
Some mostly cosmetic changes to the world are AP unlocks instead of requiring certain endings.

All Endings that require a specific amount of endings are now always available.

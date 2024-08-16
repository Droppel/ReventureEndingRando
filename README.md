# ReventureEndingRando

## Installation
1. Download [BepinEx](https://github.com/BepInEx/BepInEx/releases)https://github.com/BepInEx/BepInEx/releases you need the x86 version
2. Extract the files in the .zip into the Reventure Folder, so they are all next to the Reventure.exe
3. Start Reventure once, if Bepinex is installed correctly, there should now be multiple folders in /BepInEx/
4. Download the files from the current release
5. Put the ReventureEndingRando.dll into the /BepInEx/plugins folder
6. Put the Archipelago.MultiClient.Net.dllinto /BepInEx/core folder
7. Start the game. BEFORE selecting a new save file, press F5.
8. In the now open menu input <host>:<port> into the upper input field and your slot name into the lower inputfield.
9. Start a new file (Saveslots can be delted by holding down "delete" for 10 seconds)
10. The mod remembers the connection info for existing files. So when you want to continue later, just load the associated save file (No need to use F5 again)

## Settings
| Setting      | Range   | Default | Description                              |
|--------------|---------|---------|------------------------------------------|
|endings       | 0-99    | 40      |The amount of endings required to finish the game|
|randomizeGems | boolean | true    |If the gem unlocks are randomized|
|gemsInPool    | 0-40    | 4       |How many gems are in the pool|
|gemsRequired  | 0-100   | 75      |What percentage (rounded down) of the gems are required to open the ultimatre door|
|hardjumps     | boolean | false   |This includes jumps in logic that are difficult and result in death if missed|
|hardcombat    | boolean | false   |This adds ending 49 into logic without shield|
|treasureSword | boolean | false   |This adds the sword in the treasure room into the sword progression|


## Changes to Vanilla
All normally accessible items are now disabled by default. This includes all chest items, the strawberry, the anvil and the princess.
The various cannons and geysers need to be unlocked first.
The buttons to call/use the elevator need to be unlocked.
The two portals are now unlocked as AP Items instead of by their respective endings.
The stone you trip over, opening the castle floor and growing the vine are all AP Items.
Various NPCs only spawn after getting their respective AP item. This includes the chicken which requires all 4 GrowChicken items to be available.

All Endings that require a specific amount of endings are now always available.

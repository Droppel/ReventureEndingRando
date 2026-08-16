# ReventureEndingRando

This repository contains the tools necessary to plain the ReventureEndingRando. This includes the Client, the APWorld and an optional logic generation tool called RegionsGenerator, that allows to use more advanced settings, at the cost of higher setup difficulty.

These projects were merged into one repository on 16.08.2026. To see the respective history of ReventureRegions and the APWorld please check:
https://github.com/Droppel/reventure-archipelago
https://github.com/Droppel/reventurerust

AI Notice:
I have used LLMs for brainstorming/solving problems and the inline auto complete extensively. Nonetheless for both the Client and APWorld every single line was in the end a conscious decision by myself to add.
For the RegionsGenerator AI has additionally been used to port the original implementation written in python to rust. Afterwards I reworked the project massively (Including completely dropping a massive step in the algorithm that was already unnecessary in the python implementation) and at this point I again feel comfortable to call the project to be fully understood and vetted by me.

# Installation

## Client Installation
1. Download [BepinEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.2) you need the win_x86 version
2. Extract the files in the .zip into the Reventure Folder, so they are all next to the Reventure.exe
3. LINUX ONLY: Add the following Launch Parameters in Steam: "WINEDLLOVERRIDES="winhttp=n,b" %command%"
4. Start Reventure once, if Bepinex is installed correctly, there should now be multiple folders in /BepInEx/
5. Download the files from the current release
6. Put the ReventureEndingRando.dll into the /BepInEx/plugins folder
7. Put the Archipelago.MultiClient.Net.dllinto /BepInEx/core folder
8. Start the game.
9. In the top left input \<host\>:\<port\> into the upper input field and your slot name into the lower inputfield.
10. Start a new file (Saveslots can be deleted by holding down "delete" for 10 seconds)
11. The mod remembers the connection info for existing files. So when you want to continue later, just load the associated save file (No need to use F5 again)

## APWorld Installation
Simply drag and drop the downloaded APWorld onto your Archipelago launcher window.

# Generating

In your Archipelago Launcher click on "Generate Templates". Then grab the template from the output folder and adjust it to your liking.

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

## Advanced Generation
[ALPHA] Due to some particular problems with Reventures weight concept, a lot of advanced features (Itemlocation shuffle, random starting position, etc.) are impossible to implement within the normal constraints of Archipelago. For this reason I built a tool that generates completely new and unique logic rules based on these settings, that can then be used to generate a slot for a normal Archipelago game. This is by no means necessary and certainly not suggested for people playing this for the first time. But if you want a fresh spin on the randomizer, do give it a try!

1. When creating your YAML make sure to set experimentalRegionGraph to true
2. Run the regiongenerator tool INSIDE the folder where your yaml is located. Due to the relatively simple approach I take to editing the yaml (I just append at the end), make sure Reventure is the last game inside the yaml.
3. The tool outputs the number of possible item locations (Due to the random item placement, some locations might be impossible). If this number is to low, you should remove the logic from your yaml and try again. Most of the time everything is possible.
4. Give your yaml to the host, as normal
5. The host needs to set reventure_options => allow_experimental to true in their host.yaml
6. Generate the world

# Playing

1. Enter your connections details in the top left.
2. Click on a new savefile.
3. When reconnecting, simply select your save slot, no need to reenter your connection details.
4. If anything breaks, simply delete the save and restart at 1. The game is designed to completely sync the state from the Server, so no progress is lost.

## Changes to Vanilla
All normally accessible items are now disabled by default. This includes all chest items, the strawberry, the anvil and the princess.
The various cannons and geysers need to be unlocked first.
The buttons to call/use the elevator need to be unlocked.
The two portals are now unlocked as AP Items instead of by their respective endings.
The stone you trip over, opening the castle floor and growing the vine are all AP Items.
Various NPCs only spawn after getting their respective AP item. This includes the chicken which requires all 4 GrowChicken items to be available.

All Endings that require a specific amount of endings are now always available.

## Ingame Tracker
Endings marked with the hint icon in the ending overview are currently in logic. The normal hint menu where you can select the hint the compass and map show you still works the same way.

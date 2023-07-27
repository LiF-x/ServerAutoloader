/*
LiFx - AutoloadConfig
-1 = Uninstall - remove all associated files
0 = Do nothing - excisting files will remain intact
1 = Download but do not execute
2 = Download and execute
*/
// Custom settings and your own mods

// -------------------- DO NOT ADD NEW VARIABLES BELOW THIS LINE ----------------------------- //
$LiFx::createDataXMLS = false; // To create recipe, recipe_requirements and objects_types xml from the database

// Offline Raid Protection
$LiFx::raidProtection::timeToProtection = 5; // Defaults to 5 min check interval after people disconnect.

// Online alignment config
$LiFx::AlignmentUpdateMinutes = 1;
$LiFx::AlignmentUpdateDelta = 1;

// LiFx Loot config
$LiFx::loot::numDrops = 4; // number of drops

$LiFx::autoLoadVersion = "410"; // Do not edit

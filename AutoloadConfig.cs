/*
LiFx - AutoloadConfig
-1 = Uninstall - remove all associated files
0 = Do nothing - excisting files will remain intact
1 = Download but do not execute
2 = Download and execute
*/
// Custom settings and your own mods

// -------------------- DO NOT ADD NEW VARIABLES BELOW THIS LINE ----------------------------- //
$LiFx::createDataXMLS = true; // To create recipe, recipe_requirements and objects_types xml from the database

// Offline Raid Protection
$LiFx::raidProtection::timeToProtection = 15; // Defaults to 15 min check interval after people disconnect.
// Tier 1 monument
$LiFx::raidProtection::t1::Cost = 0;       // Points to deduct from guild monument when shield is raised
$LiFx::raidProtection::t1::Enabled = true; // True | False
// Tier 2 monument
$LiFx::raidProtection::t2::Cost = 0;       // Points to deduct from guild monument when shield is raised
$LiFx::raidProtection::t2::Enabled = true; // True | False
// Tier 3 monument
$LiFx::raidProtection::t3::Cost = 0;       // Points to deduct from guild monument when shield is raised
$LiFx::raidProtection::t3::Enabled = true; // True | False
// Tier 4 monument
$LiFx::raidProtection::t4::Cost = 0;       // Points to deduct from guild monument when shield is raised
$LiFx::raidProtection::t4::Enabled = true; // True | False

// Online alignment config
$LiFx::AlignmentUpdateMinutes = 1;
$LiFx::AlignmentUpdateDelta = 1;

// LiFx Loot config
$LiFx::loot::numDrops = 4; // number of drops

$LiFx::autoLoadVersion = "430"; // Do not edit

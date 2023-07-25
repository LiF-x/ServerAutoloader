/**
* <author>Christophe Roblin</author>
* <url>https://github.com/LiF-x</url>
* <credits>https://github.com/LiF-x</credits>
* <description>Public repository to let everyone help on core functionality added to the LiFx serverautoloader</description>
* <license>GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007</license>
*/

exec("./jettison.cs"); // Credits to Nyuton for his recommendations to this
exec("./utility.cs");
exec("./sha256.cs");
activatePackage(LiFxUtility);
if (!isObject(LiFx))
{
    new ScriptObject(LiFx)
    {
      CacheDIR = $Con::root;
    };
}
if (!isObject(LiFxTicks))
{
    new ScriptObject(LiFxTicks)
    {
    };
}
if (!isObject(LiFxDBTicks))
{
    new ScriptObject(LiFxDBTicks)
    {
    };
}

if (!isObject(ObjectsTypes))
{
    new ScriptObject(ObjectsTypes)
    {
        ID = 0;
        ParentID = 0;
        ObjectName = "";
        isContainer = 0;
        IsMovableObject = 0;
        IsUnmovableObject = 0;
        IsTool = 0;
        IsDevice = 0;
        IsDoor = 0;
        IsPremium = 0;
        MaxContSize = 0;
        Length = 0;
        MaxStackSize = 0;
        UnitWeight = 0;
        BackgrndImage = "";
        WorkAreaTop = 0;
        WorkAreaLeft = 0;
        WorkAreaWidth = 0;
        WorkAreaHeight = 0;
        BtnCloseTop = 0;
        BtnCloseLeft = 0;
        FaceImage = "";
        Description = "";
        BasePrice = 0;
        OwnerTimeout = 0;
        AllowExportFromRed = 0;
        AllowExportFromGreen = 0;
    };
}
$LiFx::debug = 0;
$LiFx::Version = "v4.0.1";
$LiFx::createDataXMLS = false;
$LiFx::hooks::onSpawnCallbacks = JettisonArray("onSpawnCallbacks");
$LiFx::hooks::onConnectCallbacks = JettisonArray("onConnectCallbacks");
$LiFx::hooks::onConnectRequestCallbacks = JettisonArray("onConnectRequestCallbacks");
$LiFx::hooks::onPostConnectRoutineCallbacks = JettisonArray("onPostConnectRoutineCallbacks");
$LiFx::hooks::onPreConnectRequestCallbacks = JettisonArray("onPreConnectRequestCallbacks");
$LiFx::hooks::onDisconnectCallbacks = JettisonArray("onDisconnectCallbacks");
$LiFx::hooks::onDeathCallbacks = JettisonArray("onDeathCallbacks");
$LiFx::hooks::onKillCallbacks = JettisonArray("onKillCallbacks");
$LiFx::hooks::onSuicideCallbacks = JettisonArray("onSuicideCallbacks");
$LiFx::hooks::onJHStartCallbacks = JettisonArray("onJHStartCallbacks");
$LiFx::hooks::onJHEndCallbacks = JettisonArray("onJHEndCallbacks");
$LiFx::hooks::onServerCreatedCallbacks = JettisonArray("onServerCreatedCallbacks");
$LiFx::hooks::onDestroyServerCallbacks = JettisonArray("onDestroyServerCallbacks");
$LiFx::hooks::onStartCallbacks = JettisonArray("onStartCallbacks");
$LiFx::hooks::onPostInitCallbacks = JettisonArray("onPostInitCallbacks");
$LiFx::hooks::onInitServerCallbacks = JettisonArray("onInitServerCallbacks");
$LiFx::hooks::onInitServerDBChangesCallbacks = JettisonArray("onInitServerDBChangesCallbacks");
$LiFx::hooks::onStartSwimCallbacks = JettisonArray("onStartSwimCallbacks");
$LiFx::hooks::onStopSwimCallbacks = JettisonArray("onStopSwimCallbacks");
$LiFx::hooks::onTick = JettisonArray("onTick");
$LiFx::hooks::mods = JettisonArray("mods");
package LiFx
{
    function LiFx::registerCallback(%callbackArray, %function, %object)
    {
        if (!%callbackArray)
        {
            return;
        }
        LiFx::debugEcho("Length is" SPC %callbackArray.Length);
        %found = false;
        if (%callbackArray.Length > 0)
        {
            for (%i = 0; %i < %callbackArray.Length; %i++)
            {
                if (%callbackArray.value[%i] $= %function || %callbackArray.value[%i] $= (%object @ "|" @ %function))
                {
                    LiFx::debugEcho("!! Did not add" SPC %callbackArray.getName() SPC %function SPC %object);
                    %found = true;
                    break;
                }
            }
        }
        if(!%found) {
          if (isObject(%object))
          {
              %callbackArray.push("string", %object @ "|" @ %function);
          }
          else
          {
              %callbackArray.push("string", %function);
          }
        }
    }
    function LiFx::ObjectsTypesDisableAutoIncrement()
    {
        %file = new FileObject(){};
        %data = "";
        %dumpeofconfig = "-- LiFx Framework anything below this line will be reset on server boot up";
        
        %file.openForRead("sql/dump.sql");
        if (%file)
        {
            %alter1 = "ALTER TABLE `objects_types` ALTER `ID` DROP DEFAULT;";
            %read1 = 0;
            %alter2 = "ALTER TABLE `objects_types`\tCHANGE COLUMN `ID` `ID` INT(10) UNSIGNED NOT NULL FIRST;";
            %read2 = 0;
            %alter3 = "ALTER TABLE `recipe` AUTO_INCREMENT=1087;";
            %read3 = 0;
            %alter4 = "ALTER TABLE `objects_types` DISABLE KEYS;";
            %read4 = 0;
            %alter5 = "SET FOREIGN_KEY_CHECKS=0;";
            %read5 = 0;
            while (!%file.isEOF())
            {
                %dataLine = %file.readLine();
                if (%dataLine $= %alter1)
                {
                    %read1 = 1;
                }
                if (%dataLine $= %alter2)
                {
                    %read2 = 1;
                }
                if (%dataLine $= %alter3)
                {
                    %read3 = 1;
                }
                if (%dataLine $= %alter4)
                {
                    %read4 = 1;
                }
                if (%dataLine $= %alter5)
                {
                    %read5 = 1;
                }
                %data = %data @ %dataLine @ "\n";
                if ((((%read1 && %read2) && %read3) && %read4) && %read5)
                {
                    continue;
                }
            }
            %file.close();
            %file.openForWrite("sql/dump.sql");
            %file.writeLine(%data);

            if (((%read1 && %read2) && %read3) && %read4)
            {
                %file.close();
                return;
            }
            %file.writeLine("");
            %file.writeLine(%alter1);
            %file.writeLine(%alter2);
            %file.writeLine(%alter3);
            %file.writeLine(%alter4);
            %file.writeLine(%alter5);
            %file.writeLine(%dumpeofconfig);
            %file.close();
        }
    }
    function LiFx::registerObjectsTypes(%object, %class)
    {
        if (!isObject(%object) && !(%object.ID))
        {
            error("Invalid object types object");
            LiFx::debugEcho("Invalid object types object");
            quit();
            return;
        }
        %found = 0;
        %objectSQL = "INSERT INTO `objects_types` VALUES (" @ %object.ID @ "," @ %object.ParentID @ ",\'" @ %object.ObjectName @ "\'," @ %object.isContainer @ "," @ %object.IsMovableObject @ "," @ %object.IsUnmovableObject @ "," @ %object.IsTool @ "," @ %object.IsDevice @ "," @ %object.IsDoor @ "," @ %object.IsPremium @ "," @ %object.MaxContSize @ "," @ %object.Length @ "," @ %object.MaxStackSize @ "," @ %object.UnitWeight @ ",\'" @ %object.BackgrndImage @ "\'," @ %object.WorkAreaTop @ "," @ %object.WorkAreaLeft @ "," @ %object.WorkAreaWidth @ "," @ %object.WorkAreaHeight @ "," @ %object.BtnCloseTop @ "," @ %object.BtnCloseLeft @ ",\'" @ %object.FaceImage @ "\',\'" @ %object.Description @ "\'," @ %object.BasePrice @ "," @ %object.OwnerTimeout @ "," @ %object.AllowExportFromRed @ "," @ %object.AllowExportFromGreen @ ");";
        %file = new FileObject("")
        {
        };
        %file.openForRead("sql/dump.sql");
        if (%file)
        {
            while (!%file.isEOF())
            {
                %data = %file.readLine();
                if (%data $= %objectSQL)
                {
                    %found = 1;
                    break;
                }
            }
            %file.close();
            if (%found)
            {
                LiFx::debugEcho("dump.sql already updated");
                return;
            }
            %file.openForAppend("sql/dump.sql");
            LiFx::debugEcho("Write SQL: " SPC %objectSQL);
            %file.writeLine("\n" @ %objectSQL);
            %file.close();
        }
    }
    function LiFx::executeCallback(%array)
    {
        LiFx::executeCallback(%array, null, null, null, null, null);
    }
    function LiFx::executeCallback(%array, %ar1)
    {
        LiFx::executeCallback(%array, %ar1, null, null, null, null);
    }
    function LiFx::executeCallback(%array, %ar1, %ar2, %ar3, %ar4, %ar5)
    {
        if (%array.Length > 0)
        {
            %i = 0;
            while (%i < %array.Length)
            {
                %data = %array.value[%i];
                LiFx::debugEcho("Callback attempt on: " @ %data);
                if (isFunction(%data))
                {
                    call(%data, %ar1, %ar2, %ar3, %ar4, %ar5);
                }
                else
                {
                    if (strpos(%data, "|") > 0)
                    {
                        %func = nextToken(%data, "obj", "|");
                        if (isObject(%obj) && %obj.isMethod(%func))
                        {
                            LiFx::debugEcho(%obj.call(%func, %ar1, %ar2, %ar3, %ar4, %ar5));
                        }
                    }
                }
                %i = %i + 1;
            }
        }
    }
    function spawnPlayer(%client)
    {
        Parent::spawnPlayer(%client);
        LiFx::executeCallback($LiFx::hooks::onSpawnCallbacks, %client);
    }
    function GameConnection::onClientEnterGame(%this)
    {
        Parent::onClientEnterGame(%this);
        LiFx::executeCallback($LiFx::hooks::onConnectCallbacks, %this);
    }
    function GameConnection::postConnectRoutine(%this)
    {
        LiFx::executeCallback($LiFx::hooks::onPostConnectRoutineCallbacks, %this);
        Parent::postConnectRoutine(%this);
    }
    function GameConnection::onConnectRequest(%this, %netAddress, %name)
    {
        %message = LiFx::executeCallback($LiFx::hooks::onConnectRequestCallbacks, %this, %netAddress, %name);
        if (!(%message $= ""))
        {
            return %message;
        }
        Parent::onConnectRequest(%this, %netAddress, %name);
    }
    function GameConnection::onPreConnectRequest(%this, %netAddress, %name)
    {
        LiFx::executeCallback($LiFx::hooks::onPreConnectRequestCallbacks, %this, %netAddress, %name);
        Parent::onPreConnectRequest(%this, %netAddress, %name);
    }
    function GameConnection::onClientLeaveGame(%this)
    {
        LiFx::executeCallback($LiFx::hooks::onDisconnectCallbacks, %this);
        Parent::onClientLeaveGame(%this);
    }
    function GameConnection::onDeath(%this, %sourceObject, %sourceClient, %damageType, %damLoc)
    {
        Parent::onDeath(%this, %sourceObject, %sourceClient, %damageType, %damLoc);
        LiFx::debugEcho("Death" SPC %sourceObject SPC %sourceClient SPC %damageType SPC %damLoc);
        LiFx::executeCallback($LiFx::hooks::onDeathCallbacks, %sourceObject, %sourceClient, %damageType, %damLoc);
        LiFx::executeCallback($LiFx::hooks::onKillCallbacks, %sourceClient, %sourceObject, %damageType, %damLoc);
    }
    function LiFxTicks::setProcessTicks(%bool)
    {
        if (%bool)
        {
            LiFxTicks.eventID = LiFxTicks.schedule(320, "onProcessTick");
        }
        else
        {
            cancel(LiFxTicks.eventID);
        }
    }
    function LiFxTicks::onProcessTick(%this)
    {
        if ($LiFx::IsJHActive != IsJHActive())
        {
            $LiFx::IsJHActive = IsJHActive();
            if ($LiFx::IsJHActive)
            {
                LiFx::debugEcho("Calling onJHStartCallbacks");
                LiFx::executeCallback($LiFx::hooks::onJHStartCallbacks);
            }
            else
            {
                LiFx::debugEcho("Calling onJHEndCallbacks");
                LiFx::executeCallback($LiFx::hooks::onJHEndCallbacks);
            }
        }
        %this.eventID = %this.schedule(32, onProcessTick);
    }
    function LiFxDBTicks::setProcessTicks(%bool)
    {
        echo("DBTicks:" SPC %bool);
        if (%bool)
        {
            LiFxDBTicks.eventID = LiFxDBTicks.schedule(5000, onProcessTick);
        }
        else
        {
            cancel(LiFxDBTicks.eventID);
        }
    }
    function LiFxDBTicks::onProcessTick(%this)
    {
        LiFx::executeCallback($LiFx::hooks::onTick);
        %this.eventID = %this.schedule(5000, onProcessTick);
    }
    function getCRC()
    {
        echo(LiFxSHA256::hashMain("art.zip"));
    }
    function loadMods()
    {
        LiFx::ObjectsTypesDisableAutoIncrement();
        return loadModsRecursivelyInFolder("mods");
    }
    function loadModsRecursivelyInFolder(%rootFolder)
    {
        loadRecursivelyInFolder(%rootFolder, "mod.cs");
        LiFx::unpackAutoloadConfig();
        return 1;
    }
    function loadRecursivelyInFolder(%rootFolder, %pattern)
    {
        if (!((%rootFolder $= "")) && !((getSubStr(%rootFolder, strlen(%rootFolder) - 1) $= "/")))
        {
            %rootFolder = %rootFolder @ "/";
        }
        %findPattern = %rootFolder @ "*/" @ %pattern;
        %file = findFirstFileMultiExpr(%findPattern @ "dso", 1);
        while (!(%file $= ""))
        {
            %csFileName = getSubStr(%file, 0, strlen(%file) - 4);
            if (!isFile(%csFileName))
            {
                LiFx::debugEcho(%csFileName);
                exec(%csFileName);
                
                // This registers your setup method, to the framework similar to how you register callbacks otherwise inside your setup function of the package
                // It is subject to change and may later be removed for automation purposes
                //LiFx::registerCallback($LiFx::hooks::mods, setup, %csFileName);
            }
            %file = findNextFileMultiExpr(%findPattern @ "dso");
        }
        %file = findFirstFileMultiExpr(%findPattern, 1);
        while (!(%file $= ""))
        {
            LiFx::debugEcho(%file);
            exec(%file);
            %file = findNextFileMultiExpr(%findPattern);
        }
        return 1;
    }
    function LiFx::execAutoloadConfig()
    {
        exec("mods/AutoloadConfig.cs");
        echo("Loaded autoloadConfig" SPC $LiFxAutoload::LiFx);
    }
    function LiFx::processTombstone(%this, %player, %tombstone)
    {
        dbi.select(LiFx, "processPlayerDeath", "SELECT  CharID,KillerID,IsKnockout," @ %tombstone @ " as Tombstone FROM chars_deathlog WHERE CharID =" SPC %player.getCharacterId() SPC "ORDER BY ID desc LIMIT 1");
    }
    function LiFx::processPlayerDeath(%this, %rs)
    {
        if (%rs.Ok() && %rs.nextRecord())
        {
            %CharID = %rs.getFieldValue("CharID");
            %KillerID = %rs.getFieldValue("KillerID");
            %isKnockout = %rs.getFieldValue("isKnockout");
            %tombstone = %rs.getFieldValue("Tombstone");
            if (%CharID $= %KillerID)
            {
                LiFx::executeCallback($LiFx::hooks::onSuicideCallbacks, %CharID, %isKnockout, %tombstone);
            }
            else
            {
                LiFx::executeCallback($LiFx::hooks::onDeathCallbacks, %CharID, %isKnockout, %tombstone);
                if (%KillerID < 214748e+09)
                {
                    LiFx::executeCallback($LiFx::hooks::onKillCallbacks, %CharID, %KillerID, %isKnockout, %tombstone);
                }
            }
        }
        dbi.remove(%rs);
        %rs.delete();
    }
  
    function LiFx::unpackAutoloadConfig(%overwrite)
    {
        if (!isFile("mods/AutoloadConfig.cs") || %overwrite)
        {
            %zip = new ZipObject("")
            {
            };
            %zip.openArchive("art.zip", Read);
            if (%zip.extractFile("AutoloadConfig.cs", "mods/AutoloadConfig.cs"))
            {
                warn("Extracted AutoloadConfig.cs to mods folder");
                log("----------- Life is Feudal: Extended -----------");
                warn("mod/AutoloadConfig.cs has been overwritten");
                warn("- You may want to review your configuration");

                LiFx::execAutoloadConfig();
            }
            else
            {
                echo("Failed to extract AutoloadConfig.cs");
            }
        }
        else
        {
            LiFx::execAutoloadConfig();
            if($LiFx::autoLoadVersion < stripChars(getSubStr($LiFx::Version, 1,strlen($LiFx::Version) - 1),"."))
            {
              LiFx::unpackAutoloadConfig(true);
            }
        }
    }
    function LiFx::titleprompt() {
      echo("==================================================================================================================================");
      echo("");
      echo("ooooo         o8o   .o88o.                 o8o                oooooooooooo                             .o8            oooo  ");
      echo("`888'         `\"'   888 `\"                 `\"'                `888'     `8                            \"888            `888  ");
      echo(" 888         oooo  o888oo   .ooooo.       oooo   .oooo.o       888          .ooooo.  oooo  oooo   .oooo888   .oooo.    888  ");
      echo(" 888         `888   888    d88' `88b      `888  d88(  \"8       888oooo8    d88' `88b `888  `888  d88' `888  `P  )88b   888  ");
      echo(" 888          888   888    888ooo888       888  `\"Y88b.        888    \"    888ooo888  888   888  888   888   .oP\"888   888  ");
      echo(" 888       o  888   888    888    .o       888  o.  )88b       888         888    .o  888   888  888   888  d8(  888   888  ");
      echo("o888ooooood8 o888o o888o   `Y8bod8P'      o888o 8\"\"888P'      o888o        `Y8bod8P'  `V88V\"V8P' `Y8bod88P\" `Y888\"\"8o o888o ");
      echo("                                                                                                                            ");
      echo("                                                                                                                            ");
      echo("                                                                                                                            ");
      echo("               oooooooooooo                 .                               .o8                  .o8                        ");
      echo("               `888'     `8               .o8                              \"888                 \"888                        ");
      echo("                888         oooo    ooo .o888oo  .ooooo.  ooo. .oo.    .oooo888   .ooooo.   .oooo888                        ");
      echo("                888oooo8     `88b..8P'    888   d88' `88b `888P\"Y88b  d88' `888  d88' `88b d88' `888                        ");
      echo("                888    \"       Y888'      888   888ooo888  888   888  888   888  888ooo888 888   888                        ");
      echo("                888       o  .o8\"'88b     888 . 888    .o  888   888  888   888  888    .o 888   888                        ");
      echo("               o888ooooood8 o88'   888o   \"888\" `Y8bod8P' o888o o888o `Y8bod88P\" `Y8bod8P' `Y8bod88P\"                       ");
      echo("                                                                                                                            ");
      echo("                                                                                                                            ");
      echo("                                                                                                                            ");
      echo("                                                .o         .oooo.         .oooo.                                            ");
      echo("                                              .d88        d8P'`Y8b       d8P'`Y8b                                           ");
      echo("                              oooo    ooo   .d'888       888    888     888    888                                          ");
      echo("                               `88.  .8'  .d'  888       888    888     888    888                                          ");
      echo("                                `88..8'   88ooo888oo     888    888     888    888                                          ");
      echo("                                 `888'         888   .o. `88b  d88' .o. `88b  d88'                                          ");
      echo("                                  `8'         o888o  Y8P  `Y8bd8P'  Y8P  `Y8bd8P'                                           ");
      echo("                                                                                                                            ");
      echo("                                                                                                                            ");
      echo("");
      echo("8fe888bf5ef2ef61cf85b2cef010fb7c9140ce0442a573f2aef4d1d279bfd0a8");
      echo("==================================================================================================================================");
      echo("");
    }
    function onServerCreated()
    {
        deactivatePackage(LiFx);
        onServerCreated();
        activatePackage(LiFx);
        LiFx::executeCallback($LiFx::hooks::onServerCreatedCallbacks);
    }
    function destroyServer()
    {
        LiFx::executeCallback($LiFx::hooks::onDestroyServerCallbacks);
        allowConnections(0);
        if ($CmMaintenance::performAtQuit)
        {
            maintenance();
            $CmMaintenance::performAtQuit = 0;
        }
        destroyWorld();
        onServerDestroyed();
        if (isObject(ServerGroup))
        {
            ServerGroup.delete();
        }
        if (isObject(irspGroup))
        {
            irspGroup.delete();
        }
        while (ClientGroup.getCount())
        {
            %client = ClientGroup.getObject(0);
            %client.delete();
        }
        deleteDataBlocks();
    }
    function activatePackage(%package) {
        deactivatePackage( LiFx ); 
        activatePackage(%package);
        activatePackage( LiFx );
        if(isObject(%package) && %package.isMethod("setup")) {
          LiFx::registerCallback($LiFx::hooks::mods, setup, %package);
        }
    }
    function onStart()
    {
        createPath("mods/LiFx/");
        Parent::onStart();
        LiFx::executeCallback($LiFx::hooks::onStartCallbacks);
        LiFx::registerCallback($LiFx::hooks::onSpawnCallbacks, sendLiFxVersion, LiFx);
        LiFxTicks::setProcessTicks(1);
        LiFxDBTicks::setProcessTicks(1);
    }
    
    function initServer()
    {
        LiFx::titleprompt();
        if (loadMods())// Load cs files 
        {
            LiFx::debugEcho("\n Calling mods");
            LiFx::executeCallback($LiFx::hooks::mods);// Execute setup functions
            
        }
        echo("\n--------- Initializing " @ $appName @ ": Server Scripts ---------");
        initBaseServer();
        exec("scripts/server/commands.cs");
        exec("scripts/server/game.cs");
        exec("art/terrains/materials.cs");
        echo("Loading CmConfiguration");
        exec("scripts/server/cm_config.cs");
        CmConfiguration_init();
        echo("Init of DB interface");
        CmDatabase_init();
        if (!CmServerInfoManager::setLocalWorldIDToLoad($cm_config::worldID))
        {
            error("Fatal: Can\'t set world to load (id=" @ $cm_config::worldID @ ") Terminating");
            quit();
            return 0;
        }
        $cm_config::worldID = CmServerInfoManager::getWorldIdToLoad();
        if (!checkServerIdLockFile())
        {
            error("Can\'t init server... looks like another instance is already started! (id=" @ $cm_config::worldID @ "). Terminating.");
            quit();
            return;
        }
        if (!CmServerInfoManager::initLocalWorld())
        {
            error("Fatal: Can\'t init local world (id=" @ $cm_config::worldID @ ") Terminating");
            quit();
            return 0;
        }
        $Con::WindowTitle = $Con::WindowTitle @ "," SPC "world ID" SPC $cm_config::worldID;
        updateWinConsoleTitle();
        if (!CmServerInfoManager::isDedicatedServer())
        {
            startSharingServerLoadingStatus();
        }
        singleton DatabaseInterface(dbi)
        {
        };
        dbi.initialize(DBIPrimary);
        singleton DatabaseInterface(dbiInventory)
        {
        };
        dbiInventory.initialize(DBIInvLoad);
        singleton DatabaseInterface(dbiInvHelper)
        {
        };
        dbiInvHelper.initialize(dbiInvHelper);
        singleton DatabaseInterface(dbiGuilds)
        {
        };
        dbiGuilds.initialize(DBIGuildsProcess);
        U32CmDbTableIDRangeInit(U32CharacterDbIDRange, "p_issueIdRange_character", "p_occupyId_character");
        LiFx::executeCallback($LiFx::hooks::onInitServerDBChangesCallbacks);
        initPlayerSpawnPoints();
        exec("art/forest/treeDatablocks.cs");
        exec("scripts/navmesh.cs");
        initNavMeshUpdates();
        determineNetwork();
        LiFx::executeCallback($LiFx::hooks::onInitServerCallbacks);
        return 1;
    }
    function onPostInit()
    {
        Parent::onPostInit();
        LiFx::executeCallback($LiFx::hooks::onPostInitCallbacks);
        LiFx.schedule(100, "reactivate");
        activatePackage(LiFxDeath);
        if($LiFx::createDataXMLS)
        {            
          LiFx.packAndUpdateRecipe();
          LiFx.packAndUpdateRecipeRequirement();
          LiFx.packAndUpdateObjectsTypes();
        }
    }
    function LiFx::reactivate()
    {
        deactivatePackage(LiFx);
        activatePackage(LiFx);
    }
    function LiFx::sendLiFxVersion(%this, %client)
    {
        %loadedMods = "";
        %text = "<color:d8d8d8>\nLiFx Server autoload is installed on this server - " @ $LiFx::Version @ "\nLoaded mods:" SPC $LiFx::hooks::mods.Length;
        %client.cmSendClientMessage(2475, %text);
        %j = 0;
        for (%i = 0; %i < $LiFx::hooks::mods.Length; %i++)
        {
            %func = nextToken($LiFx::hooks::mods.value[%i], "name", "|");
            LiFx::debugEcho(%func SPC %name);
            %version = %name.call("version");
            LiFx::debugEcho(%version);
            %loadedMods = %loadedMods @ "\n" @ %name SPC "v" @ ((%version $= "") ? "Mod not versioned" : %version);
            LiFx::debugEcho(%loadedMods);
            if (%j == 6 || %i+1 == $LiFx::hooks::mods.Length)
            {
                %text = %loadedMods;
                %client.cmSendClientMessage(2475, %text);
                %loadedMods = "";
                %j = 0;
            }
            %j++;

        }
        %text = %loadedMods;
        %client.cmSendClientMessage(2475, %text);
        %loadedMods = "";
    }
    function LiFx::debugEcho(%message)
    {
        if ($LiFx::debug)
        {
            echo(%message);
        }
    }
    function PlayerData::onStartSwim(%this, %obj)
    {
        LiFx::executeCallback($LiFx::hooks::onStartSwimCallbacks, %this, %obj);
    }
    function PlayerData::onStopSwim(%this, %obj)
    {
        LiFx::executeCallback($LiFx::hooks::onStopSwimCallbacks, %this, %obj);
    }

    function LiFxTablePackage::addTableField(%this, %field, %ifnull)
    {
        if (!isObject(%this.fields))
        {
            %this.fields = new ArrayObject()
            {
            };
        }
        %this.fields.add(%field, %field);
        if (%ifnull)
        {
            %this.ifnull[%field] = %ifnull;
        }
    }
    function LiFx::packAndUpdateRecipe()
    {
        %obj = new ScriptObject("")
        {
            class = LiFxTablePackage;
        };
        %obj.destination = "data/recipe.xml";
        %obj.Table = "recipe";
        %obj.addTableField("ID");
        %obj.addTableField("Name");
        %obj.addTableField("Description");
        %obj.addTableField("StartingToolsID", 1);
        %obj.addTableField("SkillTypeID");
        %obj.addTableField("SkillLvl");
        %obj.addTableField("ResultObjectTypeID");
        %obj.addTableField("SkillDepends");
        %obj.addTableField("Quantity");
        %obj.addTableField("Autorepeat");
        %obj.addTableField("IsBlueprint");
        %obj.addTableField("ImagePath");
        LiFx.startPackTable(%obj);
    }
    function LiFx::packAndUpdateRecipeRequirement()
    {
        %obj = new ScriptObject("")
        {
            class = LiFxTablePackage;
        };
        %obj.destination = "data/recipe_requirement.xml";
        %obj.Table = "recipe_requirement";
        %obj.addTableField("ID");
        %obj.addTableField("RecipeID");
        %obj.addTableField("MaterialObjectTypeID");
        %obj.addTableField("Quality");
        %obj.addTableField("Influence");
        %obj.addTableField("Quantity");
        %obj.addTableField("IsRegionItemRequired");
        LiFx.startPackTable(%obj);
    }
    function LiFx::packAndUpdateObjectsTypes()
    {
        %obj = new ScriptObject("")
        {
            class = LiFxTablePackage;
        };
        %obj.destination = "data/objects_types.xml";
        %obj.Table = "objects_types";
        %obj.addTableField("ID");
        %obj.addTableField("ParentID", 1);
        %obj.addTableField("Name");
        %obj.addTableField("IsContainer");
        %obj.addTableField("IsMovableObject");
        %obj.addTableField("IsUnmovableobject");
        %obj.addTableField("IsTool");
        %obj.addTableField("IsDevice");
        %obj.addTableField("IsDoor");
        %obj.addTableField("IsPremium");
        %obj.addTableField("MaxContSize");
        %obj.addTableField("Length");
        %obj.addTableField("MaxStackSize");
        %obj.addTableField("UnitWeight");
        %obj.addTableField("BackgndImage");
        %obj.addTableField("FaceImage");
        %obj.addTableField("Description");
        %obj.relaceNulls = 1;
        LiFx.startPackTable(%obj);
    }
    function LiFx::startPackTable(%this, %obj)
    {
        %sqlQuery = "SELECT ";
        %count = %obj.fields.count();
        for (%i = 0; %i < %count; %i++)
        {
            %fieldNamefv = %obj.fields.getKey(%i);
            %fieldName = %obj.fields.getValue(%i);
            if (%obj.ifnull[%fieldName])
            {
                %sqlField = "IFNULL(" @ %fieldName @ ",0)";
            }
            else
            {
                %sqlField = %fieldName;
            }
            if (%i)
            {
                %sqlQuery = %sqlQuery SPC ",";
            }
            %sqlQuery = %sqlQuery SPC %sqlField SPC %fieldNamefv;
        }
        %sqlQuery = %sqlQuery SPC "FROM `" @ %obj.Table @ "`";
        dbi.Select(%obj, LiFxPackProcessTable, %sqlQuery);
    }
    function LiFx::packAndProcessTable(%this, %rs, %obj)
    {

       
        %cacheFile = "LiFx/dbexport/" @ %obj.destination;
        createPath(%cacheFile);
        %fs = new FileObject("")
        {
        };
        if (%fs.openForWrite(%cacheFile))
        {
            %fs.writeLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            %fs.writeLine("<table name=\"" @ %obj.Table @ "\">");
            while (%rs.nextRecord())
            {
                %fs.writeLine("\t<row>");
                %count = %rs.getFieldCount();
                %i = 0;
                while (%i < %count)
                {
                    %fieldName = %rs.getFieldName(%i);
                    if (%obj.fields.countKey(%fieldName))
                    {
                        %id = %obj.fields.getKey(%obj.fields.getIndexFromKey(%fieldName));
                        %value = %rs.getFieldValue(%fieldName);
                        if ((%value $= "NULL") && %obj.relaceNulls)
                        {
                            %fs.writeLine("\t\t<" @ %id @ " isnull=\"true\" />");
                        }
                        else
                        {
                            %fs.writeLine("\t\t<" @ %id @ ">" @ %rs.getFieldValue(%fieldName) @ "</" @ %id @ ">");
                        }
                    }
                    %i = %i + 1;
                }
                %fs.writeLine("\t</row>");
            }
            %fs.writeLine("</table>");
            %fs.close();
        }
        else
        {
            error("Failed to write content to cache directory", %cacheFile, %obj.Table, %obj.destination);
        }
        %fs.delete();
        %obj.fields.delete();
        %obj.delete();
        dbi.remove(%rs);
        %rs.delete();
    }
    function LiFxPackProcessTable(%rs, %obj)
    {
        LiFx.packAndProcessTable(%rs, %obj);
    }
    
};

package LiFxDeath
{
    function cmChildObjectsGroup::onObjectAdded(%this, %obj)
    {
        LiFx::debugEcho("cmChildObjectsGroup object added" SPC %obj.shapeName);
        if ((strpos(%obj.shapeName, "tombstone.dts") >= 0) || (strpos(%obj.shapeName, "tombstone2.dts") >= 0))
        {
            foreach ( %player in PlayerList)
            {
                LiFx::debugEcho("Vector dist:" SPC VectorDist(%obj.POSITION, %player.POSITION));
                if (VectorDist(%obj.POSITION, %player.POSITION) < 0.2)
                {
                    LiFx.schedule(1000, "processTombstone", %player, %obj);
                }
            }
        }
    }
};
activatePackage(LiFx);

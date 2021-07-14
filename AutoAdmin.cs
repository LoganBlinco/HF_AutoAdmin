using HoldfastSharedMethods;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq; -- this causes an error?????????? WTF
using UnityEngine;
using UnityEngine.UI;

// Mod by [QRR] eLF
// Mod is intended to be "good enough" to usable for linebattles but main use will be for comp league
// Mod needed to be released before closed beta gets updated since I kinda need to use it for my event since CBA admining!
// Yup parts of this will be inefficient, used er whats it called again. erm itterative?? development? 
// General "design" should be "ok-ish" -> focus on customisation by config settings not performance

public class AutoAdmin : MonoBehaviour
{

    private static int minLayer = 0;
    private static int maxLayer = 30;

    public static InputField f1MenuInputField;
    public static float currentTime = -1;
    public static float FOL_TIME_CHECK_AMOUNT = 2; // seconds


    public static int D_PLAYER_LAYER = 11;
    public static int D_ARTILLERY_LAYER = 15;

    public static float D_SAFE = 2;
    public static float D_DMG_RANGE = 5;
    public static float D_DMG_MOD = 1;
    public static int D_MIN_PLAYERS_NEEDED = 2;
    public static int D_MAX_SLAP_DMG = 33;

    //D = default
    //A = artillery
    public static float D_A_SAFE = 15;
    public static float D_A_DMG_RANGE = 25;
    public static float D_A_DMG_MOD = 1;
    public static int D_A_MIN_PLAYERS_NEEDED = 1; //for arty this would be "objects"
    public static int D_A_MAX_SLAP_DMG = 33;

    //D = default
    //S = Skirm
    public static float D_S_SAFE = 7;
    public static float D_S_DMG_RANGE = 19;
    public static float D_S_DMG_MOD = 1;
    public static int D_S_MIN_PLAYERS_NEEDED = 1; 
    public static int D_S_MAX_SLAP_DMG = 24;

    public static string MESSAGE_PREFIT = "AUTOMOD: "; // TODO: make customisatble


    

    public static Dictionary<int, playerStruct> playerIdDictionary = new Dictionary<int, playerStruct>();
    public static Dictionary<int, joinStruct> playerJoinedDictionary = new Dictionary<int, joinStruct>();


    //Maps Class -> [layers]
    // e in [layers] maps -> [safe zone, warning zone, damege mod,minimumotherPlayersNeeded,maxWarningDamege]
    //each layer then maps -> [safe zone, warning zone, damege mod,minimumotherPlayersNeeded,maxWarningDamege]
    //This system allows easy customisation with specialised classes such as artillery which can FOL near cannons

    //WHY dOnt YOU makE A liNE/SKIRm/ArTY dEfAULt AND JUsT aSSiGn EaCh Instead Of aLL thESE lInES???
    //makes it easier to change in the future without using config.

    //is  dictionary best datatype for this? Probs.
    public static Dictionary<PlayerClass, Dictionary<int, layerValues>> newClassInfomation = new Dictionary<PlayerClass, Dictionary<int, layerValues>>()
    {
        { PlayerClass.ArmyLineInfantry, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.NavalMarine, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.NavalCaptain, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.NavalSailor, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.NavalSailor2, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.ArmyInfantryOfficer, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.CoastGuard, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Carpenter, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Surgeon, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },
        //Using skirm values
        { PlayerClass.Rifleman, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_S_SAFE,D_S_DMG_RANGE,D_S_DMG_MOD,D_S_MIN_PLAYERS_NEEDED,D_S_MAX_SLAP_DMG) }} },
        //Using Skirm values
        { PlayerClass.LightInfantry, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_S_SAFE,D_S_DMG_RANGE,D_S_DMG_MOD,D_S_MIN_PLAYERS_NEEDED,D_S_MAX_SLAP_DMG) }} },

        { PlayerClass.FlagBearer, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Customs, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Drummer, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Fifer, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Guard, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Violinist, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Grenadier, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.Bagpiper, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },
        //USING CANNON VALUES AND LINE VALUES
        { PlayerClass.Cannoneer, new Dictionary<int, layerValues>(){
            {D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) },
            {D_ARTILLERY_LAYER,
                new layerValues(D_A_SAFE,D_A_DMG_RANGE,D_A_DMG_MOD,D_A_MIN_PLAYERS_NEEDED,D_A_MAX_SLAP_DMG) } } },
        //USING CANNON VALUES AND LINE VALUES
         { PlayerClass.Rocketeer, new Dictionary<int, layerValues>(){
            {D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) },
            {D_ARTILLERY_LAYER,
                new layerValues(D_A_SAFE,D_A_DMG_RANGE,D_A_DMG_MOD,D_A_MIN_PLAYERS_NEEDED,D_A_MAX_SLAP_DMG) } } },

        //USING CANNON VALUES AND LINE VALUES
         { PlayerClass.Sapper, new Dictionary<int, layerValues>(){
            {D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) },
            {D_ARTILLERY_LAYER,
                new layerValues(D_A_SAFE,D_A_DMG_RANGE,D_A_DMG_MOD,D_A_MIN_PLAYERS_NEEDED,D_A_MAX_SLAP_DMG) } } },

        { PlayerClass.Hussar, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },

        { PlayerClass.CuirassierDragoon, new Dictionary<int, layerValues>(){{D_PLAYER_LAYER,
                new layerValues(D_SAFE,D_DMG_RANGE,D_DMG_MOD,D_MIN_PLAYERS_NEEDED,D_MAX_SLAP_DMG) }} },
    };


    //METHODS
    /// <summary>
    /// Ran when the player shoots. Determines if an FOL has occured and completes punishement. 
    /// Does not consider if a kill occured.
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="dryShot"></param>
    public static void playerShotController(int playerId, bool dryShot)
    {
        if (dryShot) { return; }
        PlayerClass pClass = playerIdDictionary[playerId]._playerClass;

        //get the dictionary values for this class
        Dictionary<int,layerValues> layerInfomation = newClassInfomation[pClass];
        //if the dictionary has no layers to check -> return
        if (layerInfomation.Count == 0) { return; }

        string currentReason = "";
        int currentDamege = 250; //could use int.maxvalue -- im just worried that slap might not like a value so big? shouldnt have an issue.
        float currentD = float.MaxValue;

        //if not go through each layer and check if an FOL occured
        foreach(int layerKey in layerInfomation.Keys)
        {
            var sZone = layerInfomation[layerKey].safeZone;
            var dZone = layerInfomation[layerKey].warningZone;
            var damegeMod = layerInfomation[layerKey].damegeMod;
            int playersNeeded = layerInfomation[layerKey].minimumNumberOfPlayersNeeded;
            int maxWarningDamege = layerInfomation[layerKey].maxWarningDamege;

            var variables = newFOL_Detector(layerKey, playerId, sZone, dZone, damegeMod, playersNeeded, maxWarningDamege);
            //if any of them are valid -> FOL is valid so end.
            if (variables.Length == 0) { return; }
            string reason = (string)variables[0];
            int damege = (int)variables[1];
            float dist = (float)variables[2];
            if (damege < currentDamege)
            {
                currentDamege = damege;
                currentReason = reason;
                currentD = dist;
            }


        }
        //broadcast("damage: " + currentDamege + " , closestD: " + currentReason); //used for debugging
        broadcast(currentReason);
        slapPlayer(playerId, currentDamege, currentReason);
        //PrivateMessagePlayer(playerId, currentReason); // will use this when "rc serverAdmin privateMessage" is implemented.
        updateShotInfomation(playerId, currentD);
    }

    public static object[] newFOL_Detector(int layerToCheck, int playerId, float safe_zone, float max_warning_distance, float damege_mod, int numberOfPlayersNeeded, int maxWarningDamege)
    {
        if (numberOfPlayersNeeded <= 0 || damege_mod == 0) { return new object[] { }; }
        //more efficient init. -- could consider just storing this tbh that way its not calculated at every shot. TODO: consider doing <--
        float safeZoneSQR = safe_zone * safe_zone;
        float maxWarningDistanceSQR = max_warning_distance * max_warning_distance;

        float playersInSafeZone = 0;
        float playersInDamegeZone = 0;
        float closestDSQR = float.MaxValue;
        Vector3 playerPos = playerIdDictionary[playerId]._playerObject.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(playerPos, max_warning_distance, 1 << layerToCheck);

        foreach (var hitCollider in hitColliders)
        {
            float distSQR = Vector3.SqrMagnitude(hitCollider.gameObject.transform.position - playerIdDictionary[playerId]._playerObject.transform.position);
            if (distSQR < safeZoneSQR && distSQR != 0)
            {
                playersInSafeZone += 1;
            }
            if (distSQR >= safeZoneSQR)
            {
                playersInDamegeZone += 1;
                if (distSQR < closestDSQR)
                {
                    closestDSQR = distSQR;
                }
            }
        }
        //broadcast("distance: " + closestDSQR + " safezone dist: " + safe_zone + " , warning dist: " + max_warning_distance + "in safezone: " + playersInSafeZone + "in damge" + playersInDamegeZone);
        if (playersInSafeZone >= numberOfPlayersNeeded) { return new object[] { }; } //will always interact with themself
        //broadcast("distance: " + closestD + " safezone dist: " + safe_zone + " , warning dist: " + max_warning_distance + "in safezone: "+playersInSafeZone + "in damge" + playersInDamegeZone);

        string reason;
        int damege;


        if (playersInSafeZone + playersInDamegeZone >= numberOfPlayersNeeded) // TODO: clean this up. Not very nice. Also add some kind of option for function -- probs use easingFunction -- copy paste code from terrain generator
        {
            //apply damege scaling
            reason = MESSAGE_PREFIT + "You fired : " + closestDSQR + "m out of line. Make sure to be shoulder to shoulder when firing.";
            //floor "isnt" needed. TODO: fix + validate.
            damege = (int)Mathf.Floor(damege_mod * (1 - playersInSafeZone / numberOfPlayersNeeded) * Mathf.Max(maxWarningDamege / (maxWarningDistanceSQR - safeZoneSQR) * (closestDSQR - safeZoneSQR), 0));
            return new object[] { reason, damege, closestDSQR }; //ew
        }
        //apply max damege
        reason = MESSAGE_PREFIT + "You fired without " + numberOfPlayersNeeded + " players within a radius of " + max_warning_distance + "m. Make sure to be shoulder to shoulder with atleast " + numberOfPlayersNeeded + " other players before firing.";
        damege = (int)Mathf.Floor(damege_mod * maxWarningDamege);
        return new object[] { reason, damege, closestDSQR }; //ew
    }

    public static void PlayerJoined(int playerId, ulong steamId, string name, string regimentTag, bool isBot)
    {
        joinStruct temp = new joinStruct()
        {
            _steamId = steamId,
            _name = name,
            _regimentTag = regimentTag,
            _isBot = isBot
        };
        playerJoinedDictionary[playerId] = temp;
    }

    public static void PlayerLeft(int playerId)
    {
        playerJoinedDictionary.Remove(playerId);
        playerIdDictionary.Remove(playerId);
    }

    //TODO: dont actually use most / any of this data. Kinda pointless atm.
    public static void playerSpawned(int playerId, FactionCountry playerFaction, PlayerClass playerClass, int uniformId, GameObject playerObject)
    {
        playerStruct temp = new playerStruct()
        {
            _playerFaction = playerFaction,
            _playerClass = playerClass,
            _uniformId = uniformId,
            _playerObject = playerObject
        };
        playerIdDictionary[playerId] = temp;
    }
    /// <summary>
    /// When a player is killed, checks if it was done by an FOL
    /// If killed by FOL -> kills offender, revive victim
    /// TODO: must implement some kinda of "bool shouldRevive" variable for customisation -- THINK COMP LEAGUE
    /// </summary>
    /// <param name="killerPlayerId"></param>
    /// <param name="victimPlayerId"></param>
    /// <param name="reason"></param>
    /// <param name="additionalDetails"></param>
    public static void playerKilled(int killerPlayerId, int victimPlayerId, EntityHealthChangedReason reason, string additionalDetails)
    {
        if (reason != EntityHealthChangedReason.ShotByFirearm) { return; }

        //this should never run, but just incase 
        if (!playerIdDictionary.ContainsKey(killerPlayerId)) { return; }

        float maxTimeDiffrence = 2;


        //was it an FOL? 
        var killerInfo = playerIdDictionary[killerPlayerId].shotInfo;

        if (killerInfo._timeRemaining == 0) { return; }

        if (killerInfo._timeRemaining - currentTime > maxTimeDiffrence || killerInfo._timeRemaining == 0) { return; }

        // if so slay and revive player.
        string msgReason = MESSAGE_PREFIT + "You fired : " + killerInfo._distance + "m out of line. Make sure to be shoulder to shoulder when firing.";
        slayPlayer(killerPlayerId, msgReason);

        revivePlayer(victimPlayerId, "Killed by an FOL");
    }
    /// <summary>
    /// Revives the player.
    /// TODO: add a pm telling them
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="reason"></param>
    private static void revivePlayer(int playerID, string reason)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("serverAdmin revive {0}", playerID, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }
    /// <summary>
    /// When an FOL has occured this is ran to update the playerIdDictionary of WHEN the FOL occured.
    /// This is used for the revive/autoslay feature
    /// TODO: imeplement distance to determine slay? IDK. Make a setting.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="distance"></param>
	private static void updateShotInfomation(int playerID, float distance)
    {
        shotStruct temp = new shotStruct()
        {
            _timeRemaining = currentTime,
            _distance = distance
        };
        var change = playerIdDictionary[playerID];
        change.shotInfo = temp;
        playerIdDictionary[playerID] = change;
    }

    /// <summary>
    /// What is says on the tin.
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="reason"></param>
    private static void slayPlayer(int playerID, string reason)
    {
        if (f1MenuInputField == null) { return; }

        var rcCommand = string.Format("serverAdmin slay {0} {1}", playerID, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }
    /// <summary>
    /// Yup slaps them.
    /// TODO: add a pm. 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="damege"></param>
    /// <param name="reason"></param>
    private static void slapPlayer(int playerID, int damege,  string reason)
    {
        if (f1MenuInputField == null) { return; }
        var rcCommand = string.Format("serverAdmin slap {0} {1} {2}", playerID, damege, reason);
        f1MenuInputField.onEndEdit.Invoke(rcCommand);
    }
    /// <summary>
    /// broadcast without admin prefix added
    /// </summary>
    /// <param name="message"></param>
    public static void broadcast(string message)
    {
        if (f1MenuInputField == null) { return; }
        f1MenuInputField.onEndEdit.Invoke("broadcast "+message);
    }
    /// <summary>
    /// broadcast with prefix added
    /// </summary>
    /// <param name="message"></param>
    public static void broadcast_prefix(string message)
    {
        if (f1MenuInputField == null) { return; }
        f1MenuInputField.onEndEdit.Invoke("broadcast "+MESSAGE_PREFIT+" " + message);
    }
    /// <summary>
    /// alrigght so this is weird, 
    /// for some reason String.Contains causes an errror when building??????? WTF!
    /// TODO: look into this
    /// </summary>
    /// <param name="array"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool CustomContains(string[] array, string obj)
    {
        //oh
        foreach(var e in array)
        {
            if (e==obj)
            {
                return true;
            }
        }
        return false;
    }

    //<modid>:<class>:[playerLayer,safeZone,damgeZone,damgeMod,minPlayers,maxSlap]
    private static void ChangeClassInfomation(PlayerClass variableClass, string[] variableSplit)
    {
        int numberOfVariables = 6;
        if (variableSplit.Length != numberOfVariables) { return; }


        int layer;

        float safe = D_SAFE;
        float range = D_DMG_RANGE;
        float damegeMod = D_DMG_MOD;
        int numberOfPlayers = D_MIN_PLAYERS_NEEDED;
        int maxWarningDamege = D_MAX_SLAP_DMG;

        float safe_temp;
        float range_temp;
        float damegeMod_temp;
        int numberOfPlayers_temp;
        int maxWarningDamege_temp;
        //probably a better way to do this tbh. However Im not sure how. //TODO: <--
        if (int.TryParse(variableSplit[0], out layer))
        {
            if (!(layer >= minLayer && layer <= maxLayer))
            {
                Debug.LogError("Invalid error inputted. Layer inputted: " + layer);
            }
        }

        if (float.TryParse(variableSplit[1], out safe_temp))
        {
            if (safe_temp > 0)
            {
                //Debug.Log("safe: " + safe_temp);
                safe = safe_temp;
            }
        }
        if (float.TryParse(variableSplit[2], out range_temp))
        {
            if (range_temp > safe)
            {
                //Debug.Log("range: " + range_temp);
                range = range_temp;
            }
        }
        if (float.TryParse(variableSplit[3], out damegeMod_temp))
        {
            if (damegeMod_temp >= 0)
            {
                //Debug.Log("damegeMod: " + damegeMod_temp);
                damegeMod = damegeMod_temp;
            }
        }
        if (int.TryParse(variableSplit[4], out numberOfPlayers_temp))
        {
            if (numberOfPlayers_temp >= 0)
            {
                //Debug.Log("numPlayers: " + numberOfPlayers_temp);
                numberOfPlayers = numberOfPlayers_temp;
            }
        }
        if (int.TryParse(variableSplit[5], out maxWarningDamege_temp))
        {
            if (maxWarningDamege_temp >= 0)
            {
                //Debug.Log("maxDMG: " + maxWarningDamege_temp);
                maxWarningDamege = maxWarningDamege_temp;
            }
        }
        newClassInfomation[variableClass][layer] = new layerValues(safe, range, damegeMod, numberOfPlayers, maxWarningDamege);
        Debug.Log("class: " + variableClass + "\n safe " + safe + "\n range " + range + "\n numplayer " + numberOfPlayers);
        //AutoAdmin.classInfomation[variableClass] = new float[] { safe, range, damegeMod, numberOfPlayers, maxWarningDamege };
        //Debug.Log("done");
}


    //<modid>:<class>:[playerLayer,safeZone,damgeZone,damgeMod,minPlayers,maxSlap]
    public static void PassConfigVariables(string[] value)
    {
        string modID = "2531692643";
        for (int i = 0; i < value.Length; i++)
        {
            var splitData = value[i].Split(':');
            if (splitData.Length != 3)
            {
                Debug.LogError("invalid number of variables");
                continue;
            }

        //so first variable should be the mod id
        if (splitData[0] == modID)
        {
            //Debug.Log("correect mod ID");
            string[] classTypes = System.Enum.GetNames(typeof(PlayerClass));
            if (AutoAdmin.CustomContains(classTypes, splitData[1]))
            {
                PlayerClass variableClass = (PlayerClass)System.Enum.Parse(typeof(PlayerClass), splitData[1]);
                string[] variableSplit = splitData[2].Split(',');
                ChangeClassInfomation(variableClass, variableSplit);
            }
        }

        }
    }
	#region OUTDATED/NOT IMPLEMENTED STUFF
	/*NOT IMPLEMENDED
public static void TextCommandController(int playerId, TextChatChannel channel, string text)
{
    if (channel != TextChatChannel.Admin) { return; }

    string[] temp = text.Split(' ');
    switch(temp[0])
    {
        case "ac":
            if (temp.Length != 2) { break; }
            allCharge(temp[1]);
            break;
        default:
            break;
    }
}
*/

	/* NOT IMPLEMENDED
    private static void allCharge(string v)
    {
        broadcast("enumerator method");
        float timer;
        if (float.TryParse(v, out timer))
        {
            broadcast(timer.ToString());
            if (timer >= 0)
            {
                broadcast("current time: " + currentTime);
                float timeForAC = currentTime - timer;
                float minutes = Mathf.Floor(timeForAC / 60); //could also use int division 
                float seconds = timeForAC - 60 * minutes; //could also use mod.
                var message = MESSAGE_PREFIT + " All charge at " + minutes + ":" + seconds;
                broadcast(message);

                var acCommand = MESSAGE_PREFIT + " All charge - Cav dismount;set allowFiring false";
                var temp = new allCharge(timer, acCommand);
                //broadcast(acCommand);
            }
        }
    }
    */


	/*

    public static void PassConfigVariables(string[] value)
    {
        string modID = "2531692643";
        int numberOfVariables = 5;
        for (int i = 0; i < value.Length; i++)
        {
            var splitData = value[i].Split(':');
            if (splitData.Length != 3)
            {
                Debug.LogError("invalid number of variables");
                continue;
            }

            //so first variable should be the mod id
            if (splitData[0] == modID)
            {
                //Debug.Log("correect mod ID");
                string[] classTypes = System.Enum.GetNames(typeof(PlayerClass));
                if (!AutoAdmin.CustomContains(classTypes, splitData[1])) { continue; }
                PlayerClass variableClass = (PlayerClass)System.Enum.Parse(typeof(PlayerClass), splitData[1]);
                var variableSplit = splitData[2].Split(',');
                //should have 4 data parameters
                if (variableSplit.Length != numberOfVariables) { continue; }


                float safe = AutoAdmin.D_SAFE;
                float range = AutoAdmin.D_DMG_RANGE;
                float damegeMod = AutoAdmin.D_DMG_MOD;
                float numberOfPlayers = AutoAdmin.D_MIN_PLAYERS_NEEDED;
                float maxWarningDamege = AutoAdmin.D_MAX_SLAP_DMG;

                float safe_temp;
                float range_temp;
                float damegeMod_temp;
                float numberOfPlayers_temp;
                float maxWarningDamege_temp;

                if (float.TryParse(variableSplit[0], out safe_temp))
                {
                    if (safe_temp > 0)
                    {
                        //Debug.Log("safe: " + safe_temp);
                        safe = safe_temp;
                    }
                }
                if (float.TryParse(variableSplit[1], out range_temp))
                {
                    if (range_temp > safe)
                    {
                        //Debug.Log("range: " + range_temp);
                        range = range_temp;
                    }
                }
                if (float.TryParse(variableSplit[2], out damegeMod_temp))
                {
                    if (damegeMod_temp >= 0)
                    {
                        //Debug.Log("damegeMod: " + damegeMod_temp);
                        damegeMod = damegeMod_temp;
                    }
                }
                if (float.TryParse(variableSplit[3], out numberOfPlayers_temp))
                {
                    if (numberOfPlayers_temp >= 0)
                    {
                        //Debug.Log("numPlayers: " + numberOfPlayers_temp);
                        numberOfPlayers = numberOfPlayers_temp;
                    }
                }
                if (float.TryParse(variableSplit[4], out maxWarningDamege_temp))
                {
                    if (maxWarningDamege_temp >= 0)
                    {
                        //Debug.Log("maxDMG: " + maxWarningDamege_temp);
                        maxWarningDamege = maxWarningDamege_temp;
                    }
                }
                AutoAdmin.classInfomation[variableClass] = new float[] { safe, range, damegeMod, numberOfPlayers, maxWarningDamege };
                //Debug.Log("done");
            }
        }
    }
    */
	#endregion


	public struct playerStruct
    {
        public FactionCountry _playerFaction;
        public PlayerClass _playerClass;
        public int _uniformId;
        public GameObject _playerObject;

        public shotStruct shotInfo;
    }

    public struct shotStruct
    {
        public float _timeRemaining;
        public float _distance;
    }

    public struct joinStruct
    {
        public ulong _steamId;
        public string _name;
        public string _regimentTag;
        public bool _isBot;
    }
    public struct layerValues
    {
        public float safeZone;
        public float warningZone;
        public float damegeMod;
        public int minimumNumberOfPlayersNeeded;
        public int maxWarningDamege;

        public layerValues(float s,float w, float d,int m , int dmg)
        {
            safeZone = s;
            warningZone = w;
            damegeMod = d;
            minimumNumberOfPlayersNeeded = m;
            maxWarningDamege = dmg;
        }
    }



}

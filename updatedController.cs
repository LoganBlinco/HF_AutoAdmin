using HoldfastSharedMethods;
using UnityEngine;
using UnityEngine.UI;

public class updatedController : IHoldfastSharedMethods
{
    public bool allChargeControllerRan = false;

    public void OnIsServer(bool server)
    {
        //Code from Wrex: https://github.com/CM2Walki/HoldfastMods/blob/master/NoShoutsAllowed/NoShoutsAllowed.cs

        //Get all the canvas items in the game
        var canvases = Resources.FindObjectsOfTypeAll<Canvas>();

        for (int i = 0; i < canvases.Length; i++)
        {

            //Find the one that's called "Game Console Panel"
            if (string.Compare(canvases[i].name, "Game Console Panel", true) == 0)
            {
                //Inside this, now we need to find the input field where the player types messages.
                AutoAdmin.f1MenuInputField = canvases[i].GetComponentInChildren<InputField>(true);
                if (AutoAdmin.f1MenuInputField != null)
                {
                    Debug.Log("Found the Game Console Panel");
                }
                else
                {
                    Debug.Log("We did Not find Game Console Panel");
                }
                //break;
            }
        }
        allChargeControllerRan = false;
        AutoAdmin.InitialiseVariables();
    }

    public void OnIsClient(bool client, ulong steamId)
    {
    }


	#region Player Join/leave 
	public void OnPlayerJoined(int playerId, ulong steamId, string playerName, string regimentTag, bool isBot)
    {
        AutoAdmin.PlayerJoined(playerId, steamId, playerName, regimentTag, isBot);    }

    public void OnPlayerLeft(int playerId)
    {
        AutoAdmin.PlayerLeft(playerId);
    }

    //CONSOLE COMMANDS

    public void OnConsoleCommand(string input, string output, bool success)
    {
        Debug.LogFormat("OnConsoleCommand {0} {1} {2}", input, output, success);
    }

    public void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn)
    {
        CustomCommands.OnRCLogin(playerId, inputPassword, isLoggedIn);
        Debug.LogFormat("OnRCLogin {0} {1} {2}", playerId, inputPassword, isLoggedIn);
    }

    public void OnRCCommand(int playerId, string input, string output, bool success)
    {
        CustomCommands.OnRCCommand(playerId, input, output, success);
        Debug.LogFormat("OnRCCommand {0} {1} {2} {3}", playerId, input, output, success);

    }


    #endregion

    public void OnPlayerSpawned(int playerId, int spawnSectionId, FactionCountry playerFaction, PlayerClass playerClass, int uniformId, GameObject playerObject)
    {

        AutoAdmin.playerSpawned(playerId, playerFaction, playerClass, uniformId, playerObject);
    }

	#region Shoot or kill
	public void OnPlayerKilledPlayer(int killerPlayerId, int victimPlayerId, EntityHealthChangedReason reason, string additionalDetails)
    {
        if (!allChargeControllerRan)
        {
            AutoAdmin.AllChargeController();
            allChargeControllerRan = true;
        }

        ArtilleryChecker.playerKilled(killerPlayerId, victimPlayerId, reason, additionalDetails);
        AutoAdmin.playerKilled(killerPlayerId, victimPlayerId, reason, additionalDetails);
        AutoAdmin.AllChargePlayerKilled(victimPlayerId);
    }

    public void OnPlayerShoot(int playerId, bool dryShot)
    {
        AutoAdmin.playerShotController(playerId, dryShot);
    }
    #endregion

    public void OnUpdateTimeRemaining(float time)
    {
        if (AutoAdmin.allChargeState == AllChargeEnums.CustomSystem)
        {
            AutoAdmin.AllCharge_CustomSystemTime(time);
        }
        AutoAdmin.currentTime = time;
    }

    public void OnTextMessage(int playerId, TextChatChannel channel, string text)
    {
    }

    #region Variable passing 



    //Using Wrex standard for variable infomation
    //Wrex: For my mod i want the data variables to be configured in a:
    //<modid>:<variable>:<value> system

    //<modid>:<class name>:<safe_distance,damege_distance,damegeMod,playersNeeded,maxWarningDamege>
    public void PassConfigVariables(string[] value)
    {
        ConfigVariables.PassConfigVariables(value);
    }

    public void OnAdminPlayerAction(int playerId, int adminId, ServerAdminAction action, string reason)
    {
        if (action == ServerAdminAction.Slay)
        {
            AutoAdmin.AllChargePlayerKilled(playerId);
        }
    }
    public void OnInteractableObjectInteraction(int playerId, int interactableObjectId, GameObject interactableObject, InteractionActivationType interactionActivationType, int nextActivationStateTransitionIndex)
    {
        ArtilleryChecker.OnInteractableObjectInteraction(playerId, interactableObject, interactionActivationType, nextActivationStateTransitionIndex);    }

    public void OnDamageableObjectDamaged(GameObject damageableObject, int damageableObjectId, int shipId, int oldHp, int newHp)
    {
        ArtilleryChecker.ObjectDamaged(damageableObject, damageableObjectId, shipId, oldHp, newHp);
    }



    #endregion


    // NOT USED METHODS



    public void OnSyncValueState(int value)
    {
    }

    public void OnUpdateSyncedTime(double time)
    {
    }

    public void OnUpdateElapsedTime(float time)
    {
    }


    public void OnPlayerHurt(int playerId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
    {
    }

    public void OnScorableAction(int playerId, int score, ScorableActionType reason)
    {
    }

    public void OnRoundDetails(int roundId, string serverName, string mapName, FactionCountry attackingFaction, FactionCountry defendingFaction, GameplayMode gameplayMode, GameType gameType)
    {
    }

    public void OnPlayerBlock(int attackingPlayerId, int defendingPlayerId)
    {
    }

    public void OnPlayerMeleeStartSecondaryAttack(int playerId)
    {
    }

    public void OnPlayerWeaponSwitch(int playerId, string weapon)
    {
    }

    public void OnCapturePointCaptured(int capturePoint)
    {
    }

    public void OnCapturePointOwnerChanged(int capturePoint, FactionCountry factionCountry)
    {
    }

    public void OnCapturePointDataUpdated(int capturePoint, int defendingPlayerCount, int attackingPlayerCount)
    {
    }

    public void OnRoundEndFactionWinner(FactionCountry factionCountry, FactionRoundWinnerReason reason)
    {
    }

    public void OnRoundEndPlayerWinner(int playerId)
    {
    }

    public void OnPlayerStartCarry(int playerId, CarryableObjectType carryableObject)
    {
    }

    public void OnPlayerEndCarry(int playerId)
    {
    }

    public void OnPlayerShout(int playerId, CharacterVoicePhrase voicePhrase)
    {
    }

    public void OnEmplacementPlaced(int itemId, GameObject objectBuilt, EmplacementType emplacementType)
    {
    }

    public void OnEmplacementConstructed(int itemId)
    {
    }

    public void OnBuffStart(int playerId, BuffType buff)
    {
    }

    public void OnBuffStop(int playerId, BuffType buff)
    {
    }

    public void OnShotInfo(int playerId, int shotCount, Vector3[][] shotsPointsPositions, float[] trajectileDistances, float[] distanceFromFiringPositions, float[] horizontalDeviationAngles, float[] maxHorizontalDeviationAngles, float[] muzzleVelocities, float[] gravities, float[] damageHitBaseDamages, float[] damageRangeUnitValues, float[] damagePostTraitAndBuffValues, float[] totalDamages, Vector3[] hitPositions, Vector3[] hitDirections, int[] hitPlayerIds, int[] hitDamageableObjectIds, int[] hitShipIds, int[] hitVehicleIds)
    {
    }

    public void OnVehicleSpawned(int vehicleId, FactionCountry vehicleFaction, PlayerClass vehicleClass, GameObject vehicleObject, int ownerPlayerId)
    {
    }

    public void OnVehicleHurt(int vehicleId, byte oldHp, byte newHp, EntityHealthChangedReason reason)
    {
    }

    public void OnPlayerKilledVehicle(int killerPlayerId, int victimVehicleId, EntityHealthChangedReason reason, string details)
    {
    }

    public void OnShipSpawned(int shipId, GameObject shipObject, FactionCountry shipfaction, ShipType shipType, int shipNameId)
    {
    }

    public void OnShipDamaged(int shipId, int oldHp, int newHp)
    {
    }




}
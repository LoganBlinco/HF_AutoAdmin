using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunishmentController : MonoBehaviour
{
    public static void Punishment_slayPlayer(int killerPlayerId, string msgReason, InputField f1MenuInputField)
    {
        string message;
        switch (AutoAdmin.punishmentMode)
        {
            case PunishmentEnums.ManualReview:
                message = string.Format("Manual Review Mode on. Player {0} would be slayed with reason {1}", killerPlayerId, msgReason);
                ConsoleController.invoke("serverAdmin say " + message);
                break;
            case PunishmentEnums.WarningOnly:
                message = string.Format("Warning Only Mode on. Player {0} would be slayed with reason {1}", killerPlayerId, msgReason);
                ConsoleController.invoke("serverAdmin say " + message);
                break;
            case PunishmentEnums.Standard:
                ConsoleController.slayPlayer(killerPlayerId, msgReason, f1MenuInputField);
                break;
        }
    }

    public static void Punishment_slapPlayer(int playerId, int currentDamege, string currentReason, InputField f1MenuInputField)
    {
        switch (AutoAdmin.punishmentMode)
        {
            case PunishmentEnums.ManualReview:
                string message = string.Format("Manual Review Mode on. Player {0} would be slapped for {1} damege with reason {2}", playerId, currentDamege, currentReason);
                ConsoleController.invoke("serverAdmin say " + message);
                break;
            case PunishmentEnums.WarningOnly:
                //Pm controlled by another system -- might want to update this. TODO
                //ConsoleController.slapPlayer(playerId, currentDamege, currentReason, f1MenuInputField);
                break;
            case PunishmentEnums.Standard:
                ConsoleController.slapPlayer(playerId, currentDamege, currentReason, f1MenuInputField);
                break;
        }
    }


    public static void Punishment_revivePlayer(int victimPlayerId, string reason)
    {
        string message;
        switch(AutoAdmin.punishmentMode)
        {
            case PunishmentEnums.ManualReview:
                message = string.Format("Manual Review Mode on. Player {0} would be revived with reason {1}", victimPlayerId, reason);
                ConsoleController.invoke("serverAdmin say " + message);
                break;
            case PunishmentEnums.WarningOnly:
                message = string.Format("Manual Review Mode on. Player {0} would be revived with reason {1}", victimPlayerId, reason);
                ConsoleController.invoke("serverAdmin say " + message);
                break;
            case PunishmentEnums.Standard:
                AutoAdmin.reviveWithDelay(victimPlayerId, reason);
                break;
        }
    }


}

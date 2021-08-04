using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableAccess : MonoBehaviour
{

	private static string dMessage = "Custom Variable {0} has value {1}";

	public static Dictionary<string, Action<string[]>> getVariable = new Dictionary<string, Action<string[]>>()
	{
		{"liveTimer",get_liveTimer },
		{"ArtyOnArtyTime",get_ArtyOnArtyTime },
		{"ArtySlapDamege",get_ArtySlapDamege },
		{"allChargeState",get_allChargeState },
		{"allChargeVisableWarning",get_allChargeVisableWarning },
		{"allChargeTriggerDelay",get_allChargeTriggerDelay },
		{"allChargeTimeTrigger",get_allChargeTimeTrigger },
		{"allChargeMinPercentageAlive",get_allChargeMinPercentageAlive },
		{"minimumNumberOfPlayers",get_minimumNumberOfPlayers },
		{"isAllCharge",get_isAllCharge },
		{"allChargeActivityVal",get_allChargeActivityVal },
		{"allChargeMessage",get_allChargeMessage },
		{"numberOfPlayersAlive",get_numberOfPlayersAlive },
		{"numberOfPlayersSpawned",get_numberOfPlayersSpawned },
		{"currentTime",get_currentTime },
		{"MESSAGE_PREFIX",get_MESSAGE_PREFIX },
		{"punishmentMode",get_punishmentMode }
	};

	private static void get_punishmentMode(string[] msg)
	{
		string val =  AutoAdmin.punishmentMode.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_MESSAGE_PREFIX(string[] msg)
	{
		string val = AutoAdmin.MESSAGE_PREFIX.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_currentTime(string[] msg)
	{
		string val = AutoAdmin.currentTime.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_numberOfPlayersSpawned(string[] msg)
	{
		string val = AutoAdmin.numberOfPlayersSpawned.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_numberOfPlayersAlive(string[] msg)
	{
		string val = AutoAdmin.numberOfPlayersAlive.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_allChargeMessage(string[] msg)
	{
		string val = AutoAdmin.allChargeMessage.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_allChargeActivityVal(string[] msg)
	{
		string val = AutoAdmin.allChargeActivityVal.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_isAllCharge(string[] msg)
	{
		string val = AutoAdmin.isAllCharge.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_minimumNumberOfPlayers(string[] msg)
	{
		string val = AutoAdmin.minimumNumberOfPlayers.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_allChargeMinPercentageAlive(string[] msg)
	{
		string val = AutoAdmin.allChargeMinPercentageAlive.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_allChargeTimeTrigger(string[] msg)
	{
		string val = AutoAdmin.allChargeTimeTrigger.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_allChargeTriggerDelay(string[] msg)
	{
		string val = AutoAdmin.allChargeTriggerDelay.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_allChargeVisableWarning(string[] msg)
	{
		string val = AutoAdmin.allChargeVisableWarning.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_allChargeState(string[] msg)
	{
		string val = AutoAdmin.allChargeState.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_ArtySlapDamege(string[] msg)
	{
		string val = AutoAdmin.ArtySlapDamege.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_ArtyOnArtyTime(string[] msg)
	{
		string val = AutoAdmin.ArtyOnArtyTime.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}

	private static void get_liveTimer(string[] msg)
	{
		string val = AutoAdmin.liveTimer.ToString();
		ConsoleController.broadcast_prefix(string.Format(dMessage, msg[1], val), AutoAdmin.f1MenuInputField);
	}


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CustomCommand system.
/// 
/// Should be pretty easy to add further commands to the system using the dictionary.
/// Much better than switch statement -- ewww 
/// </summary>
public class CustomCommands : MonoBehaviour
{

	static List<int> playersLoggedIn = new List<int>();


	//When shortening commands, i wont be doing inputs true/false since that kinda destroys the purpose of the shortended command
	//rc meleeOn is faster than rc set allowFiring false
	//but rc melee true is only a "bit" faster / lots of effort

	public static Dictionary<string, Action<string[]>> customCommandDict = new Dictionary<string, Action<string[]>>()
	{
		{"delay", command_delay},
		{"ac", command_ac},
		{"cget", command_cget},
		{"cset", command_cset}, //TODO: implement
		{"ssinput", command_ssinput}, //send serverside input

		//45th stan command request
		{"hold", command_hold},
		{"resume", command_resume},

		{"melee", command_melee},
		{"dance", command_dance}, 
		{"god", command_god},
		//simple time
		{"aids", command_aids},
		{"time", command_time},
		{"tracers", command_tracers},
		//simple weather -- Mamba
		{"calm", command_weather},
		{"storm", command_weather},
		{"fog", command_weather},
		{"snowy", command_weather},
		{"calmrain", command_weather},
		{"calm2", command_weather},
		{"calm3", command_weather},
		{"calm4", command_weather},
		{"fog2", command_weather},
		{"fog3", command_weather},
	};


	#region Command helper functions

	private static void command_generalBool(string[] msg, string command, bool bDefault)
	{
		if (msg.Length < 2)
		{
			ConsoleController.invoke(command+" "+ bDefault);
			return;
		}
		bool state;
		if (bool.TryParse(msg[1], out state))
		{
			ConsoleController.invoke(command+" "+ state);
		}
	}
	//default value will be fail
	private static void command_generalFloat(string[] msg, string command)
	{
		if (msg.Length < 2) { return; }//invalid input
		float floatVal;
		if (float.TryParse(msg[1], out floatVal))
		{
			ConsoleController.invoke(command +" " + floatVal);
		}
	}
	#endregion


	#region commands using helpers

	/// <summary>
	/// rc aids -> makes fog to full. Can input true/false to enable/disable.
	/// </summary>
	/// <param name="msg"></param>
	private static void command_aids(string[] msg)
	{
		if (msg.Length < 2)
		{
			ConsoleController.invoke("nature weather fog enabled false");
			ConsoleController.invoke("nature weather fog visibility 0");
			return;
		}
		bool state;
		if (bool.TryParse(msg[1], out state))
		{
			ConsoleController.invoke("nature weather fog enabled " + state);
			ConsoleController.invoke("nature weather fog visibility 0");
		}
	}


	private static void command_tracers(string[] msg)
	{
		command_generalBool(msg, "set drawFirearmTrajectories", true);
	}

	private static void command_time(string[] msg)
	{
		command_generalFloat(msg, "nature time timeOfDay ");
	}

	private static void command_god(string[] msg)
	{
		command_generalBool(msg, "set characterGodMode", true);
	}

	public static void command_melee(string[] msg)
	{
		command_generalBool(msg, "set allowFiring", false);
	}

	private static void command_dance(string[] msg)
	{
		command_generalBool(msg, "set allowPreviewAnimations", true);
	}


	#endregion


	#region Custom commands that dont use helpers (actually require thinking -- smh)

	/// <summary>
	/// Changes the map to the weather specified. If a transition is given will apply the transition.
	/// </summary>
	/// <param name="msg"></param>
	public static void command_weather(string[] msg)
	{
		if (msg.Length < 2)//just the [weatherType]
		{
			ConsoleController.invoke("nature weather preset "+msg[0]);
			return;
		}
		float transition;
		if (float.TryParse(msg[1], out transition) && transition >= 0)
		{
			ConsoleController.invoke("nature weather preset " + msg[0]+" "+transition);
		}
	}

	/// <summary>
	/// Holds the round disabling movement, deaths and shooting
	/// </summary>
	/// <param name="msg"></param>
	private static void command_hold(string[] msg)
	{
		ConsoleController.broadcast_prefix("Round paused by Admin. Wait for further instructions. Do not fire or melee", AutoAdmin.f1MenuInputField);
		ConsoleController.invoke("set characterGodMode true");
		ConsoleController.invoke("set characterRunSpeed 0");
		ConsoleController.invoke("set characterWalkSpeed 0");
		ConsoleController.invoke("set allowFiring false");
		ConsoleController.invoke("set cannonMoverSpeedMultiplier 0");
		ConsoleController.invoke("set cannonMoverRotationMultiplier 0");
		ConsoleController.invoke("set rocketMoverSpeedMultiplier 0");
		ConsoleController.invoke("set rocketMoverRotationMultiplier 0");
		ConsoleController.invoke("vehiclesManager cappedSpeed 0");
		ConsoleController.invoke("vehiclesManager forceCappedSpeed true");
	}

	/// <summary>
	/// Resumes the round enabling movement , deaths and shooting
	/// </summary>
	/// <param name="msg"></param>
	private static void command_resume(string[] msg)
	{
		ConsoleController.broadcast_prefix("Round unpaused by Admin. Round is Live", AutoAdmin.f1MenuInputField);
		ConsoleController.invoke("set characterGodMode false");
		ConsoleController.invoke("set characterRunSpeed 1");
		ConsoleController.invoke("set characterWalkSpeed 1");
		ConsoleController.invoke("set allowFiring true");
		ConsoleController.invoke("set cannonMoverSpeedMultiplier 1");
		ConsoleController.invoke("set cannonMoverRotationMultiplier 1");
		ConsoleController.invoke("set rocketMoverSpeedMultiplier 1");
		ConsoleController.invoke("set rocketMoverRotationMultiplier 1");
		ConsoleController.invoke("vehiclesManager forceCappedSpeed false");
	}


	private static void command_ssinput(string[] msg)
	{
		//just want to run the command without the first element (rc ssinput [COMMAND])
		var temp = new List<string>(msg);
		temp.RemoveAt(0);
		ConsoleController.invoke(string.Join(" ", temp.ToArray()));
	}

	private static void command_ac(string[] inputArray)
	{
		if (inputArray.Length == 1) //just "rc ac"
		{
			//just call ac now.
			ConsoleController.broadcast_prefix(AutoAdmin.allChargeMessage, AutoAdmin.f1MenuInputField);
			ConsoleController.invoke("set allowFiring false");
			AutoAdmin.isAllCharge = true;
		}
		if (inputArray.Length == 2)
		{
			int time;
			if (int.TryParse(inputArray[1], out time) && time >= 0)
			{
				int acTime = (int)AutoAdmin.currentTime - time;

				float minutes = Mathf.Floor(acTime / 60); //could also use int division 
				float seconds = acTime - 60 * minutes; //could also use mod.

				ConsoleController.broadcast_prefix(string.Format("All charge at {0}:{1}", minutes, seconds), AutoAdmin.f1MenuInputField);
				AutoAdmin.CallAllCharge_NoWarning(acTime);
			}
		}
	}


	#endregion


	private static void command_delay(string[] msg)
	{
		//"delay [time] [command]"
		if (msg.Length < 3) { return; }

		var tempArray = new List<string>(msg);
		tempArray.RemoveAt(0); //removes "delay"
		tempArray.RemoveAt(0); //removes [time]
		int delay;
		if (int.TryParse(msg[1],out delay) && delay >= 0)
		{
			int timeForMessage = (int)AutoAdmin.currentTime - delay;
			var command = string.Join(" ", tempArray.ToArray());
			ConsoleController.invoke(string.Format("delayed {0} {1}", timeForMessage, command));
		}
		Debug.Log("Errror with delay command input");
	}
	private static void command_cget(string[] msg)
	{
		//msg.lenght will always be at least 1 , so dont worry about this erroring.
		string errorMsg = string.Format("Could not find the variable {0}. If you think this is an error contact the mod creator", msg[0]);

		//input should just be "rc cget [varname]"
		if (msg.Length != 2) 
		{
			ConsoleController.broadcast_prefix(errorMsg, AutoAdmin.f1MenuInputField);
			return;
		} 

		Action<string[]> function;
		if (VariableAccess.getVariable.TryGetValue(msg[1],out function))
		{
			//found the variable. Can output.
			function(msg);
			return;
		}
		ConsoleController.broadcast_prefix(errorMsg+" error type 2", AutoAdmin.f1MenuInputField);


		//custom get value of variable
	}
	private static void command_cset(string[] msg)
	{
		//format "cget,varname,value"
		if (msg.Length < 3) { return; }

		Action<string> function;
		if (ConfigVariables.commandDictionary.TryGetValue(msg[1],out function))
		{
			var temp = new List<string>(msg);
			temp.RemoveAt(0);
			temp.RemoveAt(0); //remove the "cset" and "variable name"
			//function(msg[2]);
			function(string.Join(" ",temp.ToArray()));
			return;
		}
		else
		{
			ConsoleController.broadcast_prefix(string.Format("Unable to find variable {0}. If you think this is an error contact {1}", msg[1], AutoAdmin.MOD_AUTHOR), AutoAdmin.f1MenuInputField);
			return;
		}
	}


	public static void OnRCCommand(int playerId, string input, string output, bool success)
	{
		if (success) { playersLoggedIn.Add(playerId); }

		if (!playersLoggedIn.Contains(playerId)) { return; }


		string[] inputArray = input.Split(' ');

		Action<string[]> function;
		if (customCommandDict.TryGetValue(inputArray[0], out function))
		{
			function(inputArray);
		}
	}

	public static void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn)
	{
		playersLoggedIn.Add(playerId);
	}
}

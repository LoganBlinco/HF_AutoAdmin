using HoldfastSharedMethods;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryChecker
{
	private static float maxTimeDiffrence = 0.1f; //make this small incase multiple arty shots from diffrent people.

	public static Dictionary<int, artyShotInfo> shotTimeStamps = new Dictionary<int, artyShotInfo>();

	private static readonly int REVIVE_DELAY = 2;

	public static void OnInteractableObjectInteraction(int playerId, GameObject interactableObject, InteractionActivationType interactionActivationType, int nextActivationStateTransitionIndex)
	{
		//one time interaction = arty being shot
		if (interactionActivationType != InteractionActivationType.OneTimeInteraction) { return; }

		if (!SpacingDetection.startsWithCheck(interactableObject.ToString())){ return; }
		//Is an arty piece

		if (AutoAdmin.currentTime > AutoAdmin.ArtyOnArtyTime)
		{
			shotTimeStamps[playerId] = new artyShotInfo(AutoAdmin.currentTime,artyShotTypes.BeforeArtyOnArty);
		}
		if (AutoAdmin.currentTime > AutoAdmin.liveTimer)
		{
			string msg = "Do not fire Artillery before Artillery is live";
			ConsoleController.slapPlayer(playerId, AutoAdmin.ArtySlapDamege, msg, AutoAdmin.f1MenuInputField);
			ConsoleController.privateMessage(playerId, msg, AutoAdmin.f1MenuInputField);
			shotTimeStamps[playerId] = new artyShotInfo(AutoAdmin.currentTime, artyShotTypes.BeforeLive);
			//an error gets thrown here. Er--- it kinda works good since prevents cannon being shot? TODO: look into this.
			return;
		}
		if (AutoAdmin.isAllCharge)
		{
			string msg = "Do not fire Artillery during All charge arty was live";
			PunishmentController.Punishment_slapPlayer(playerId, AutoAdmin.ArtySlapDamege, msg, AutoAdmin.f1MenuInputField);
			ConsoleController.privateMessage(playerId, msg, AutoAdmin.f1MenuInputField);
			shotTimeStamps[playerId] = new artyShotInfo(AutoAdmin.currentTime, artyShotTypes.DuringAllCharge);
			return;
		}
	}

	public static void playerKilled(int killerPlayerId, int victimPlayerId, EntityHealthChangedReason reason, string additionalDetails)
	{
		//need to check epxlosion radius

		if (!(reason == EntityHealthChangedReason.HitByCannonball || reason == EntityHealthChangedReason.DirectCannonballHit 
			|| reason == EntityHealthChangedReason.RocketExplosionRadius))
		{
			//player was not killed by arty.
			return;
		}
		artyShotInfo tempVal;
		if (shotTimeStamps.TryGetValue(killerPlayerId, out tempVal) && tempVal.time - AutoAdmin.currentTime < maxTimeDiffrence)
		{
			if (tempVal.type == artyShotTypes.BeforeArtyOnArty) { return; }//can ignore this case
																		   //otherwise either during all charge, or before live.
																		   //for both cases slay and revive

			string message;
			if (tempVal.type == artyShotTypes.BeforeLive)
			{
				message = "You fired artillery before live.";
			}
			else
			{
				//only othercase is during all charge
				message = "You fired artillery during all charge";
			}
			PunishmentController.Punishment_slayPlayer(killerPlayerId, message, AutoAdmin.f1MenuInputField);
			ConsoleController.revivePlayerDelayed(victimPlayerId, "You were killed illegally by artillery", REVIVE_DELAY, AutoAdmin.f1MenuInputField);
		}
	}
	public static void ObjectDamaged(GameObject damageableObject, int damageableObjectId, int shipId, int oldHp, int newHp)
	{
		if (newHp != 0) { return; }

		if (!SpacingDetection.startsWithCheck(damageableObject.name)) { return; }

		//is an arty piece and has been destroyed.

		if (AutoAdmin.currentTime < AutoAdmin.ArtyOnArtyTime) { return; } //dont care if A v A is live

		foreach(var val in shotTimeStamps)
		{
			if (val.Value.time - AutoAdmin.currentTime < maxTimeDiffrence)
			{
				//"likely" that arty on arty was caused by this player.
				//Will go to manual review
				ConsoleController.broadcast(AutoAdmin.MESSAGE_PREFIX + string.Format(" player {0} has 'probably' done early Arty on Arty. Admin check him!", val.Key),
					AutoAdmin.f1MenuInputField);
			}
		}
	}
	/// <summary>
	/// Stores infomation about the shot if it was fired beforelive/beforeAvA or during AC.
	/// Allows program to slay/revive if needed.
	/// </summary>
	public struct artyShotInfo
	{
		public float time;
		public artyShotTypes type;

		public artyShotInfo(float _time, artyShotTypes _type)
		{
			time = _time;
			type = _type;
		}
	}

	public enum artyShotTypes
	{
		BeforeLive,
		BeforeArtyOnArty,
		DuringAllCharge
	}


}

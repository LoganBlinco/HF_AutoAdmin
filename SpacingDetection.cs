using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacingDetection
{

    public static readonly string[] artilleryNames = new string[]
    {
        // Game Map variants
        "Cannon_4Pdr",
        "Cannon_6Pdr",
        "Cannon_18Pdr",
        "Cannon_24Pdr",
        "Cannon_FieldGun_9PDR",
        "Carronade",
        "CoastalCannon_36Pdr",
        "Longgun_RotatingCannonCarriage",
        "Mortar",
        "MovableCannon_FieldGun_9PDR",
        "Swivlegun",
        "Rocket_Moveable_Usable",
        "Rocket_launcher_Gunboat",

        // Mod Map variants
        "Movable_FieldGun_9PDR_Destructible",
        "Movable_RocketLauncher_Destructible",
        "4Pdr(Wheel Carriage)_Destructible",
        "4Pdr(Wheel Carriage)_Naval_Destructible",
        "4Pdr_Gunboat_Naval_Destructible",
        "6Pdr_Destructible",
        "9Pdr(Wheel Carriage)_Naval_Destructible",
        "18Pdr_Naval_Destructible",
        "24Pdr_Destructible",
        "24Pdr_Naval_Destructible",
        "36Pdr_French_Destructible",
        "36Pdr_French_Naval_Destructible",
        "FieldGun_9PDR_Destructible",
        "FieldGun_9PDR_Naval_Destructible",
        "Longgun_RotatingCannonCarriage_Destructible",
        "RocketLauncher_Gunboat_Naval_Destructible"
    };

    public static bool startsWithCheck(string name)
    {
        foreach(var element in artilleryNames)
        {
            if (name.StartsWith(element))
            {
                return true;
            }
        }
        return false;
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
        Vector3 playerPos = AutoAdmin.playerIdDictionary[playerId]._playerObject.transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(playerPos, max_warning_distance, 1 << layerToCheck);

        foreach (var hitCollider in hitColliders)
        {
            if (layerToCheck == 15)
            {
                var objName = hitCollider.transform.parent.parent.parent.parent.name; //lots of parants since the cannons/mortars/rockets are made from many collider components.
                //On arty layer, check that the name is part of the arty list. If not then we can move on.
                /*Debug.Log("Object: " + hitCollider.name + "\n" +
                    "Parant: " + hitCollider.transform.parent.name + "\n" +
                    "Grand Parant: " + hitCollider.transform.parent.parent.name + "\n" +
                    "Great Grand: "+ hitCollider.transform.parent.parent.parent.name + "\n" +
                    "Great Great Great" + hitCollider.transform.parent.parent.parent.parent.name);
                */
                //Might need more testing with mods/other maps but "should" work
                if (!startsWithCheck(objName)) { continue; }
            }


            float distSQR = Vector3.SqrMagnitude(hitCollider.gameObject.transform.position - AutoAdmin.playerIdDictionary[playerId]._playerObject.transform.position);
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
        if (playersInSafeZone >= numberOfPlayersNeeded) { return new object[] { }; }

        string reason;
        int damege;


        if (playersInSafeZone + playersInDamegeZone >= numberOfPlayersNeeded) // TODO: clean this up. Not very nice. Also add some kind of option for function -- probs use easingFunction -- copy paste code from terrain generator
        {
            //apply damege scaling
            reason = AutoAdmin.MESSAGE_PREFIX + "You fired : " + closestDSQR + "m out of line. Make sure to be shoulder to shoulder when firing.";
            //floor "isnt" needed. TODO: fix + validate.
            damege = (int)(damege_mod * (1 - playersInSafeZone / numberOfPlayersNeeded) * Mathf.Max(maxWarningDamege / (maxWarningDistanceSQR - safeZoneSQR) * (closestDSQR - safeZoneSQR), 0));
            return new object[] { reason, damege, closestDSQR }; //ew
        }
        //apply max damege
        reason = AutoAdmin.MESSAGE_PREFIX + "You fired without " + numberOfPlayersNeeded + " players within a radius of " + max_warning_distance + "m. Make sure to be shoulder to shoulder with atleast " + numberOfPlayersNeeded + " other players before firing.";
        damege = (int)(damege_mod * maxWarningDamege);
        return new object[] { reason, damege, closestDSQR }; //ew
    }
}

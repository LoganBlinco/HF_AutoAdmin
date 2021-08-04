using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct layerValues
{
    public float safeZone;
    public float warningZone;
    public float damegeMod;
    public int minimumNumberOfPlayersNeeded;
    public int maxWarningDamege;

    public layerValues(float s, float w, float d, int m, int dmg)
    {
        safeZone = s;
        warningZone = w;
        damegeMod = d;
        minimumNumberOfPlayersNeeded = m;
        maxWarningDamege = dmg;
    }
}
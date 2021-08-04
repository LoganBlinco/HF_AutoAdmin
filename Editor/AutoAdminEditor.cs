
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
// USED FOR DEBUGGING ERRORS
// MAKE SURE TO PUT IN AN "Editor" folder to prevent build crashing 
[CustomEditor(typeof(AutoAdmin))]
public class AutoAdminEditor : Editor
{


	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (GUILayout.Button("Run Method"))
		{
			ConfigVariables.PassConfigVariables(new string[] { "2531692643:ArmyLineInfantry:15,4,5,6,7,8" });
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ObstacleManager))]
public class ObstacleEditor_Inspector : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        GUILayout.Label("BLockage Map");
        //Get the target obstacle manager for the custom inspector
        ObstacleManager manager = target.GetComponent<ObstacleManager>();
        for(int j = 0; j < 10; j++)
        {
            //Horizontally allign the check boxes
            GUILayout.BeginHorizontal();
                GUILayout.Label($"{j+1}\t");
                for (int i = 0; i < 10; i++)
                {
                    if(GUILayout.Toggle(manager.getBlocked(i,j), $""))
                    {
                        manager.setBlocked(i, j, true);
                    }
                    else
                    {
                        manager.setBlocked(i, j, false);
                    }
                }
            GUILayout.EndHorizontal();
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86.Avx;

[CustomEditor(typeof(ObstacleManager))]
public class ObstacleEditor_Inspector : Editor
{

    //Create a custom inspector for editor tool
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        GUILayout.Label("BLockage Map");
        //Get the target obstacle manager for the custom inspector
        ObstacleManager manager = target.GetComponent<ObstacleManager>();
        for(int j = 9; j >= 0; j--)
        {
            //Horizontally allign the check boxes
            GUILayout.BeginHorizontal();
                GUILayout.Label($"{j+1}\t");
                for (int i = 0; i < 10; i++)
                {
                    //fetch the correct value from scriptable object and check the tick boxes
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


        //Without setting the scriptable object as dirty the data won't persists between different instances of unity editor
        //Dirty in this sense means it forces editor to exclude the object from undo redo stack and forces updation
        if (GUILayout.Button("Save Grid To Disk"))
        {
            EditorUtility.SetDirty(manager.isBlockedScriptableObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}

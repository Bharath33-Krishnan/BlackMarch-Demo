using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ObstacleManager : MonoBehaviour
{
    //scriptable object holding values of whether a cell is blocked or not
    public isBlockedArray isBlockedScriptableObject;

    //Function that toggles obstacles in grid according to scriptable object
    public void GenerateObstacles()
    {
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                //Access correct grid cell and toggle obstacle if it is blocked
                if(GridGenerator.cells[i, j] != null)
                    GridGenerator.cells[i, j].ToggleObstacle(isBlockedScriptableObject.getBlocked(i, j));
            }
        }
    }

    //Utility function to return if a cell is blocked in data of scriptable object
    public bool getBlocked(int i , int j)
    {
        if (isBlockedScriptableObject == null)
        {
            Debug.LogError("Scriptable Object is null");
            return false;
        }
        return isBlockedScriptableObject.getBlocked(i, j);
    }

    //Utility function to set a cell as blocked in data of scriptable object
    public void setBlocked(int i ,int j , bool value)
    {
        if (isBlockedScriptableObject == null)
        {
            Debug.LogError("Scriptable Object is null");
            return;
        }
        isBlockedScriptableObject.setBlocked(i, j, value);

    }
}

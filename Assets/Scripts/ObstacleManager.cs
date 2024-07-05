using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class ObstacleManager : MonoBehaviour
{
    //scriptable object holding values of whether a cell is blocked or not
    public isBlockedArray isBlockedScriptableObject;

    
    public void GenerateObstacles()
    {
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                if(GridGenerator.cells[i, j] != null)
                    GridGenerator.cells[i, j].ToggleObstacle(isBlockedScriptableObject.getBlocked(i, j));
            }
        }
    }

    public bool getBlocked(int i , int j)
    {
        if (isBlockedScriptableObject == null)
        {
            Debug.LogError("Scriptable Object is null");
            return false;
        }
        return isBlockedScriptableObject.getBlocked(i, j);
    }

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

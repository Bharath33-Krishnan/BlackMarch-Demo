using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int CellIndex;
    public bool IsObstacle { get; set; }

    //Basically the red sphere
    public GameObject ObstacleObj;

    private void Update()
    {
        if (ObstacleObj == null)
            return;

        //Set the red sphere as active if the cell is an obstacle
        if (IsObstacle)
            ObstacleObj.SetActive(true);
        else
            ObstacleObj.SetActive(false);

    }

    public void setCellIndex(Vector2Int index)
    {
        CellIndex = index;
    }

    public Vector2Int getCellIndex()
    {
        return CellIndex;
    }
}

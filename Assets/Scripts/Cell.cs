using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int CellIndex;
    public bool IsObstacle;

    public void setCellIndex(Vector2Int index)
    {
        CellIndex = index;
    }

    public Vector2Int getCellIndex()
    {
        return CellIndex;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellObj;
    public Vector2 CellOffset;

    public static Cell[,] cells = new Cell[10, 10];

    private void Awake()
    {
        GenerateGrid();
    }


    void GenerateGrid()
    {
        //Helper Function to generate the grid
        Vector3 cellSize = cellObj.transform.localScale;
        for(int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //Generating Offset for Each Cell
                Vector3 cellOffset = new Vector3(cellSize.x * i + CellOffset.x * i, 0, cellSize.z * j + CellOffset.y * j);
                GameObject cellInstance = Instantiate(cellObj, transform.position + cellOffset, Quaternion.identity);
                cellInstance.transform.parent = transform;

                //Seting the cell information
                Vector2Int cellIndex = new Vector2Int(i, j);
                cellInstance.name = cellInstance.name + $"({cellIndex.x},{cellIndex.y})";
                Cell cellInfo = cellInstance.GetComponent<Cell>();
                cells[i, j] = cellInfo;
                if (cellInfo == null)
                {
                    Debug.LogError("Cell Template is missing Cell Component");
                    return;
                }
                cellInfo.setCellIndex(cellIndex);
            }
        }
    }

    public static bool isTraversible(int i,int j)
    {
        return !cells[i,j].IsObstacle;
    }

    public static Cell getCell(Vector2Int id)
    {
        return cells[id.x, id.y];
    }
    public static Vector3 getCellPos(Vector2Int id)
    {
        return cells[id.x, id.y].transform.position;
    }
}

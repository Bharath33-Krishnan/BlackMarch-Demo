using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int CellIndex;
    public bool IsObstacle { get; set; }

    public GameObject cellGfx;
    //Basically the red sphere
    public GameObject ObstacleObj;

    MeshRenderer myCellMaterial;

    private void Start()
    {
    }

    private void Update()
    {

    }

    public void ToggleObstacle(bool obstacle)
    {
        IsObstacle = obstacle;
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

    //Utility function to toggle border color of cell
    public void ToggleBorder(bool val)
    {
        //This functions depends on the CellMat shader
        if(!myCellMaterial)
            myCellMaterial = cellGfx.GetComponent<MeshRenderer>();
        if (val)
            myCellMaterial.material.SetInt("_CellBorder", 1);
        else
            myCellMaterial.material.SetInt("_CellBorder", 0);
    }
}

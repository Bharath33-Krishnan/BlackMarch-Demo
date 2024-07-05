using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RayCastManager: MonoBehaviour
{
    public Cell selected_cell { get; private set; }
    public Canvas CellUICanvas;

    TMP_Text CellUI_Text;
    Camera cam;

    void Start()
    {
        selected_cell = null;
        cam = Camera.main;

        if(CellUICanvas != null)
            CellUI_Text = CellUICanvas.GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        RaycastHover();
        ShowSelectedCellUI();
    }

    //Helper Function to raycast grid cells
    void RaycastHover()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.TryGetComponent<Cell>(out Cell mouseHitCell))
            {
                selected_cell = mouseHitCell;
                //Set the UI textstring
                if(CellUI_Text != null)
                    CellUI_Text.text = mouseHitCell.getCellIndex().ToString();
                return;
            }
        }
        selected_cell = null;
    }

    //Helper Function to show UI for selected Cell
    void ShowSelectedCellUI()
    {
        if (CellUICanvas == null)
            return;
        if (selected_cell == null)
        {
            CellUICanvas.enabled = false;
            return;
        }
        CellUICanvas.enabled = true;
        CellUICanvas.transform.position = selected_cell.transform.position + .65f*Vector3.up;
    }

            
}


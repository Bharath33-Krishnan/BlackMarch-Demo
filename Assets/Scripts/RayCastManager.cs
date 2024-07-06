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
            //Raycast and try to get cell component from the hit
            if (hit.transform.TryGetComponent<Cell>(out Cell mouseHitCell))
            {
                //Toggles Border Color of older selected cell
                if(selected_cell!=null)
                    selected_cell.ToggleBorder(false);

                selected_cell = mouseHitCell;

                //Toggles Border Color of older selected cell
                if(selected_cell!=null)
                    selected_cell.ToggleBorder(true);
                //Set the UI textstring
                if(CellUI_Text != null)
                    CellUI_Text.text = $" {mouseHitCell.getCellIndex().x} , {mouseHitCell.getCellIndex().y} ";
                return;
            }
        }

        //The code will reach here if not hit object with Cell Component was reached
        //So toggle the older cell back to normal color and set selected cell as null
        if (selected_cell != null)
            selected_cell.ToggleBorder(false);
        selected_cell = null;
    }

    //Helper Function to show UI for selected Cell if it exists
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
    }

            
}


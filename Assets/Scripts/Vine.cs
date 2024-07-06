using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    [Header("Vine Properties")]
    [SerializeField] GameObject vineGfx;
    [SerializeField] float GfxEndPos;
    [SerializeField] float vineSpeed = 2;

    //Function to animate the vine growing
    private void Update()
    {
        if(vineGfx.transform.position.y < GfxEndPos)
        {
            vineGfx.transform.position += vineGfx.transform.up * vineSpeed * Time.deltaTime; 
        }
    }


}

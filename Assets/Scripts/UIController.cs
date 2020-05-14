using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Canvas mainCanvas;
    public Canvas pauseCanvas;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (mainCanvas.enabled ||pauseCanvas.enabled)
        {
            if (Input.anyKey)
            {
                return;
            }      
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTextController : MonoBehaviour
{

    Canvas introCanvas;

    // Start is called before the first frame update
    void Start()
    {

        introCanvas = GetComponent<Canvas>();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey)
        {
            introCanvas.enabled = false;
            Time.timeScale = 1;
        }
        
    }
}

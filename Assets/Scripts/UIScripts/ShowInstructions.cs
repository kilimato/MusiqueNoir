using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowInstructions : MonoBehaviour
{
    public GameObject instructionsText;
    bool instructionsVisible;
    // Start is called before the first frame update
    void Start()
    {
        instructionsText = GameObject.Find("InstructionsText");
        instructionsVisible = false;
        instructionsText.SetActive(instructionsVisible);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            instructionsVisible = !instructionsVisible;

            instructionsText.SetActive(instructionsVisible);
        }
    }
}

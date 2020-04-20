using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleControls : MonoBehaviour
{

    public TextMeshProUGUI controlsText;
    private bool toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowHideControls()
    {
        toggle = !toggle;
        controlsText.enabled = toggle;
    }
}

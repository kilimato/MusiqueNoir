using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAboveObject : MonoBehaviour
{
    public TextMeshProUGUI textLabel;
    // Update is called once per frame
    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);
        textLabel.transform.position = namePos;
    }
}

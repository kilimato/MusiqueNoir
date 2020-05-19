// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// sets text above an object, so the text is in a world position instead of a screen position
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
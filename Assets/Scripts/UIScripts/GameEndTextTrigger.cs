using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndTextTrigger : MonoBehaviour
{
    public TextMeshProUGUI endText;
    // Start is called before the first frame update
    void Start()
    {
        endText.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            endText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            endText.enabled = false;
        }
    }
}

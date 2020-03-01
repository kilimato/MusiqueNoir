using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject mainCircle;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("PlayerCircle") && mainCircle.transform.localScale.x > 0.08)
        {
            Debug.Log("Player hit the door!");
            gameObject.SetActive(false);
        }
    }
}

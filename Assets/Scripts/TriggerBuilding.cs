using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBuilding : MonoBehaviour
{
    public SpriteRenderer interior;
    // Start is called before the first frame update
    private void Start()
    {
        interior.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //when player collides with this trigger, inside of the building is revealed
            interior.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //when player exit colliding with this trigger, inside of the building is hidden
            interior.enabled = false;
        }

    }


}

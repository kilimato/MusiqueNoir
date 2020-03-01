using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : MonoBehaviour
{
    public BoxCollider2D player;
    public bool canClimb = false;
    // Start is called before the first frame update

    // player climbs the stairs when colliding with them
    // Sent each frame where a collider on another object is touching this object's collider (2D physics only).
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("entered ladder: " + other.ToString());
        if (other.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Platform").GetComponent<BoxCollider2D>(), player, true);
            canClimb = true;
        }
    }

    // Sent when a collider on another object stops touching this object's collider (2D physics only).
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exited ladder: " + other.ToString());
        if (other.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Platform").GetComponent<BoxCollider2D>(), player, false);
            canClimb = false;
        }
    }

    public bool CanPlayerClimb()
    {
        return canClimb;
    }
}

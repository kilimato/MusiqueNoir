using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private BoxCollider2D doorCollider;
    private bool canEnter = false;

    public GameObject outsideTilemaps;
    public GameObject insideTilemaps;

    // Start is called before the first frame update
    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canEnter && Input.GetKeyDown(KeyCode.E))
        {
            EnterBuilding();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canEnter = !canEnter;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canEnter = false;
        }
    }


    private void EnterBuilding()
    {
        outsideTilemaps.SetActive(false);
        insideTilemaps.SetActive(true);
        canEnter = false;
    }
}

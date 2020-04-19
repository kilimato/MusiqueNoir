using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private BoxCollider2D doorCollider;
    public bool canEnter = true;

    public GameObject insideTilemaps;
    public GameObject buildingExterior;

    // Start is called before the first frame update
    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {/*
        if (canEnter && insideTilemaps.activeSelf == false && Input.GetKeyDown(KeyCode.E))
        {
            canEnter = false;
            EnterBuilding();
        }

        if (!canEnter && insideTilemaps.activeSelf == true && Input.GetKeyDown(KeyCode.E))
        {
            canEnter = true;
            ExitBuilding();
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && canEnter)
        {
            buildingExterior.SetActive(false);
            insideTilemaps.SetActive(true);
            canEnter = !canEnter;
        }
        else if (other.gameObject.tag == "Player" && !canEnter)
        {
            buildingExterior.SetActive(true);
            insideTilemaps.SetActive(false);
            canEnter = !canEnter;
        }
    }
    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canEnter = false;
        }
    }


    private void EnterBuilding()
    {
        buildingExterior.SetActive(false);
        insideTilemaps.SetActive(true);
    }

    private void ExitBuilding()
    {
        buildingExterior.SetActive(true);
        insideTilemaps.SetActive(false);
    }
    */
}


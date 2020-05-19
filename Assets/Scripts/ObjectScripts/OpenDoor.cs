// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles opening doors and updating tilemaps accordingly
public class OpenDoor : MonoBehaviour
{
    public bool canEnter = false;
    public GameObject insideTilemaps;
    public GameObject buildingExterior;

    [FMODUnity.EventRef]
    public string openDoorEvent = "";

    // Update is called once per frame
    void Update()
    {
        if (canEnter && Input.GetKeyDown(KeyCode.E))
        {
            ChangeBetweenTilemaps();
            FMODUnity.RuntimeManager.PlayOneShot(openDoorEvent);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canEnter = false;
        }
    }

    private void ChangeBetweenTilemaps()
    {
        buildingExterior.SetActive(!buildingExterior.activeSelf);
        insideTilemaps.SetActive(!insideTilemaps.activeSelf);
    }
}
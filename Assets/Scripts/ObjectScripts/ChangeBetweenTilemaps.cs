// @author Tapio Mylläri
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates a tilemap and deactivates the other tilemap. Can be used for other gameobjects as well.
/// </summary>
public class ChangeBetweenTilemaps : MonoBehaviour
{
    public GameObject insideTilemaps;
    public GameObject buildingExterior;
    public bool activateInterior = true;  //if true, then activates insideTilemaps, else it activates buildingExterior.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ChangeTilemaps();
        }
    }

    private void ChangeTilemaps()
    {
        insideTilemaps.SetActive(activateInterior);
        buildingExterior.SetActive(!activateInterior);
    }
}
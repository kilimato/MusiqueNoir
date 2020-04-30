using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBetweenTilemaps : MonoBehaviour
{

    public GameObject insideTilemaps;
    public GameObject buildingExterior;
    public bool activateInterior = true;

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
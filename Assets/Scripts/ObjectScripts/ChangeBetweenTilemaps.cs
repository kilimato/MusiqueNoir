using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBetweenTilemaps : MonoBehaviour
{
    private BoxCollider2D doorCollider;

    public GameObject insideTilemaps;
    public GameObject buildingExterior;

    [SerializeField]
    private bool activeInterior = true;
    [SerializeField]
    private bool activeExterior = false;

    public ElevatorMovement elevatorMovement;

    // Start is called before the first frame update
    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && activeExterior && !elevatorMovement.IsMoving())
        {
            ChangeTilemaps();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"  && activeInterior && !elevatorMovement.IsMoving())
        {
            ChangeTilemaps();
        }
    }


    private void ChangeTilemaps()
    {
        activeInterior = !activeInterior;
        activeExterior = !activeExterior;

        buildingExterior.SetActive(activeExterior);
        insideTilemaps.SetActive(activeInterior);
    }
}
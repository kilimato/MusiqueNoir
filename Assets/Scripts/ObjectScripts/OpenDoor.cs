using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private BoxCollider2D doorCollider;
    private EdgeCollider2D edgeCollider;
    public bool canEnter = false;
    public bool touchedEdgeCollider = false;

    public GameObject insideTilemaps;
    public GameObject buildingExterior;

    [SerializeField]
    private bool activeInterior = false;
    [SerializeField]
    private bool activeExterior = true;

    // Start is called before the first frame update
    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canEnter && Input.GetKeyDown(KeyCode.E))
        {
            ChangeBetweenTilemaps();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.IsTouching(edgeCollider))
            {
                if (insideTilemaps.activeSelf == true)
                {
                    ChangeBetweenTilemaps();
                }
            }
            else
            {
                canEnter = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (true)
        {

        }
        if (other.gameObject.tag == "Player")
        {
            canEnter = false;
        }
    }


    private void ChangeBetweenTilemaps()
    {
        activeInterior = !activeInterior;
        activeExterior = !activeExterior;

        buildingExterior.SetActive(activeExterior);
        insideTilemaps.SetActive(activeInterior);
    }
}
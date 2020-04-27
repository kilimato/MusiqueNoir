using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Handles that checkpoint is updated to manager when player collides with it
public class Checkpoint : MonoBehaviour
{
    private GameManager manager;

    public Light2D checkpointLight;
    public Color32 checpointColor;

    public GameObject insideTilemaps;
    public GameObject buildingExterior;

    bool insideTilemapVisible;
    bool exteriorTilemapVisible;

    void Start()
    {
        //manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //checkpointLight = GetComponent<Light2D>();
    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            manager.lastCheckPointPos = transform.position;
            if (gameObject.activeSelf)
            {
                insideTilemapVisible = insideTilemaps.activeSelf;
                exteriorTilemapVisible = buildingExterior.activeSelf;
            }
            checkpointLight.color = checpointColor;
        }
    }
    */
    public bool IsInteriorVisible()
    {
        return insideTilemapVisible;
    }

    public bool IsExteriorVisible()
    {
        return exteriorTilemapVisible;
    }
}

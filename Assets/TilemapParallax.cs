using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class TileMapParallax : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.3f;
    [SerializeField] float offset = 0f; //for multiple backgrounds
    [SerializeField] GameObject viewTarget; //camera
    [SerializeField] bool xOnly = true;

    Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        float newXPos = viewTarget.transform.position.x * scrollSpeed + offset;
        float newYPos = viewTarget.transform.position.y * scrollSpeed + offset;
        
        //check if parallax should be x only
        if(xOnly)
        {
            tilemap.transform.position = new Vector3(newXPos, tilemap.transform.position.y, tilemap.transform.position.z);
        }
        else
        {
            tilemap.transform.position = new Vector3(newXPos, newYPos, tilemap.transform.position.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBetweenTilemaps : MonoBehaviour
{
    private BoxCollider2D roofCollider;
    public bool isInside = true;

    public BoxCollider2D boxCollider;

    public GameObject outsideTilemaps;
    public GameObject insideTilemaps;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // eli: kun pelaaja tulee sisältä ulos (muita vaihtoehtoja ei ole) ontriggerExit
    // 

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && isInside)
        {
            outsideTilemaps.SetActive(true);
            insideTilemaps.SetActive(false);
            isInside = !isInside;
            boxCollider.enabled = true;
        }
        
        else if (other.gameObject.tag == "Player" && !isInside)
        {
           // isInside = !isInside;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && isInside)
        {
            boxCollider.enabled = false;
        }
        else if (other.gameObject.tag == "Player" && !isInside)
        {
            outsideTilemaps.SetActive(false);
            insideTilemaps.SetActive(true);
            boxCollider.enabled = false;
        }
    }
}

// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for updating shader material with outline when player enters the trigger area
public class OutlineController : MonoBehaviour
{
    private Material mat;
    private BoxCollider2D boxCollider;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        boxCollider = GetComponent<BoxCollider2D>();
        mat.SetFloat("_OutlineThickness", 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.IsTouching(boxCollider))
        {
            if (CompareTag("Door"))
            {
                mat.SetFloat("_OutlineThickness", 2f);
            }
            else
            {
                mat.SetFloat("_OutlineThickness", 0.5f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            mat.SetFloat("_OutlineThickness", 0);
        }
    }
}

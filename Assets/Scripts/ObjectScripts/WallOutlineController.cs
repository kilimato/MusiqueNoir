using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOutlineController : MonoBehaviour
{
    private Material mat;
    private CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();

        mat = GetComponent<Renderer>().material;
        mat.SetFloat("_OutlineThickness", 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            mat.SetFloat("_OutlineThickness", 1.2f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //StopCoroutine(UpdateOutline());
            mat.SetFloat("_OutlineThickness", 0);
        }
    }
}

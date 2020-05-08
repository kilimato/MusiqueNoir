using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Material mat;
    private BoxCollider2D boxCollider;
    //private float maxThickness = 0.5f;
    //private float currentThickness= 0.1f;
    //private float amount = 0.05f;
    //private float waitTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        boxCollider = GetComponent<BoxCollider2D>();
        mat.SetFloat("_OutlineThickness", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.IsTouching(boxCollider))
        {
            if (CompareTag("Door"))
            {
                mat.SetFloat("_OutlineThickness", 1f);
            }
            else
            {
                // StartCoroutine(UpdateOutline());
                mat.SetFloat("_OutlineThickness", 0.5f);
            }
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
    /*
    IEnumerator UpdateOutline()
    {
        // eli eka kasvaa tiettyyn pisteeseen ja pienenee tiettyyn pisteeseen
        // alkaa nollasta -> kunnes on 0.5f -> lähtee pienenemään -> kunnes on 0.01f > alkaa nousta -> jne.

        float timer = 0;

        while (true) // this could also be a condition indicating "alive or dead"
        {
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (maxThickness > currentThickness)
            {
                timer += Time.deltaTime;
                currentThickness += amount;
                mat.SetFloat("_OutlineThickness", currentThickness);
                yield return null;
            }
            // reset the timer

            yield return new WaitForSeconds(waitTime);

            timer = 0;
            while (0.1f < currentThickness)
            {
                timer += Time.deltaTime;
                currentThickness -= amount;
                mat.SetFloat("_OutlineThickness", currentThickness);
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds(waitTime);
        }
    }
    */
}

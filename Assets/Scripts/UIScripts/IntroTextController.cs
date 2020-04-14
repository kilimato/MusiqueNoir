using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class IntroTextController : MonoBehaviour
{

    Canvas introCanvas;
    RawImage fadeImage;
    Text introText;

    // Start is called before the first frame update
    void Start()
    {

        introCanvas = GetComponent<Canvas>();
        fadeImage = GetComponentInChildren<RawImage>();
        introText = GetComponentInChildren<Text>();
        //Time.timeScale = 0;
        StartCoroutine(FadeIn(introText));

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey && introText.color.a >= 0.9f && introCanvas.enabled == true)
        {
            StartCoroutine(FadeOut(fadeImage));
            StartCoroutine(FadeOut(introText));
            //introCanvas.enabled = false;
            Time.timeScale = 1;
        }

    }

    IEnumerator FadeOut(Graphic g)
    {

        Color tempColor = g.color;

        while (tempColor.a > 0)
        {
            tempColor.a -= 0.01f;
            g.color = tempColor;
            yield return null;

        }

    }

    IEnumerator FadeIn(Graphic g)
    {

        Color tempColor = g.color;

        while (tempColor.a < 1)
        {
            tempColor.a += 0.01f;
            g.color = tempColor;
            yield return null;

        }

    }


}

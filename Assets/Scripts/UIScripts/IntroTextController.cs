// @author Eeva Tolonen & Tapio Mylläri
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class IntroTextController : MonoBehaviour
{
    Canvas introCanvas;
    RawImage fadeImage;
    TextMeshProUGUI introText;
    public bool isAlreadySeen = false;

    // Start is called before the first frame update
    public void Start()
    {
        introCanvas = GetComponent<Canvas>();
        if (isAlreadySeen)
        {
            introCanvas.enabled = false;
            enabled = false;
            return;
        }
        fadeImage = GetComponentInChildren<RawImage>();
        introText = GetComponentInChildren<TextMeshProUGUI>();
        Time.timeScale = 0;
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
            isAlreadySeen = true;
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

    public void ShowIntroText()
    {
        if (!isAlreadySeen)
        {
            introCanvas.enabled = true;
            fadeImage = GetComponentInChildren<RawImage>();
            Color imageColor = fadeImage.color;
            fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1); 

            introText = GetComponentInChildren<TextMeshProUGUI>();
            Time.timeScale = 0;
            StartCoroutine(FadeIn(introText));
            //StartCoroutine(FadeIn(fadeImage));
            isAlreadySeen = !isAlreadySeen;
        }
    }
}

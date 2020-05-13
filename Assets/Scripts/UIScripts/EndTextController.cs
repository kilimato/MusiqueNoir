using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTextController : MonoBehaviour
{
    Canvas endCanvas;
    public GameObject fadeImage;
    public GameObject fadeText;
    public Canvas mainCanvas;
    // Start is called before the first frame update
    void Start()
    {
        endCanvas = GetComponent<Canvas>();
    }

    public void ShowEndText()
    {
        endCanvas.enabled = true;
        StartCoroutine(FadeToBlack());
    }

    public void Update()
    {

        /*
        if (Input.GetKey(KeyCode.Q))
        {
            StartCoroutine(FadeToBlack());
        }
        */
        if (Input.GetKey(KeyCode.Space) && endCanvas.enabled)
        {
            StartCoroutine(FadeToBlack(false));
            mainCanvas.enabled = true;
            endCanvas.enabled = false;
            Time.timeScale = 0;
        }

    }

    public IEnumerator FadeToBlack(bool fadeToBlack = true, int fadeSpeed = 2)
    {

        Color imageColor = fadeImage.GetComponent<Image>().color;
        Color textColor = fadeText.GetComponent<TextMeshProUGUI>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (fadeImage.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = imageColor.a + (fadeSpeed * Time.deltaTime);

                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, fadeAmount);
                textColor = new Color(textColor.r, textColor.g, textColor.b, fadeAmount);
                fadeImage.GetComponent<Image>().color = imageColor;
                fadeText.GetComponent<TextMeshProUGUI>().color = textColor;
                yield return null;
            }
        }
        else
        {
            while (fadeImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = imageColor.a - (fadeSpeed * Time.deltaTime);

                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, fadeAmount);
                textColor = new Color(textColor.r, textColor.g, textColor.b, fadeAmount);
                fadeImage.GetComponent<Image>().color = imageColor;
                fadeText.GetComponent<TextMeshProUGUI>().color = textColor;
                yield return null;
            }
        }
    }
}

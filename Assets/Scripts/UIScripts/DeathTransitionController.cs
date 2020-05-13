using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathTransitionController : MonoBehaviour
{
    Canvas endCanvas;
    public GameObject fadeImage;
    public EndTextController endTextController;
    GameObject manager;
    public GameObject player;

    // Update is called once per frame
    private void Start()
    {
        endCanvas = GetComponent<Canvas>();

        manager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void DeathTransitionToBlack()
    {
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Animator>().SetFloat("Speed", 0.0f);
        endCanvas.enabled = true;
        endTextController.enabled = false;
        StartCoroutine(FadeLoadAndFadeAgain());
    }

    public IEnumerator FadeToBlack(bool fadeToBlack = true, int fadeSpeed = 2)
    {
        Color imageColor = fadeImage.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (fadeImage.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = imageColor.a + (fadeSpeed * Time.deltaTime);

                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, fadeAmount);
                fadeImage.GetComponent<Image>().color = imageColor;
                yield return null;
            }
        }
        else
        {
            while (fadeImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = imageColor.a - (fadeSpeed * Time.deltaTime);

                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, fadeAmount);
                fadeImage.GetComponent<Image>().color = imageColor;
                yield return null;
            }
        }
    }

    public IEnumerator FadeLoadAndFadeAgain()
    {
        yield return StartCoroutine(FadeToBlack());
        manager.GetComponent<GameManager>().LoadGame(gameObject);
        StartCoroutine(FadeAgain());
    }

    public IEnumerator FadeAgain()
    {
        yield return StartCoroutine(FadeToBlack(false));

        endTextController.enabled = true;
        endCanvas.enabled = false;
        player.GetComponent<PlayerController>().enabled = true;
    }
}

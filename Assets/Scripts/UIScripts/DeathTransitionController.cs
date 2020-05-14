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
    public GameObject resonator;

    // Update is called once per frame
    private void Start()
    {
        endCanvas = GetComponent<Canvas>();

        manager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void DeathTransitionToBlack(GameObject caller)
    {
        player.GetComponent<PlayerController>().enabled = false;
        resonator.GetComponent<ParticleScript>().ringSize = resonator.GetComponent<ParticleScript>().ringMinSize;
        resonator.SetActive(false);
        player.GetComponent<Animator>().SetFloat("Speed", 0.0f);
        endCanvas.enabled = true;
        endTextController.enabled = false;
        StartCoroutine(FadeLoadAndFadeAgain(caller));
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

    public IEnumerator FadeLoadAndFadeAgain(GameObject caller)
    {
        yield return StartCoroutine(FadeToBlack());
        Debug.Log("Caller of loadGame() is: " + gameObject);
        manager.GetComponent<GameManager>().LoadGame(caller);
        StartCoroutine(FadeAgain());
    }

    public IEnumerator FadeAgain()
    {
        yield return StartCoroutine(FadeToBlack(false));

        endTextController.enabled = true;
        endCanvas.enabled = false;
        player.GetComponent<PlayerController>().enabled = true;
        resonator.SetActive(true);
    }
}

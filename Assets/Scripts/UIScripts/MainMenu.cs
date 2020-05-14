using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }
    /*
    public void PlayGame()
    {
        Time.timeScale = 1;
        //SceneManager.LoadScene(0);
        canvas.enabled = false;
    }
    */
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    

    private void Update()
    {
        if (Input.anyKey)
        {
            return;
        }
    }
}

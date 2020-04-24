using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.enabled = !canvas.enabled;
            Time.timeScale = canvas.enabled ? 0 : 1;
        }
    }

    public void ResumeGame()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        // start menu is always the first scene to be loaded -> always 0
        SceneManager.LoadScene(0);
    }
}

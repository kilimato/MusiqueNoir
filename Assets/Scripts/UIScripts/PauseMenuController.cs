// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// handles pausing the game and setting timescale to 0 when paused
public class PauseMenuController : MonoBehaviour
{
    public Canvas mainCanvas;
    public GameObject pauseMenu;
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mainCanvas.enabled && pauseMenu.activeSelf)
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
}
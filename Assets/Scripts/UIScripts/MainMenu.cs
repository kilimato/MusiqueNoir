// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles just quitting the game
public class MainMenu : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

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
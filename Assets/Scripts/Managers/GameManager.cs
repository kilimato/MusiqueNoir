using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager that manages the state of our game
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Vector2 lastCheckPointPos = Vector2.zero;
    private Transform startingPoint;

    public GameObject fow;
    private bool isActive = true;

    public GameObject insideTilemaps;
    public GameObject outsideTilemaps;

    private void Awake()
    {
        startingPoint = GameObject.FindGameObjectWithTag("StartingPoint").transform;
        if (lastCheckPointPos == Vector2.zero) lastCheckPointPos = startingPoint.position;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);

    }

    private void Start()
    {
        outsideTilemaps.SetActive(true);
        insideTilemaps.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            isActive = !isActive;
            fow.SetActive(isActive);
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

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

    public GameObject[] checkpoints;

    public GameObject fow;
    private bool isActive = true;

    public GameObject insideTilemaps;
    public GameObject adBuildingTilemaps;

    //public BoxCollider2D checkpoint1, checkpoint2;

    private void Awake()
    {
        insideTilemaps = GameObject.Find("InsideTilemaps");
        adBuildingTilemaps = GameObject.Find("AdministrativeBuilding");
        //checkpoints = 
        startingPoint = GameObject.FindGameObjectWithTag("StartingPoint").transform;
        if (lastCheckPointPos == Vector2.zero) lastCheckPointPos = startingPoint.position;

        if (lastCheckPointPos != Vector2.zero)
        {
            insideTilemaps.SetActive(true);
            adBuildingTilemaps.SetActive(false);
            /*
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (new Vector2(checkpoints[i].transform.position.x, checkpoints[i].transform.position.y) == lastCheckPointPos)
                {
                    adBuildingTilemaps.SetActive(checkpoints[i].GetComponent<Checkpoint>().IsExteriorVisible());
                    insideTilemaps.SetActive(checkpoints[i].GetComponent<Checkpoint>().IsInteriorVisible());
                }
            }
           
         */
        }
        else
        {
            insideTilemaps.SetActive(false);
            adBuildingTilemaps.SetActive(true);
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);

    }

    private void Start()
    {
        if (lastCheckPointPos != Vector2.zero)
        {
            insideTilemaps.SetActive(true);
            adBuildingTilemaps.SetActive(false);
            /*
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (new Vector2(checkpoints[i].transform.position.x, checkpoints[i].transform.position.y) == lastCheckPointPos)
                {
                    adBuildingTilemaps.SetActive(checkpoints[i].GetComponent<Checkpoint>().IsExteriorVisible());
                    insideTilemaps.SetActive(checkpoints[i].GetComponent<Checkpoint>().IsInteriorVisible());
                }
            }
           
         */
        }
        else
        {
            insideTilemaps.SetActive(false);
            adBuildingTilemaps.SetActive(true);
        }
        
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

// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Handles that checkpoint is updated to game manager when player collides with it
public class Checkpoint : MonoBehaviour
{
    private GameManager manager;

    public Light2D checkpointLight;
    public Color32 checkedColor;
    public Color32 inactiveColor;

    public List<GameObject> checkpoints;
    private GameObject[] points;
    public int orderNumber;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        checkpoints.Add(GameObject.FindGameObjectWithTag("StartingPoint"));
        points = (GameObject.FindGameObjectsWithTag("Checkpoint"));
        foreach (GameObject point in points)
        {
            checkpoints.Add(point);
        }
    }

    // updates checkpoint to be the one with the highest order number, so player always returns to the checkpoint that is 
    // furthest in terms of level progression
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (manager.lastCheckpoint.GetComponent<Checkpoint>().orderNumber <= orderNumber)
            {
                manager.lastCheckpoint.GetComponent<Checkpoint>().ChangeInactiveColor();
                manager.lastCheckpoint = this.gameObject;
                manager.lastCheckpointPos = transform.position;
                manager.SaveGame();
                checkpointLight.color = checkedColor;
            }
        }
    }

    public void ChangeInactiveColor()
    {
        checkpointLight.color = inactiveColor;
    }

    public Color32 GetCurrentColor()
    {
        return checkpointLight.color;
    }
}
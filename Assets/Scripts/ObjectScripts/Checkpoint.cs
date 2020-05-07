﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Handles that checkpoint is updated to manager when player collides with it
public class Checkpoint : MonoBehaviour
{
    private GameManager manager;

    public Light2D checkpointLight;
    public Color32 checkpointColor;

    public List<GameObject> checkpoints;
    private GameObject[] points;

    public int orderNumber;


    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //checkpointLight = GetComponent<Light2D>();
        checkpoints.Add(GameObject.FindGameObjectWithTag("StartingPoint"));
        points = (GameObject.FindGameObjectsWithTag("Checkpoint"));
        foreach (GameObject point in points)
        {
            checkpoints.Add(point);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (manager.lastCheckpoint.GetComponent<Checkpoint>().orderNumber <= orderNumber)
            {
                manager.lastCheckpoint = this.gameObject;
                manager.lastCheckpointPos = transform.position;
                manager.SaveGame();
                checkpointLight.color = checkpointColor;
            }
        }
    }
}

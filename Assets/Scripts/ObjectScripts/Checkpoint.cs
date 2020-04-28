using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Handles that checkpoint is updated to manager when player collides with it
public class Checkpoint : MonoBehaviour
{
    private GameManager manager;

    public Light2D checkpointLight;
    public Color32 checpointColor;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        checkpointLight = GetComponent<Light2D>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           manager.lastCheckpointPos = transform.position;
           //checkpointLight.color = checpointColor;
        }
    }
}

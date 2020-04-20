using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keeps track of the last checkpoint player visited
public class PlayerPos : MonoBehaviour
{
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        transform.position = manager.lastCheckPointPos;
    }
}

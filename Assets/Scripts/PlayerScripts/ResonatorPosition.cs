// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// updates resonator's position to player's position (resonator isn't a child of player so doesn't update automatically)
public class ResonatorPosition : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
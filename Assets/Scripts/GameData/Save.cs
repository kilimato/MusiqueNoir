using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// here we store values to save for the save file
[System.Serializable]
public class Save
{

    public bool finishedStartingConversation;
    public bool enteringDialogue;

    public List<bool> tilemapsActive = new List<bool>();
    public List<bool> savedPeasants = new List<bool>();
    public List<bool> brokenWalls = new List<bool>();
    public List<bool> enemies = new List<bool>();

    public float[] checkpoint = new float[3];
}




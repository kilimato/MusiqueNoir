using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// here we store values to save for the save file
[System.Serializable]
public class Save
{
    public List<int> changingVisibilityAreas = new List<int>();
    public List<bool> tilemapsActive = new List<bool>();

    public float[] checkpoint = new float[2];
    public bool finishedStartingConversation;
}

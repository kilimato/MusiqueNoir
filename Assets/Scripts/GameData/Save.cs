using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// here we store values to save for the save file
[System.Serializable]
public class Save
{
    public List<int> changingVisibilityAreas = new List<int>();
    public List<bool> tilemapsActive = new List<bool>();

    public bool finishedStartingConversation;
    public bool enteringDialogue;
    public class CheckpointPos
    {
        public float[] checkpoint;

        public CheckpointPos(PlayerPos playerpos)
        {
            checkpoint = new float[2];
            checkpoint[0] = playerpos.transform.position.x;
            checkpoint[1] = playerpos.transform.position.y;
        }
    }
}




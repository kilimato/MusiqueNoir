using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float[] checkpointPosition;

    public PlayerData(GameManager manager)
    {
        checkpointPosition = new float[2];

        checkpointPosition[0] = manager.lastCheckPointPos.x;
        checkpointPosition[1] = manager.lastCheckPointPos.y;
    }
}

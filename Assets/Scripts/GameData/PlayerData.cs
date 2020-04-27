using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public float speed;
    public bool canHide;
    public bool isVisible;


    public PlayerData(PlayerController controller)
    {
        speed = controller.speed;
        canHide = controller.canHide;
        isVisible = controller.isVisible;
    }
}

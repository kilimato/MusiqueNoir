using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for placing information that is unique to each character in their dialogue
public class Character : MonoBehaviour
{
    public Color32 dialogueColor;


    public Color GetDialogueColor()
    {
        return dialogueColor;
    }
}

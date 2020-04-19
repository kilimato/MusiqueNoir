﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private JsonData dialogue;
    public TextMeshProUGUI textDisplay;
    public Canvas dialogueCanvas;
    private int index;
    private string speaker;

    // true when a dialog is loaded and a sentence can be printed,
    // false if dialog hasn't finished loading and if we've reached EOD
    public bool inDialogue;

    // dialogTrigger needs to know whether it's own dialogue has loaded (not just any dialogue)
    // so we return bool to indicate that state
    public bool LoadDialogue(string path)
    {
        // we don't load a dialogue if we're already in one
        if (!inDialogue)
        {
            index = 0;
            var jsonTextFile = Resources.Load<TextAsset>("Dialogues/" + path);
            dialogue = JsonMapper.ToObject(jsonTextFile.text);
            inDialogue = true;
            return true;
        }
        return false;
    }

    // we need to know which dialogTrigger instance needs to know when it ends
    // this returns true when we still have lines to print, and false when we have hit our last line EOD
    public bool PrintLine()
    {
        if (inDialogue)
        {
            JsonData line = dialogue[index];
            if (line[0].ToString() == "EOD")
            {
                inDialogue = false;
                textDisplay.text = "";
                dialogueCanvas.enabled = false;
                return false;
            }
            // every line has exactly one key (the speaker) so this always gives us the correct speaker
            foreach (JsonData key in line.Keys)
            {
                speaker = key.ToString();
            }

            string dialoguePart = speaker + ": " + line[0].ToString();

            StopAllCoroutines();
            DialogueTextColor(speaker);
            StartCoroutine(TypeDialogue(dialoguePart));
            index++;
        }
        return true;
    }


    IEnumerator TypeDialogue(string dialoguePart)
    {
        textDisplay.text = "";
        foreach (char letter in dialoguePart.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }
    private void DialogueTextColor(string character)
    {
        textDisplay.color = GameObject.Find(character).GetComponent<Character>().GetDialogueColor();
    }
}
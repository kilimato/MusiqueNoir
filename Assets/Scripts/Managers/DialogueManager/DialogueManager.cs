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
    private string dialoguePart;

    private bool coroutineRunning;

    // true when a dialog is loaded and a sentence can be printed,
    // false if dialog hasn't finished loading and if we've reached EOD
    public bool inDialogue;

    public bool finishedDialogue = false;

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
        if (finishedDialogue) return false;

        if (inDialogue)
        {
            JsonData line = dialogue[index];
            if (line[0].ToString() == "EOD")
            {
                inDialogue = false;
                textDisplay.text = "";
                dialogueCanvas.enabled = false;
                finishedDialogue = true;
                return false;
            }
            // every line has exactly one key (the speaker) so this always gives us the correct speaker
            foreach (JsonData key in line.Keys)
            {
                speaker = key.ToString();
            }
            if (coroutineRunning)
            {
                StopAllCoroutines();
            }
            dialoguePart = speaker + ": " + line[0].ToString();

            Debug.Log(dialoguePart);


            /* if (coroutineRunning)
             {
                 StopAllCoroutines();
                 textDisplay.text = dialogue[index - 1].Keys +": " + dialogue[index-1][0].ToString();
                 return true;
             }
             */
            //textDisplay.text = dialoguePart;

           // DialogueTextColor(speaker);
            StartCoroutine(TypeDialogue(dialoguePart));
            index++;
        }
        return true;
    }


    IEnumerator TypeDialogue(string dialoguePart)
    {
        coroutineRunning = true;
        textDisplay.text = "";
        
        int revealedChars = 0;
        while (revealedChars < dialoguePart.Length)
        {
            while (dialoguePart[revealedChars] == ' ')
            {
                revealedChars++;
            }
            revealedChars++;
            textDisplay.text = dialoguePart.Substring(0, revealedChars);
            yield return new WaitForSeconds(0.02f);
        }
        coroutineRunning = false;
        
        /*
         * ADDED COLOR TAG TO ENABLE FURTHER DIALOGUE IMPROVEMENT
        coroutineRunning = true;
        textDisplay.text = dialoguePart;
        //textDisplay.color = new Color32(0,0,0,0);
        string currentlyShowingText = "";
        string notVisibleText = "";

        int revealedCharIndex = 0;
        while (revealedCharIndex < dialoguePart.Length)
        {
            revealedCharIndex++;
            //currentlyShowingText = textDisplay.co);
            //textDisplay.text = currentlyShowingText;
            currentlyShowingText = dialoguePart.Substring(0, revealedCharIndex);
            notVisibleText = dialoguePart.Substring(revealedCharIndex, dialoguePart.Length-revealedCharIndex);
            textDisplay.text =  "<#FF0000>" + currentlyShowingText;
            //DialogueTextColor(speaker);

            yield return new WaitForSeconds(0.02f);
        }
        coroutineRunning = false;
        */
    }


    private void DialogueTextColor(string character)
    {
        textDisplay.color = GameObject.Find(character).GetComponent<Character>().GetDialogueColor();
    }

    public bool FinishedDialogue()
    {
        return finishedDialogue;
    }
}

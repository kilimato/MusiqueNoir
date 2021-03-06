﻿// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using TMPro;

// script for managing end dialogue and showing end text when dialogue is finished
public class EndDialogueManager : MonoBehaviour
{
    private JsonData dialogue;
    public TextMeshProUGUI textDisplay;
    public Canvas dialogueCanvas;

    public Image characterImage;
    private int index;
    private string speaker;
    private string dialoguePart;

    public bool coroutineRunning;
    // true when a dialog is loaded and a sentence can be printed,
    // false if dialog hasn't finished loading and if we've reached EOD
    public bool inDialogue;

    public bool finishedDialogue = false;

    [FMODUnity.EventRef]
    public string typingEvent = "";
    [FMODUnity.EventRef]
    public string smashingEvent = "";

    private bool play = true;
    public EndTextController endTextController;

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
        dialogueCanvas.enabled = true;

        if (inDialogue)
        {
            JsonData line = dialogue[index];
            if (line[0].ToString() == "EOD")
            {
                inDialogue = false;
                textDisplay.text = "";
                dialogueCanvas.enabled = false;
                finishedDialogue = true;
                endTextController.ShowEndText();
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
                PrintEntireLine();
            }

            dialoguePart = speaker + ": " + line[0].ToString();

            Debug.Log(dialoguePart);
            DialogueImage(speaker);
            StartCoroutine(TypeDialogue(dialoguePart));
            index++;
        }
        return true;
    }

    public void PrintEntireLine()
    {
        textDisplay.text = TextColorToHex(speaker) + dialoguePart;

        //play sound
        FMODUnity.RuntimeManager.PlayOneShot(smashingEvent);
    }

    IEnumerator TypeDialogue(string dialoguePart)
    {
        coroutineRunning = true;

        string currentlyShowingText = "";
        string notVisibleText = "";

        int revealedCharIndex = 0;
        while (revealedCharIndex < dialoguePart.Length)
        {
            revealedCharIndex++;
            currentlyShowingText = dialoguePart.Substring(0, revealedCharIndex);
            notVisibleText = dialoguePart.Substring(revealedCharIndex, dialoguePart.Length - revealedCharIndex);
            textDisplay.text = TextColorToHex(speaker) + currentlyShowingText + "<#00000000>" + notVisibleText;

            //play sound
            if (play) FMODUnity.RuntimeManager.PlayOneShot(typingEvent);
            play = !play;

            yield return new WaitForSeconds(0.02f);
        }
        coroutineRunning = false;

    }

    private void DialogueImage(string character)
    {
        characterImage.sprite = GameObject.Find(character).GetComponent<Character>().GetDialogueSprite();
    }

    private string TextColorToHex(string character)
    {
        string hexColor = "<#" + ColorUtility.ToHtmlStringRGB(GameObject.Find(character).GetComponent<Character>().GetDialogueColor()) + ">";
        return hexColor;
    }

    public bool FinishedDialogue()
    {
        return finishedDialogue;
    }
}
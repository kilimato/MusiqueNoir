﻿// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//  trigger for activating NPC-dialogue after NPC is rescued
public class RescuedTrigger : MonoBehaviour
{
    public RescuedDialogueManager dialogueManager;
    public GameObject player;
    public GameObject rescued;
    public string dialoguePath;
    public GameObject textElement;
    public TextMeshProUGUI canvasText;

    public bool inTrigger = false;
    public bool dialogueLoaded = false;
    public bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = GameObject.Find("RescuedDialogueManager").GetComponent<RescuedDialogueManager>();
        }
        textElement.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            inTrigger = true;
            textElement.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player && player.GetComponent<PlayerController>().isVisible)
        {
            inTrigger = false;
            textElement.SetActive(false);
            canvasText.text = "";
        }
    }

    private void RunDialogue(bool keyTrigger)
    {
        if (firstTime)
        {
            dialogueLoaded = dialogueManager.LoadDialogue(dialoguePath);
            if (dialogueLoaded)
            {
                dialogueManager.PrintLine();
                firstTime = false;
            }
        }
        else if (keyTrigger)
        {
            if (!dialogueLoaded)
            {
                dialogueLoaded = dialogueManager.LoadDialogue(dialoguePath);
            }
            if (dialogueLoaded)
            {
                dialogueManager.PrintLine();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (rescued.GetComponent<ResonatingNPCController>().startDialogue && firstTime)
        {
            dialogueManager.finishedDialogue = false;
            rescued.GetComponent<ResonatingNPCController>().startDialogue = false;

            RunDialogue(firstTime);
            StartCoroutine(WaitTime());
        }

        if (!firstTime)
        {
            // here we start dialogue when we are in collision area of the NPC and press E
            // could be anything else tp trigger the dialogue
            RunDialogue(Input.GetKeyDown(KeyCode.E));
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
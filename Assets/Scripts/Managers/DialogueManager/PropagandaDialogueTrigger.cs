using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PropagandaDialogueTrigger : MonoBehaviour
{

    public PropagandaDialogueManager dialogueManager;
    public GameObject player;
    public string dialoguePath;
    public Canvas propagandaCanvas;

    public bool inTrigger = false;
    public bool dialogueLoaded = false;

    public bool firstTime = true;

    public int sentenceIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = GameObject.Find("PropagandaDialogueManager").GetComponent<PropagandaDialogueManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player && player.GetComponent<PlayerController>().isVisible)
        {
            inTrigger = false;
        }
    }

    private void RunDialogue()
    {
        if (!dialogueLoaded)
        {
            dialogueLoaded = dialogueManager.LoadDialogue(dialoguePath, sentenceIndex);
            Debug.Log("Dialogue loaded: " + dialoguePath + " " + sentenceIndex);
        }
        /*   else if (dialogueLoaded && dialogueManager.GetCurrentDialoguePath() != dialoguePath)
           {
               Debug.Log("Wrong dialogue: " + dialogueManager.GetCurrentDialoguePath());
               dialogueLoaded = dialogueManager.LoadDialogue(dialoguePath);

               Debug.Log("Dialogue changed: " + dialogueManager.GetCurrentDialoguePath());
           }*/
        else if (dialogueLoaded)
        {
            Debug.Log("Dialogue correct: " + dialoguePath);
            dialogueManager.PrintLine();
            sentenceIndex++;

            if (sentenceIndex > 10)
            {
                sentenceIndex = 0;
                dialogueManager.LoadDialogue(dialoguePath, sentenceIndex);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            // here we start dialogue when we are in collision area of the NPC and press C
            // could be anything else tp trigger the dialogue
            if (!IsInvoking("RunDialogue"))
            {
                propagandaCanvas.enabled = true;
                InvokeRepeating("RunDialogue", 0, 4.5f);
            }
            // RunDialogue(Input.GetKeyDown(KeyCode.C));
        }
        if (!inTrigger)
        {
            if (IsInvoking("RunDialogue"))
            {
                CancelInvoke("RunDialogue");
                propagandaCanvas.enabled = false;
            }
        }
    }
}

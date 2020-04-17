using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject player;
    public string dialoguePath;
    public bool inTrigger = false;
    public bool dialogueLoaded = false;

    public bool startDialog = false;

    public bool firstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        if (dialogueManager == null)
        {
            dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            startDialog = true;
            inTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            inTrigger = false;
        }
    }

    private void RunDialogue(bool trigger)
    {
        if (startDialog && firstTime)
        {
            if (inTrigger && !dialogueLoaded)
            {
                dialogueLoaded = dialogueManager.LoadDialogue(dialoguePath);
            }
            if (dialogueLoaded)
            {
                dialogueManager.PrintLine();
                firstTime = false;
            }
        }
        else if (trigger)
        {
            if (inTrigger && !dialogueLoaded)
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
        if (startDialog)
        {
            // we trigger the dialog for the first time, then press c to continue dialog
            RunDialogue(startDialog);
            startDialog = false;
        }
        else
        {
            // here we start dialogue when we are in collision area of the NPC and press C
            // could be anything else tp trigger the dialogue
            RunDialogue(Input.GetKeyDown(KeyCode.C));
        }
    }
}

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

    private void RunDialogue(bool keyTrigger)
    {
        if (firstTime)
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
        else if (keyTrigger)
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
        if (dialogueManager.coroutineRunning && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.coroutineRunning = false;
            dialogueManager.StopAllCoroutines();
            dialogueManager.PrintEntireLine();
            return;
        }
        if (firstTime && inTrigger)
        {
            // we trigger the dialog for the first time, then press e to continue dialog
            RunDialogue(firstTime);
        }

        if (!firstTime)
        {
            // here we continue dialogue when we are in collision area of the NPC and press E
            // could be anything else to trigger the dialogue
            RunDialogue(Input.GetKeyDown(KeyCode.E));
        }

        if (dialogueManager.finishedDialogue)
        {
            gameObject.SetActive(false);
        }
    }
}

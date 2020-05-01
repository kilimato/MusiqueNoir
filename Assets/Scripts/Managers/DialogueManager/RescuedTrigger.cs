using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescuedTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject player;
    public GameObject rescued;
    public string dialoguePath;

    //public bool inTrigger = false;
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


    private void RunDialogue(bool keyTrigger)
    {
        if (firstTime)
        {
            if (!dialogueLoaded)
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
        if (dialogueManager.coroutineRunning && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.coroutineRunning = false;
            dialogueManager.StopAllCoroutines();
            dialogueManager.PrintEntireLine();
            return;
        }
        if (rescued.GetComponent<ResonatingNPCController>().startDialogue && firstTime)
        {
            dialogueManager.finishedDialogue = false;
            rescued.GetComponent<ResonatingNPCController>().startDialogue = false;

            // we trigger the dialog for the first time, then press e to continue dialog
            RunDialogue(firstTime);
        }

        if (!firstTime)
        {
            // here we start dialogue when we are in collision area of the NPC and press E
            // could be anything else tp trigger the dialogue
            RunDialogue(Input.GetKeyDown(KeyCode.E));
        }
    }
}

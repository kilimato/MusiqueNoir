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

    //how many sentences are in the dialogue file - for some reason this was hard coded as 10 before
    public int sentenceAmount = 10;

    //time revoke waits in seconds before calling RunDialogue again
    private float wait = 13.0f;

    FMOD.Studio.EventInstance dialogueSoundInstance;
    FMOD.Studio.EventDescription dialogueSoundDescription;

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

            //Plays sound and gives it's duration as float seconds
            //wait = PlayOneShotAndGetDuration(sentenceIndex);
            PlayOneShotAndGetDuration(sentenceIndex);

            sentenceIndex++;

            if (sentenceIndex >= sentenceAmount)
            {
                sentenceIndex = 0;
                dialogueManager.LoadDialogue(dialoguePath, sentenceIndex);
            }
        }

    }

    //Plays FMOD sound (now hardcoded as propagande event) as a one shot and gives it's duration in float in seconds
    public float PlayOneShotAndGetDuration(int index)
    {
        int length = 8000;
        dialogueSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Propaganda/Propaganda1/propaganda " + index.ToString());
        dialogueSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(propagandaCanvas.transform));
        dialogueSoundInstance.start();
        dialogueSoundInstance.getDescription(out dialogueSoundDescription);
        dialogueSoundDescription.getLength(out length);
        dialogueSoundInstance.release();
        dialogueSoundInstance.clearHandle();
        return (float)(length / 1000);
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
                InvokeRepeating("RunDialogue", 0, wait);
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

// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LitJson;

// script for managing dialogue triggering when player is in range of a speaker
public class PropagandaDialogueTrigger : MonoBehaviour
{
    public PropagandaDialogueManager dialogueManager;
    public GameObject player;
    public GameObject speaker;

    private JsonData dialogue;
    public string dialoguePath;
    public Canvas propagandaCanvas;
    public int dialogueNumber;

    public bool inTrigger = false;
    public bool dialogueLoaded = false;
    public bool firstTime = true;

    public int sentenceIndex = 0;
    private Transform childTextElement;
    //how many sentences are in the dialogue file
    public int sentenceAmount;

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
        var jsonTextFile = Resources.Load<TextAsset>("Dialogues/" + dialoguePath);
        dialogue = JsonMapper.ToObject(jsonTextFile.text);
        sentenceAmount = dialogue.Count - 1;

        childTextElement = speaker.transform.Find("TextElement");
    }

    // we load correct dialogue when player enters the trigger area, and activate the text element to show dialogue
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            inTrigger = true;
            childTextElement.gameObject.SetActive(true);
            dialogueLoaded = dialogueManager.LoadDialogue(dialoguePath, sentenceIndex);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player && player.GetComponent<PlayerController>().isVisible)
        {
            inTrigger = false;

            childTextElement.gameObject.SetActive(false);
        }
    }


    // here we loop through the dialogue sentences and start from the beginnning after going through all of them so 
    // the speakers always have dialogue to print to screen when player is in range
    private void RunDialogue()
    {
        if (!dialogueLoaded)
        {
            dialogueLoaded = dialogueManager.LoadDialogue(dialoguePath, sentenceIndex);
        }

        else if (dialogueLoaded)
        {
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
        dialogueSoundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Propaganda/Propaganda" + dialogueNumber + "/propaganda " + index.ToString());
        dialogueSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(speaker));
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
            if (!IsInvoking("RunDialogue"))
            {
                propagandaCanvas.enabled = true;
                InvokeRepeating("RunDialogue", 0, wait);
            }
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

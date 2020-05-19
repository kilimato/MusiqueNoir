// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Tilemaps;
using System.IO;
using TMPro;

// GameManager that manages the state of our game and saving/loading
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject player;
    public GameObject resonator;

    public GameObject dialogueManager;
    public GameObject dialogueTrigger;
    public Canvas dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI rescuedText;
    public GameObject introTextController;
    public GameObject[] rescuedDialogueTriggers;
    public GameObject[] rescuedDialogueManagers;
    public Canvas rescuedCanvas;
    public GameObject endDialogueTrigger;

    [SerializeField]
    public GameObject[] changingVisibilityAreas;
    [SerializeField]
    public GameObject[] checkpoints;
    [SerializeField]
    public GameObject[] peasants;
    [SerializeField]
    public GameObject[] enemies;
    [SerializeField]
    public StateMachine sm;

    [SerializeField]
    public GameObject[] breakableObjects;
    [SerializeField]
    public GameObject[] speakers;

    [SerializeField]
    public Vector3 lastCheckpointPos;
    [SerializeField]
    public GameObject lastCheckpoint;
    [SerializeField]
    public GameObject startingPoint;
    [SerializeField]
    public bool finishedStartingConversation;


    public void Start()
    {
        if (instance == null)
        {
            //If null, then this instance is now the singleton of the assigned type
            instance = this;

            //making sure this instance is kept persisted across screens
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void NewGame()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }

        ResetTilemaps();
        ResetDialogues();
        ResetPlayerPosition();
        ResetPlayer();
        ResetResonator();
        ResetPeasants();
        ResetSpeakers();
        ResetObjects();
        ResetEnemies();
        ResetCheckpoints();

        foreach (GameObject breakableObject in breakableObjects)
        {
            breakableObject.SetActive(true);
        }

        Time.timeScale = 1;
    }

    private void ResetPlayer()
    {
        player.SetActive(true);
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Rigidbody2D>().simulated = true;
        player.GetComponent<PlayerController>().isVisible = true;
    }

    private void ResetResonator()
    {
        resonator.GetComponent<ParticleScript>().ResetResonator();
    }

    private void ResetEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.position = enemy.GetComponent<EnemyController>().GetStartingPos();
            sm = enemy.GetComponent<EnemyController>().stateMachine;
            sm.ChangeState(new PatrolState(enemy.GetComponent<EnemyController>()));
            // enemy.GetComponent<EnemyController>().startingDirection = -transform.localScale.x / Mathf.Abs(transform.localScale.x);
        }
    }

    private void ResetPeasants()
    {
        foreach (GameObject peasant in peasants)
        {
            peasant.GetComponent<ResonatingNPCController>().SetBrainwashedState();
        }
    }

    private void ResetSpeakers()
    {
        foreach (GameObject speaker in speakers)
        {
            speaker.GetComponent<ResonatingSpeakerController>().ResetSpeaker();
        }
    }

    private void ResetObjects()
    {
        foreach (GameObject breakableObject in breakableObjects)
        {
            breakableObject.GetComponent<ResonatingObjectController>().ResetObject();
        }
    }

    private void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(startingPoint.transform.position.x, startingPoint.transform.position.y, -1);
    }

    private void ResetTilemaps()
    {
        changingVisibilityAreas[0].SetActive(false);
        changingVisibilityAreas[1].SetActive(true);
        changingVisibilityAreas[2].SetActive(false);
        changingVisibilityAreas[3].SetActive(true);
    }

    private void ResetDialogues()
    {
        dialogueManager.SetActive(true);
        finishedStartingConversation = false;

        dialogueManager.GetComponent<DialogueManager>().inDialogue = false;
        dialogueManager.GetComponent<DialogueManager>().finishedDialogue = false;
        dialogueCanvas.enabled = false;

        dialogueTrigger.SetActive(true);
        dialogueTrigger.GetComponent<DialogueTrigger>().firstTime = true;
        dialogueTrigger.GetComponent<DialogueTrigger>().inTrigger = false;
        dialogueTrigger.GetComponent<DialogueTrigger>().dialogueLoaded = false;
        dialogueText.text = "";

        endDialogueTrigger.SetActive(true);
        endDialogueTrigger.GetComponent<EndDialogueTrigger>().firstTime = true;
        endDialogueTrigger.GetComponent<EndDialogueTrigger>().inTrigger = false;
        endDialogueTrigger.GetComponent<EndDialogueTrigger>().dialogueLoaded = false;

        foreach (GameObject manager in rescuedDialogueManagers)
        {
            manager.GetComponent<RescuedDialogueManager>().finishedDialogue = false;
            manager.GetComponent<RescuedDialogueManager>().inDialogue = false;
        }

        introTextController.GetComponent<IntroTextController>().isAlreadySeen = false;

        foreach (GameObject trigger in rescuedDialogueTriggers)
        {
            trigger.GetComponent<RescuedTrigger>().firstTime = true;
            trigger.GetComponent<RescuedTrigger>().inTrigger = false;
            trigger.GetComponent<RescuedTrigger>().dialogueLoaded = false;
        }
        rescuedCanvas.enabled = false;
    }

    private void ResetCheckpoints()
    {
        // also resets the checkpoint lights
        lastCheckpointPos = startingPoint.transform.position;
        lastCheckpoint = startingPoint;

        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.GetComponent<Checkpoint>().ChangeInactiveColor();
        }
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        foreach (GameObject tilemapGameObject in changingVisibilityAreas)
        {
            save.tilemapsActive.Add(tilemapGameObject.activeSelf);
        }

        foreach (GameObject peasant in peasants)
        {
            save.savedPeasants.Add(peasant.GetComponent<ResonatingNPCController>().saved);
        }

        foreach (GameObject wall in breakableObjects)
        {
            save.brokenWalls.Add(wall.activeSelf);
        }

        save.finishedRescuedConversations.Add(rescuedDialogueManagers[0].GetComponent<RescuedDialogueManager>().finishedDialogue);
        save.finishedRescuedConversations.Add(rescuedDialogueManagers[1].GetComponent<RescuedDialogueManager>().finishedDialogue);

        save.enteringRescuedDialogues.Add(rescuedDialogueTriggers[0].GetComponent<RescuedTrigger>().firstTime);
        save.enteringRescuedDialogues.Add(rescuedDialogueTriggers[1].GetComponent<RescuedTrigger>().firstTime);

        save.finishedStartingConversation = dialogueManager.GetComponent<DialogueManager>().FinishedDialogue();
        save.enteringDialogue = dialogueTrigger.GetComponent<DialogueTrigger>().firstTime;

        save.checkpoint[0] = lastCheckpointPos.x;
        save.checkpoint[1] = lastCheckpointPos.y;
        save.checkpoint[2] = -1;

        return save;
    }

    public void LoadGame(GameObject caller)
    {
        if (Input.GetKey(KeyCode.Space) && caller.tag != "Enemy")
        {
            return;
        }

        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            ResetTilemaps();
            ResetDialogues();
            ResetPlayerPosition(); 
            ResetPlayer();
            ResetResonator();
            ResetCheckpoints();
            ResetPeasants();
            ResetObjects();
            ResetSpeakers();
            ResetEnemies();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            changingVisibilityAreas[0].SetActive(save.tilemapsActive[0]);
            changingVisibilityAreas[1].SetActive(save.tilemapsActive[1]);
            changingVisibilityAreas[2].SetActive(save.tilemapsActive[2]);
            changingVisibilityAreas[3].SetActive(save.tilemapsActive[3]);

            dialogueManager.GetComponent<DialogueManager>().finishedDialogue = save.finishedStartingConversation;
            dialogueTrigger.GetComponent<DialogueTrigger>().firstTime = save.enteringDialogue;


            rescuedDialogueManagers[0].GetComponent<RescuedDialogueManager>().finishedDialogue = save.finishedRescuedConversations[0];
            rescuedDialogueManagers[1].GetComponent<RescuedDialogueManager>().finishedDialogue = save.finishedRescuedConversations[1];
            
            rescuedDialogueTriggers[0].GetComponent<RescuedTrigger>().firstTime = save.enteringRescuedDialogues[0];
            rescuedDialogueTriggers[1].GetComponent<RescuedTrigger>().firstTime = save.enteringRescuedDialogues[1];


            lastCheckpointPos.x = save.checkpoint[0];
            lastCheckpointPos.y = save.checkpoint[1];
            lastCheckpointPos.z = save.checkpoint[2];

            player.transform.position = lastCheckpointPos;
            for (int i = 0; i < peasants.Length; i++)
            {
                peasants[i].GetComponent<ResonatingNPCController>().saved = save.savedPeasants[i];
                peasants[i].GetComponent<Animator>().SetBool("IsSaved", save.savedPeasants[i]);
                if (save.savedPeasants[i])
                {
                    peasants[i].GetComponent<ResonatingNPCController>().SetSavedState();
                }
            }

            for (int i = 0; i < breakableObjects.Length; i++)
            {
                breakableObjects[i].SetActive(save.brokenWalls[i]);
            }


            rescuedDialogueManagers[0].GetComponent<RescuedDialogueManager>().finishedDialogue = save.finishedRescuedConversations[0];
            rescuedDialogueManagers[1].GetComponent<RescuedDialogueManager>().finishedDialogue = save.finishedRescuedConversations[1];

            ResetObjects();
            ResetSpeakers();
            ResetResonator();

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }

        Time.timeScale = 1;
    }
}

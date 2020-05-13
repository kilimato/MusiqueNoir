using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Tilemaps;
using System.IO;
using TMPro;

// GameManager that manages the state of our game
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject player;

    public GameObject dialogueManager;
    public GameObject dialogueTrigger;
    public Canvas dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public GameObject introTextController;

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
        
        /*
        // 3: resetting the game so that everything is in a default state
        ResetTilemaps();
        // ResetCheckpoints();
        //currentCheckpoint = startingCheckpoint;
        finishedStartingConversation = false;
        */
        Debug.Log("Game Saved");
    }

    public void NewGame()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        

        //File.Delete(Application.persistentDataPath + "/gamesave.save");

        ResetTilemaps();
        ResetDialogues();
        ResetPlayerPosition();
        ResetPlayer();
        ResetPeasants();
        ResetObjects();
        ResetEnemies();
        ResetCheckpoints();

        Time.timeScale = 1;
    }

    private void ResetPlayer()
    {
        player.SetActive(true);
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Rigidbody2D>().simulated = true;
        player.GetComponent<PlayerController>().isVisible = true;
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
            peasant.GetComponent<ResonatingNPCController>().saved = false;
        }
    }

    private void ResetObjects()
    {
        foreach (GameObject breakableObject in breakableObjects)
        {
            breakableObject.SetActive(true);
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
        //dialogueCanvas.enabled = true;
        dialogueText.text = "";

        introTextController.GetComponent<IntroTextController>().isAlreadySeen = false;
    }
    private void ResetCheckpoints()
    {
        // resettaa valotkin
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
        //int i = 0;
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

        save.finishedStartingConversation = dialogueManager.GetComponent<DialogueManager>().FinishedDialogue();
        save.enteringDialogue = dialogueTrigger.GetComponent<DialogueTrigger>().firstTime;

        save.checkpoint[0] = lastCheckpointPos.x;
        save.checkpoint[1] = lastCheckpointPos.y;
        save.checkpoint[2] = -1;

        return save;
    }

    public void LoadGame(GameObject caller)
    {
        // current problem: if we check for enemy hitting player and loading then, for some reason loading game does 
        // not change timescale from 0 to 1
        if (Input.GetKey(KeyCode.Space) && caller.tag != "Enemy")
        {
            return;
        }
       
    // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            ResetTilemaps(); // CHECK!
            ResetDialogues();
            ResetPlayerPosition(); // CHECK!
            ResetPlayer();
            ResetCheckpoints();   // CHECK!
            ResetPeasants();
            ResetObjects();
            ResetEnemies();
            

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            changingVisibilityAreas[0].SetActive(save.tilemapsActive[0]);
            changingVisibilityAreas[1].SetActive(save.tilemapsActive[1]); 
            changingVisibilityAreas[2].SetActive(save.tilemapsActive[2]);
            changingVisibilityAreas[3].SetActive(save.tilemapsActive[3]);

            dialogueManager.GetComponent<DialogueManager>().finishedDialogue = save.finishedStartingConversation;
            if (save.finishedStartingConversation)
            {
               // dialogueManager.SetActive(false);
            }
            dialogueTrigger.GetComponent<DialogueTrigger>().firstTime = save.enteringDialogue;

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

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }

        Time.timeScale = 1;
    }

    private void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        */
    }
}

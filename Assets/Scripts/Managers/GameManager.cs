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

    [SerializeField]
    private GameObject[] changingVisibilityAreas;
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

        ResetTilemaps();
        ResetDialogues();
        ResetPlayerPosition();
        ResetPeasants();
        ResetObjects();
        ResetEnemies();
        Time.timeScale = 1;
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
    }

    private void ResetDialogues()
    {
        finishedStartingConversation = false;
        dialogueManager.GetComponent<DialogueManager>().finishedDialogue = false;
        //alogueManager.GetComponent<DialogueManager>().dialogu
        dialogueManager.SetActive(true);
        dialogueTrigger.GetComponent<DialogueTrigger>().firstTime = true;
        dialogueTrigger.GetComponent<DialogueTrigger>().dialogueLoaded = false;
        //dialogueCanvas.enabled = true;
        dialogueText.text = "";
    }
    private void ResetCheckpoints()
    {
        lastCheckpointPos = startingPoint.transform.position;
        lastCheckpoint = startingPoint;
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

    public void LoadGame()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            
            ResetTilemaps();
            ResetDialogues();
            ResetPlayerPosition();
            ResetCheckpoints();
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
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
    }
}

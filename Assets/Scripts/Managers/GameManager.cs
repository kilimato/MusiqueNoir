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

    public GameObject dialogueManager;
    public GameObject dialogueTrigger;
    public Canvas dialogueCanvas;

    public TextMeshProUGUI dialogueText;

    [SerializeField]
    private GameObject[] changingVisibilityAreas;
    /*
    [SerializeField]
    public Vector2 currentCheckpoint;
    [SerializeField]
    public float[] startingCheckpoint = new float[2];*/
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

        // 3: resetting the game so that everything is in a default state
         ResetTilemaps();
       // ResetCheckpoints();
        //currentCheckpoint = startingCheckpoint;
        finishedStartingConversation = false;

        Debug.Log("Game Saved");
    }

    public void NewGame()
    {
        ResetTilemaps();
        ResetDialogues();
        Time.timeScale = 1;
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
        dialogueCanvas.enabled = true;
        dialogueText.text = "";
    }
    private void ResetCheckpoints()
    {
       // currentCheckpoint = startingCheckpoint;
    }


    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        //int i = 0;
        foreach (GameObject tilemapGameObject in changingVisibilityAreas)
        {
            //Tilemap tilemap = tilemapGameObject.GetComponent<Tilemap>();
            if (/*target.activeRobot != null*/true)
            {
                //save.changingVisibilityAreas.Add(tilemapGameObject);
                save.tilemapsActive.Add(tilemapGameObject.activeSelf);
                //save.livingTargetPositions.Add(target.position);
                //save.livingTargetsTypes.Add((int)target.activeRobot.GetComponent<Robot>().type);
                // i++;
            }
        }

        // save.hits = hits;
        // save.shots = shots;
        //save.checkpoint[0] = currentCheckpoint[0];
        //save.checkpoint[1] = currentCheckpoint[1];
        save.finishedStartingConversation = dialogueManager.GetComponent<DialogueManager>().FinishedDialogue();
        save.enteringDialogue = dialogueTrigger.GetComponent<DialogueTrigger>().firstTime;

        return save;
    }

    public void LoadGame()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            ResetTilemaps();
            ResetDialogues();

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
                dialogueManager.SetActive(false);
            }
            dialogueTrigger.GetComponent<DialogueTrigger>().firstTime = save.enteringDialogue;

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }

        Time.timeScale = 1;
    }
}

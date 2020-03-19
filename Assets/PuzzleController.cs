using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{

    private Canvas canvas;

    bool puzzleSolved = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }


    private void AbortPuzzle()
    {
        Destroy(gameObject);
    }

    public void EndPuzzle()
    {
        if (puzzleSolved)
        {
            GetComponentInParent<PuzzleResonatingObjectController>().DisableObject();
            Destroy(gameObject);
        }
        else
        {
            GetComponentInParent<PuzzleResonatingObjectController>().ClosePuzzle();
        }
    }

    public void SolvePuzzle()
    {
        puzzleSolved = true;
    }
}

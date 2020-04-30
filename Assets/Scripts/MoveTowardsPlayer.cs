using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class MoveTowardsPlayer : MonoBehaviour
{
    public Seeker seeker;
    public AIPath aiPath;
    public AIDestinationSetter destSetter;

    public GameObject alertSign;
    private GameObject target;
    private PlayerController player;

    public StateMachine sm;

    public Animator animator;

    GameObject manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager");
        target = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        destSetter = GetComponent<AIDestinationSetter>();

        player = target.GetComponent<PlayerController>();

        sm = GetComponent<EnemyController>().stateMachine;

        seeker.enabled = false;
        aiPath.enabled = false;
        destSetter.enabled = false;
        destSetter.target = target.transform;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player") && sm.GetCurrentState() != "ChaseState")
        {

            sm.ChangeState(new ChaseState(GetComponent<EnemyController>()));

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player") && player.IsVisible())
        {
            gameObject.SetActive(false);
            //manager.GetComponent<GameManager>().LoadGame();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

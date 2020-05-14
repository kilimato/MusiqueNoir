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
    private DeathTransitionController deathController;

    public StateMachine sm;

    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        destSetter = GetComponent<AIDestinationSetter>();

        player = target.GetComponent<PlayerController>();
        deathController = GameObject.Find("GameEndCanvas").GetComponent<DeathTransitionController>();
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
            deathController.DeathTransitionToBlack(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MoveTowardsPlayer : MonoBehaviour
{
    public Seeker seeker;
    public AIPath aiPath;
    public AIDestinationSetter destSetter;

    public GameObject alertSign;
    private GameObject target;
    public GameObject startingPos;

    public StateMachine sm;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.Find("Player");
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        destSetter = GetComponent<AIDestinationSetter>();

        sm = GetComponent<EnemyController>().stateMachine;

        seeker.enabled = false;
        aiPath.enabled = false;
        destSetter.enabled = false;
        destSetter.target = target.transform;

    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(transform.position, target.transform.position) > 10f)
        {

            if (sm.GetCurrentState() == "ChaseState") sm.ChangeState(new PatrolState(GetComponent<EnemyController>()));
            ReturnToStartPos();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && sm.GetCurrentState() == "PatrolState")
        {

            sm.ChangeState(new ChaseState(GetComponent<EnemyController>()));

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    // not working right now 
    private void ReturnToStartPos()
    {
        //destSetter.target = startingPos.transform;
    }

}

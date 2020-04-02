using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChaseState : IState
{

    private Seeker seeker;
    private AIPath aiPath;
    private AIDestinationSetter destSetter;

    //private GameObject alertSign;
    private GameObject target;
    //private GameObject startingPos;

    readonly EnemyController enemy;

    public ChaseState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {

        Debug.Log("Entering Chase State");

        seeker = enemy.GetComponent<Seeker>();
        aiPath = enemy.GetComponent<AIPath>();
        destSetter = enemy.GetComponent<AIDestinationSetter>();

        target = GameObject.FindGameObjectWithTag("Player");

        seeker.enabled = true;
        aiPath.enabled = true;
        destSetter.enabled = true;

        enemy.GetComponent<Animator>().SetBool("Alerted", true);

    }

    public void Execute()
    {

        //Debug.Log("Executing Chase State");
        //alertSign.SetActive(true);
        destSetter.target = target.transform;

    }

    public void Exit()
    {
        seeker.enabled = false;
        aiPath.enabled = false;
        destSetter.enabled = false;
        enemy.GetComponent<Animator>().SetBool("Alerted", false);
        //Debug.Log("Exiting Chase State");

    }
}
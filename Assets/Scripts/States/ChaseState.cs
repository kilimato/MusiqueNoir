// @author Tapio Mylläri
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Chasing state for enemies. Enemies chase the player during this state.
/// </summary>
public class ChaseState : IState
{
    private Seeker seeker;
    private AIPath aiPath;
    private AIDestinationSetter destSetter;

    //private GameObject alertSign;
    private GameObject target;
    private PlayerController player;

    private float chaseTime = 3f;

    readonly EnemyController enemy;

    [FMODUnity.EventRef]
    public string soundEvent = "event:/SFX/Enemy/alert";

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
        player = target.GetComponent<PlayerController>();

        seeker.enabled = true;
        aiPath.enabled = true;
        destSetter.enabled = true;

        enemy.GetComponent<Animator>().SetBool("Alerted", true);

        //play sound
        FMODUnity.RuntimeManager.PlayOneShot(soundEvent, seeker.transform.position);
    }

    public void Execute()
    {
        chaseTime -= Time.deltaTime;
        destSetter.target = target.transform;

        if (target.transform.position.x < enemy.transform.position.x)
        {
            enemy.Move(5f, new Vector2(-1, 0));
        }
        else if (target.transform.position.x > enemy.transform.position.x)
        {
            enemy.Move(5f, new Vector2(1, 0));
        }

        if (Vector2.Distance(enemy.transform.position, target.transform.position) > 10f || chaseTime <= 0)
        {
            if (enemy.stateMachine.GetCurrentState() == "ChaseState") enemy.stateMachine.ChangeState(new ReturnState(enemy));
        }
        if (!player.IsVisible()) enemy.stateMachine.ChangeState(new ReturnState(enemy));
    }

    public void Exit()
    {
        seeker.enabled = false;
        aiPath.enabled = false;
        destSetter.enabled = false;
        enemy.GetComponent<Animator>().SetBool("Alerted", false);

        enemy.ReturnToStartingPos();
    }
}
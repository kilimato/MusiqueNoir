// @author Tapio Mylläri
using UnityEngine;

public class PatrolState : IState
{
    readonly EnemyController enemy;

    public float patrolDirection;

    public PatrolState(EnemyController enemy) { this.enemy = enemy; }

    public void Enter()
    {
        //Debug.Log("Enter Patrol State");
        patrolDirection = enemy.GetStartingDirection();
        enemy.patrolMovementTime = enemy.startPatrolMovementTime;
        enemy.patrolTurnTime = enemy.startPatrolTurnTime;
    }

    public void Execute()
    {
        // Debug.Log("Executing Patrol State");

        if (enemy.patrolMovementTime <= 0f)
        {
            enemy.animator.SetFloat("Speed", 0);
            enemy.patrolTurnTime -= Time.deltaTime;
            if (enemy.patrolTurnTime <= 0f)
            {
                patrolDirection *= -1;
                enemy.patrolMovementTime = enemy.startPatrolMovementTime;
                enemy.patrolTurnTime = enemy.startPatrolTurnTime;
            }
        }
        else
        {
            enemy.patrolMovementTime -= Time.deltaTime;
            enemy.Move(enemy.patrolSpeed, new Vector2(patrolDirection, 0));
            enemy.animator.SetFloat("Speed", enemy.patrolSpeed);
        }
    }

    public void Exit()
    {
        //Debug.Log("Exiting Patrol State");
    }
}
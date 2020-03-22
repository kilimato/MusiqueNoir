using UnityEngine;

public class PatrolState : IState
{

    readonly EnemyController enemy;

    public float patrolDirection = -1f;

    public PatrolState(EnemyController enemy) { this.enemy = enemy; }

    public void Enter()
    {
        Debug.Log("Enter Patrol State");
        enemy.patrolMovementTime = 2f;
        enemy.patrolSpeed = 2f;
        enemy.patrolTurnTime = 2f;
    }

    public void Execute()
    {
        Debug.Log("Executing Patrol State");

        if (enemy.patrolMovementTime <= 0f)
        {

            enemy.patrolTurnTime -= 1f * Time.deltaTime;
            if (enemy.patrolTurnTime <= 0f)
            {
                enemy.transform.localScale = new Vector3(enemy.transform.localScale.x * -1f, 2.5f, 1f);
                patrolDirection *= -1;
                enemy.patrolMovementTime = 2f;
                enemy.patrolTurnTime = 2f;
            }

        }
        else
        {

            enemy.patrolMovementTime -= 1f * Time.deltaTime;
            Vector3 movement = new Vector3(patrolDirection, 0f, 0f) * enemy.patrolSpeed * Time.deltaTime;
            enemy.transform.position += movement;

        }


    }

    public void Exit()
    {
        Debug.Log("Exiting Patrol State");
    }
}
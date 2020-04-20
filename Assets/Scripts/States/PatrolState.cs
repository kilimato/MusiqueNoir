using UnityEngine;

public class PatrolState : IState
{

    readonly EnemyController enemy;

    public float patrolDirection;

    public PatrolState(EnemyController enemy) { this.enemy = enemy; }

    public void Enter()
    {
        //Debug.Log("Enter Patrol State");
        patrolDirection = -enemy.transform.localScale.x / Mathf.Abs(enemy.transform.localScale.x);
    }

    public void Execute()
    {
       // Debug.Log("Executing Patrol State");

        if (enemy.patrolMovementTime <= 0f)
        {

            enemy.patrolTurnTime -= 1f * Time.deltaTime;
            if (enemy.patrolTurnTime <= 0f)
            {
                enemy.transform.localScale = new Vector3(enemy.transform.localScale.x * -1f, enemy.transform.localScale.y, 1f);
                patrolDirection *= -1;
                enemy.patrolMovementTime = enemy.startPatrolMovementTime;
                enemy.patrolTurnTime = enemy.startPatrolTurnTime;
            }

        }
        else
        {

            enemy.patrolMovementTime -= 1f * Time.deltaTime;
            Vector3 movement = new Vector3(patrolDirection, 0f, 0f) * enemy.patrolSpeed * Time.deltaTime;
            enemy.transform.position += movement;
            enemy.GetComponent<Animator>().SetFloat("Speed", enemy.patrolMovementTime);

        }
    }

    public void Exit()
    {
        //Debug.Log("Exiting Patrol State");
    }

}
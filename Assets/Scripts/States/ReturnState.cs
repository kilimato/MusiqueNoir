// @author Tapio Mylläri
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IState
{
    readonly EnemyController enemy;

    public ReturnState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        //throw new System.NotImplementedException();
    }

    public void Execute()
    {
        if (Mathf.Abs(enemy.transform.position.x - enemy.GetStartingPos().x) < 0.15f)
        {
            enemy.stateMachine.ChangeState(new PatrolState(enemy));
        }
        enemy.animator.SetFloat("Speed", enemy.patrolSpeed);
        enemy.ReturnToStartingPos();
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }
}

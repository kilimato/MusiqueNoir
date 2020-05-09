using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : IState
{

    EnemyController enemy;
    float stunTime;

    public StunnedState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {

        stunTime = 10f;
    }

    public void Execute()
    {

        while(stunTime < 5f)
        {
            stunTime -= Time.deltaTime;
        }

        enemy.stateMachine.ChangeState(new ReturnState(enemy));

    }

    public void Exit()
    {

    }
}

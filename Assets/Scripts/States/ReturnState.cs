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
        if(Vector3.Distance(enemy.transform.position, enemy.GetStartingPos()) < 0.1f)
        {
            enemy.stateMachine.ChangeState(new PatrolState(enemy));
        }
        enemy.ReturnToStartingPos();
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
}

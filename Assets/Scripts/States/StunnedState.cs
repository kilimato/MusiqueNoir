using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : IState
{

    EnemyController enemy;
    float stunTime;

    [FMODUnity.EventRef]
    public string soundEvent = "event:/SFX/Enemy/stunned";

    public StunnedState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        //play sound
        FMODUnity.RuntimeManager.PlayOneShot(soundEvent, enemy.transform.position);

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

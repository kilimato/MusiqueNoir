using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : IState
{

    EnemyController enemy;
    private float stunTime;

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

        enemy.rb2D.simulated = false;

        stunTime = 5f;
    }

    public void Execute()
    {

        if (stunTime > Mathf.Epsilon)
        {
            stunTime -= Time.deltaTime;
        }
        else
        {
            enemy.stateMachine.ChangeState(new ReturnState(enemy));
        }

    }

    public void Exit()
    {

        enemy.rb2D.simulated = true;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public string state = "Patrol";

    public float patrolMovementTime = 2f; //milliseconds
    public float patrolTurnTime = 2f; //milliseconds
    public float patrolSpeed = 2f;

    readonly StateMachine stateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(new PatrolState(this));
    }

    // Update is called once per frame
    void Update()
    {

        stateMachine.Update();

    }
}

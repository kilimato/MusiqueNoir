using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private string state;

    public float patrolMovementTime = 2f; 
    public float patrolTurnTime = 2f;
    public float patrolSpeed = 2f;

    public StateMachine stateMachine = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(new PatrolState(this));
        state = stateMachine.GetCurrentState();
    }

    // Update is called once per frame
    void Update()
    {
        state = stateMachine.GetCurrentState(); //Poista tää jossakin vaihees

        stateMachine.Update();

    }
}

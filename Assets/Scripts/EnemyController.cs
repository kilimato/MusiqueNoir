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

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(new PatrolState(this));
        state = stateMachine.GetCurrentState();
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        state = stateMachine.GetCurrentState(); //Poista tää jossakin vaihees

        stateMachine.Update();

    }

    public void Move(float speed, bool isGoingLeft)
    {
        if(isGoingLeft)
        {
            transform.localScale = new Vector3(2f, 2f, 1f);
            Vector3 movement = new Vector3(-1f, 0f, 0f) * speed * Time.deltaTime;
            transform.position += movement;
        }
        else
        {
            transform.localScale = new Vector3(-2f, 2f, 1f);
            Vector3 movement = new Vector3(1f, 0f, 0f) * speed * Time.deltaTime;
            transform.position += movement;
        }
    }

    public Vector3 GetStartingPos()
    {
        return startingPos;
    }

    public void ReturnToStartingPos()
    {
        if (transform.position.x > startingPos.x)
        {
            Move(patrolSpeed, true);
        }
        else
        {
            Move(patrolSpeed, false);
        }
    }
}

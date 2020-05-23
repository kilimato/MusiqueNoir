// @author Tapio Mylläri
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy controller script. Handles enemies data and initialization. 
/// </summary>
public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private string state;

    //These are modified by PatrolState and are used for visually debugging in editor.
    //Better alternative would be to pass these values to PatrolState and never modify
    //them, but I have no idea how to debug those values visually unless done like this.
    public float patrolMovementTime = 2f;
    public float patrolTurnTime = 2f;
    public float patrolSpeed = 2f;

    //These are the actual unmodified values for entering the PatrolState.
    [HideInInspector]
    public float startPatrolMovementTime;
    [HideInInspector]
    public float startPatrolTurnTime;

    public StateMachine stateMachine = new StateMachine();
    public Rigidbody2D rb2D;

    public Animator animator;

    private Vector3 startingPos;
    public float startingDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        startingDirection = -transform.localScale.x / Mathf.Abs(transform.localScale.x);
        startPatrolMovementTime = patrolMovementTime;
        startPatrolTurnTime = patrolTurnTime;
        stateMachine.ChangeState(new PatrolState(this));
        state = stateMachine.GetCurrentState();
    }

    // Update is called once per frame
    void Update()
    {
        state = stateMachine.GetCurrentState(); //Debuggausta varten
        stateMachine.Update();
    }

    // Moves the enemy to a given direction at a given speed.
    public void Move(float speed, Vector2 direction)
    {
        Vector2 movement = direction * speed * Time.deltaTime;
        rb2D.velocity = movement;
        rb2D.position += movement;
    }

    public Vector3 GetStartingPos()
    {
        return startingPos;
    }

    public float GetStartingDirection()
    {
        return startingDirection;
    }

    public void ReturnToStartingPos()
    {
        if (transform.position.x > startingPos.x)
        {
            Move(patrolSpeed, new Vector2(-1, 0));
        }
        else
        {
            Move(patrolSpeed, new Vector2(1, 0));
        }
    }
}

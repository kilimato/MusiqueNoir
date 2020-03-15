using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public string state = "Patrol";

    public float patrolMovementTime = 2f; //milliseconds
    public float patrolTurnTime = 2f; //milliseconds
    public float patrolSpeed = 2f;
    private float patrolDirection = -1f;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(state == "Patrol" && !animator.GetBool("Alerted"))
        {

            if (patrolMovementTime <= 0f)
            {

                patrolTurnTime -= 1f * Time.deltaTime;
                if(patrolTurnTime <= 0f)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1f, 2.5f, 1f);
                    patrolDirection *= -1;
                    patrolMovementTime = 4f;
                    patrolTurnTime = 2f;
                }

            }
            else
            {

                patrolMovementTime -= 1f * Time.deltaTime;
                Vector3 movement = new Vector3(patrolDirection, 0f, 0f) * patrolSpeed * Time.deltaTime;
                transform.position += movement;

            }


        }
        
    }
}

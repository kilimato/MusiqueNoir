using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float horizontalInput;
    private float verticalInput;

    public GameObject mainCircle;
    public GameObject secondaryCircle;
    public ClimbLadder climbLadder;

    public Animator animator;

    private Vector3 minSize = new Vector3(0.01f, 0.01f, 1);

    // Update is called once per frame
    void Update()
    {
        PlayerMoves();
        CanPlayerClimb();

        IsMusicPlayed();
    }

    private void PlayerMoves()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
        
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;
        transform.position += movement;
        //transform.Translate(Vector2.right * horizontalInput * speed * Time.deltaTime);

        // flips the player sprite to face to the right direction
        if (movement.x >= 0.01f)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        }
        else if (movement.x <= -0.01f)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f, 1f);
        }

    }


    private void CanPlayerClimb()
    {
        if (climbLadder.CanPlayerClimb())
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            Vector3 movement = new Vector3(0f, verticalInput / 2, 0f) * speed * Time.deltaTime;
            transform.position += movement;
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }


    public void IsMusicPlayed()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            mainCircle.transform.localScale += new Vector3(0.1f, 0.1f, 0) * Time.deltaTime;
            secondaryCircle.transform.localScale += new Vector3(0.1f, 0.1f, 0) * Time.deltaTime;

            animator.SetBool("IsPlayingMusic", true);
        }

        if (!Input.GetKey(KeyCode.Space) && (mainCircle.transform.localScale.x > 0.000001))
        {
            mainCircle.transform.localScale -= new Vector3(0.1f, 0.1f, 0) * Time.deltaTime;
            secondaryCircle.transform.localScale -= new Vector3(0.1f, 0.1f, 0) * Time.deltaTime;

            animator.SetBool("IsPlayingMusic", false);
        }
    }
}

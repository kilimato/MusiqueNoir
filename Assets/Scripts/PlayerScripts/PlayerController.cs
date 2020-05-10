using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float runningSpeed = 8f;
    public float horizontalInput;
    public float verticalInput;

    public Animator animator;

    private Vector3 minSize = new Vector3(0.01f, 0.01f, 1);
    private SpriteRenderer sr;

    public HidePlayer hideplayerScript;
    public bool canHide = false;
    public bool isVisible = true;
    public bool usingElevator = false;

    private Rigidbody2D rb;

    private Vector3 movement = Vector3.zero;

    public DialogueManager dialogueManager;

    [FMODUnity.EventRef]
    public string goToHiding = "";
    [FMODUnity.EventRef]
    public string comeFromHiding = "";

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hideplayerScript = GetComponent<HidePlayer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoves();

        IsPlayerUsingResonator();

        CanPlayerHide();
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement);
    }

    // eli: jos pelaaja on alueella (canHide = true) ja painanut E:tä -> muututaan näkymättömäksi, painaessaan uudestaan -> muuttuu näkyväksi
    private void CanPlayerHide()
    {
        if (canHide && isVisible && Input.GetKeyDown(KeyCode.E))
        {
            isVisible = !isVisible;
            sr.enabled = false;
            rb.simulated = false;

            //play sound
            FMODUnity.RuntimeManager.PlayOneShot(goToHiding);
        }
        else if (!isVisible && Input.GetKeyDown(KeyCode.E))
        {
            isVisible = !isVisible;
            sr.enabled = true;
            rb.simulated = true;

            //play sound
            FMODUnity.RuntimeManager.PlayOneShot(comeFromHiding);
        }
    }

    //trigger for player resonator animation
    private void IsPlayerUsingResonator()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("IsPlayingMusic", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("IsPlayingMusic", false);
        }
    }

    private void PlayerMoves()
    {
        movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.fixedDeltaTime;
        // player can't move if they're hiding, in elevator or in dialogue
        if (dialogueManager.inDialogue || !isVisible)
        {
            movement = Vector3.zero;
            animator.SetFloat("Speed", 0.0f);
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        verticalInput = Input.GetAxis("Vertical");

        // player run animation trigger
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement = new Vector3(horizontalInput, 0f, 0f) * runningSpeed * Time.fixedDeltaTime;
            animator.SetBool("IsRunning", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("IsRunning", false);
        }



        // flips the player sprite to face to the right direction
        if (movement.x != 0)
        {
            sr.flipX = movement.x > 0f ? true : false;
        }

        //if (movement.x >= 0.01f)
        //{
        //    transform.localScale = new Vector3(2f, 2.5f, 1f);
        //}
        //else if (movement.x <= -0.01f)
        //{
        //    transform.localScale = new Vector3(-2f, 2.5f, 1f);
        //}

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HidingPlace"))
        {
            canHide = true;
        }
        if (collision.CompareTag("Elevator"))
        {
            usingElevator = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HidingPlace"))
        {
            canHide = false;
        }
        if (collision.CompareTag("Elevator"))
        {
            usingElevator = false;
        }
    }
    public bool IsVisible()
    {
        return isVisible;
    }

    public bool IsUsingElevator()
    {
        return usingElevator;
    }
}

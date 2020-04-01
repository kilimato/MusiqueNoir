using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float horizontalInput;
    private float verticalInput;

    public Animator animator;

    private Vector3 minSize = new Vector3(0.01f, 0.01f, 1);
    private SpriteRenderer sr;

    public HidePlayer hideplayerScript;
    private bool isHidden = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hideplayerScript = GetComponent<HidePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoves();

        CanPlayerHide();
    }

    private void CanPlayerHide()
    {
        if(isHidden && Input.GetKey(KeyCode.E))
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }
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

        if (movement.x != 0)
        {
            sr.flipX = movement.x > 0f ? true : false;
        }

        //if (movement.x >= 0.01f)
        //{
        //    transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        //}
        //else if (movement.x <= -0.01f)
        //{
        //    transform.localScale = new Vector3(-2.5f, 2.5f, 1f);
        //}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HidingPlace"))
        {
            isHidden = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HidingPlace"))
        {
            isHidden = false;
        }
    }
}

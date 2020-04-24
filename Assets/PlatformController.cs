using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    public bool isColliding = false;
    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && isColliding )
        {
            Debug.Log("Collided with player");
            edgeCollider.isTrigger = true;
        }
        if(!isColliding)
        {
            edgeCollider.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }
}

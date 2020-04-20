using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{
    public Vector3 posA;
    public Vector3 posB;

    private Vector3 nexPos;
    public PlatformEffector2D upperFloorOneWayPlatform;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform transformB;

    public bool moving = false;
    public bool onElevator = false;
    void Start()
    {
        posA = transform.localPosition;
        posB = transformB.localPosition;
        nexPos = posB;
    }

    private void FixedUpdate()
    {
        if (onElevator && Input.GetKeyDown(KeyCode.E))
        {
            moving = true;
            //upperFloorOneWayPlatform.rotationalOffset = upperFloorOneWayPlatform.rotationalOffset != 180 ? 180 : 0;
        }

        if (moving)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, nexPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.localPosition, nexPos) <= 0.01)
        {
            moving = false;
            ChangeDestination();
        }
    }

    private void ChangeDestination()
    {
        nexPos = nexPos != posA ? posA : posB;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            onElevator = true;
            //other.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            onElevator = false;
            // other.gameObject.transform.parent = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //onElevator = true;
            other.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //onElevator = false;
            other.gameObject.transform.parent = null;
        }
    }

    public bool IsMoving()
    {
        return moving;
    }
}

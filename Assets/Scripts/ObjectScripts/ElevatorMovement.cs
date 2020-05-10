using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{

    public Vector3 posA;
    public Vector3 posB;
    public float speed = 1;
    public Animator aniA;
    public Animator aniB;

    private GameObject player;

    private Vector3 tempPos;
    private float t;
    private bool isTeleportingEnabled;
    private Vector3 startingPos;
    private Vector3 leavingPos;
    private bool usingElevator;

    // Start is called before the first frame update
    void Start()
    {

        isTeleportingEnabled = false;

        posA = transform.Find("ElevatorPosA").position;
        posB = transform.Find("ElevatorPosB").position;

        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {

        if (isTeleportingEnabled && Input.GetKeyDown(KeyCode.E))
        {
            // triggering animation
            if (startingPos == posA)
            {
                aniA.SetTrigger("PlayerLeaves");
            }
            else if (startingPos == posB)
            {
                aniB.SetTrigger("PlayerLeaves");
            }

            StartCoroutine(SmoothMovement(player, startingPos));
        }

    }

    IEnumerator SmoothMovement(GameObject movableObject, Vector3 startingPos)
    {
        usingElevator = true;
        isTeleportingEnabled = false;
        movableObject.SetActive(false);


        //Vähän ruma tällai tarkastella, mutten tähän hätään keksi miten vaihtaa clamp-tarkistusta, 
        //t:n arvoa ja sen summauksen ja vähentämisen riippuvuutta t:n lähtöarvosta siistimmin.
        if (startingPos == posA)
        {
            // wait for animation to end
            while((aniA.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
            {
                yield return null;
            }

            t = 0;
            do
            {
                t += speed * Time.deltaTime;
                tempPos = new Vector3(Mathf.Lerp(posA.x, posB.x, t), Mathf.Lerp(posA.y, posB.y, t), Mathf.Lerp(posA.z, posB.z, t));
                movableObject.transform.position = tempPos;
                yield return null;

            } while (Mathf.Clamp01(t) != 1);

            // wait for animation to end
            while ((aniB.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
            {
                yield return null;
            }
        }
        else if (startingPos == posB)
        {
            // wait for animation to end
            while ((aniB.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
            {
                yield return null;
            }

            t = 1;
            do
            {
                t -= speed * Time.deltaTime;
                tempPos = new Vector3(Mathf.Lerp(posA.x, posB.x, t), Mathf.Lerp(posA.y, posB.y, t), Mathf.Lerp(posA.z, posB.z, t));
                movableObject.transform.position = tempPos;
                yield return null;

            } while (Mathf.Clamp01(t) != 0);

            // wait for animation to end
            while ((aniA.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
            {
                yield return null;
            }
        }

        isTeleportingEnabled = true;
        movableObject.SetActive(true);
        usingElevator = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTeleportingEnabled = true;
            startingPos = Vector3.Distance(posA, collision.transform.position) < Vector3.Distance(posB, collision.transform.position) ? posA : posB;

            // triggering animation
            if (startingPos == posA)
            {
                aniA.SetTrigger("PlayerEnters");
            }
            else if (startingPos == posB)
            {
                aniB.SetTrigger("PlayerEnters");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTeleportingEnabled = false;
            if (usingElevator == false)
            {
                leavingPos = startingPos;
                // triggering animation
                if (leavingPos == posA)
                {
                    aniA.SetTrigger("PlayerLeaves");
                }
                else if (leavingPos == posB)
                {
                    aniB.SetTrigger("PlayerLeaves");
                }
            }
        }
    }

    public bool IsUsingElevator()
    {
        return usingElevator;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{

    public Vector3 posA;
    public Vector3 posB;
    public float speed = 1;

    private GameObject player;

    private Vector3 tempPos;
    private float t;
    private bool isTeleportingEnabled;
    private Vector3 startingPos;
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
            t = 0;
            do
            {
                t += speed * Time.deltaTime;
                tempPos = new Vector3(Mathf.Lerp(posA.x, posB.x, t), Mathf.Lerp(posA.y, posB.y, t), Mathf.Lerp(posA.z, posB.z, t));
                movableObject.transform.position = tempPos;
                yield return null;

            } while (Mathf.Clamp01(t) != 1);
        }
        else if (startingPos == posB)
        {
            t = 1;
            do
            {
                t -= speed * Time.deltaTime;
                tempPos = new Vector3(Mathf.Lerp(posA.x, posB.x, t), Mathf.Lerp(posA.y, posB.y, t), Mathf.Lerp(posA.z, posB.z, t));
                movableObject.transform.position = tempPos;
                yield return null;

            } while (Mathf.Clamp01(t) != 0);
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTeleportingEnabled = false;
        }
    }

    public bool IsUsingElevator()
    {
        return usingElevator;
    }

}

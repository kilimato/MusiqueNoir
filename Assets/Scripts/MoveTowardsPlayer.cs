using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MoveTowardsPlayer : MonoBehaviour
{
    public Seeker seeker;
    public AIPath aiPath;
    public AIDestinationSetter destSetter;

    public GameObject alertSign;
    public GameObject target;
    public GameObject startingPos;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        destSetter = GetComponent<AIDestinationSetter>();

        seeker.enabled = false;
        aiPath.enabled = false;
        destSetter.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, target.transform.position) > 10f)
        {
            /*
            seeker.enabled = false;
            aiPath.enabled = false;
            destSetter.enabled = false;
            */
            alertSign.SetActive(false);

            ReturnToStartPos();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCircle"))
        {
            alertSign.SetActive(true);

            seeker.enabled = true;
            aiPath.enabled = true;
            destSetter.enabled = true;

            destSetter.target = target.transform;

            animator.SetBool("Alerted", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    // not working right now 
    private void ReturnToStartPos()
    {
        destSetter.target = startingPos.transform;
    }

}

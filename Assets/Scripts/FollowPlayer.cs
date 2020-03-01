using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    private float x_offset = 6;
    private float y_offset = 4.5f;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(x_offset, y_offset, 0);
    }

    // Update is called once per frame
    void Update()
    {
       // transform.position = new Vector3(player.transform.position.x + x_offset, player.transform.position.y + y_offset, transform.position.z);
    }

    void LateUpdate()
    {

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -50f);
    }
}

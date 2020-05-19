// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    [SerializeField]
    private float x_offset = 6;
    [SerializeField]
    private float y_offset = 3f;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(x_offset, y_offset, 0);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + y_offset, -50f);
    }
}

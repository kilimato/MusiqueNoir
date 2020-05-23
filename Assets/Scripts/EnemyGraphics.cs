// @author Tapio Mylläri
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Makes sure that the enemy sprite is facing the right way, if the enemy has some velocity.
/// </summary>
public class EnemyGraphics : MonoBehaviour
{
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb2D.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-2f, 2f, 1f);
        }
        else if (rb2D.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(2f, 2f, 1f);
        }
    }
}

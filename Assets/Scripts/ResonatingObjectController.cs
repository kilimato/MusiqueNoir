using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonatingObjectController : MonoBehaviour
{
    private ParticleSystem particles;
    ParticleSystem.MainModule psMain;
    private float waitTime;
    private float timeAfterCollision = 3.0f;

    private float exposureStartTime;
    private float exposureTime = 100.0f;

    private float timer = 0;
    private float collisionExitTimer = 0;

    public float ringSize = 0.5f;
    public float ringMaxSize = 5f;
    private float ringStep = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        psMain = particles.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && (!IsInvoking("EmitParticles")))
        {
            InvokeRepeating("EmitParticles", 0, 0.5f);
            timer ++;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        timer++;
        if(timer > exposureTime)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        collisionExitTimer++;
        timer = 0;
    }

    private void EmitParticles()
    {
        if (collisionExitTimer != 0) collisionExitTimer++;
        if (collisionExitTimer > timeAfterCollision)
        {
            CancelInvoke("EmitParticles");
            Debug.Log("Stopped emitting after cooldown");
            collisionExitTimer = 0;
            ringSize = 0;
        }
        else
        {
            if (ringSize >= ringMaxSize)
            {
                ringSize = ringMaxSize;
            }
            else
            {
                ringSize += ringStep;
            }

            psMain.startSize = ringSize;
            particles.Emit(1);
        }
    }
}

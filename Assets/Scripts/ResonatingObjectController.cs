﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonatingObjectController : MonoBehaviour
{
    private ParticleSystem particles;
    ParticleSystem.MainModule psMain;

    private float minExitTime = 3.0f;
    private float maxExposureTime = 4.0f;

    public float exposureTimer = 0;
    public float exitTimer = 0;
    bool exposureTimerActive = false;
    bool exitCollisionTimerActive = false;

    public float ringSize = 2f;
    public float ringMaxSize = 6f;
    private float ringStep = 1f;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        psMain = particles.main;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         ei oo niin vaikeaa, oon vaan väsynyt
         eli meillä on timer, joka tsekkaa aikaa sekunneissa
         kun trigger -> timeri lähtee käyntiin, jos ylittää 4s, niin objekti tuhoutuu
         kun exitTrigger -> timeri nollaantuu, ja kun ylittää 3s, efekti loppuu
         */
        if (exposureTimerActive)
        {
            exposureTimer += Time.deltaTime;
            if (exposureTimer > maxExposureTime)
            {
                Destroy(gameObject);
            }
            Debug.Log(exposureTimer);
        }

        if (exitCollisionTimerActive)
        {
            exitTimer += Time.deltaTime;
            Debug.Log(exitTimer);
        }
    }


    /* Sent when another object enters a trigger collider attached to this object (2D physics only).
     * This message is sent to the trigger Collider2D and the Rigidbody2D (if any) that the trigger Collider2D belongs to, and to the Rigidbody2D (or the Collider2D if there is no Rigidbody2D) that touches the trigger.
     * Note: Trigger events are only sent if one of the Colliders also has a Rigidbody2D attached. Trigger events are sent to disabled MonoBehaviours, to allow enabling Behaviours in response to collisions.*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        exitCollisionTimerActive = false;
        exitTimer = 0;

        exposureTimerActive = true;
        Debug.Log("Entering");
        if (other.gameObject.CompareTag("Resonator") && (!IsInvoking("EmitParticles")))
        {
            InvokeRepeating("EmitParticles", 0, 0.5f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Staying");

        if (exposureTimer >= maxExposureTime)
        {
            // ei toimi, miksei mene tänne vaikka triggerstaytä kutsutaan joka framella?
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exiting");
        exposureTimerActive = false;
        exposureTimer = 0;

        exitCollisionTimerActive = true;
        exitTimer = 0;
    }

    public void EmitParticles()
    {
        if (exitTimer >= minExitTime)
        {
            CancelInvoke("EmitParticles");
            Debug.Log("Stopped emitting after cooldown");

            exitCollisionTimerActive = false;
            exitTimer = 0;
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
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResonatingObjectController : MonoBehaviour
{
    private ParticleSystem particles;
    ParticleSystem.MainModule psMain;

    private float minExitTime = 2.0f;
    private float maxExposureTime = 2.5f;

    public float exposureTimer = 0;
    public float exitTimer = 0;
    bool exposureTimerActive = false;
    bool exitCollisionTimerActive = false;

    public float ringSize = 2f;
    public float ringMaxSize = 6f;
    private float ringStep = 1f;

    private bool waves = false;

    Renderer rend;
    private Material mat;

    float waveAmount = 0f;


    // FMOD event
    [FMODUnity.EventRef]
    public string ResonanceEvent = "event:/resonating";
    public FMOD.Studio.EventInstance soundEvent;

    public float resonanceStartingIntensity = 0.0f;
    private float resonanceIntensity;

    FMOD.Studio.PARAMETER_ID soundParameterId;



    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        psMain = particles.main;

        mat = GetComponent<Renderer>().material;


        //FMOD sound event
        //manually starting sound event
        soundEvent = FMODUnity.RuntimeManager.CreateInstance(ResonanceEvent);
        resonanceIntensity = resonanceStartingIntensity;

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());


        FMOD.Studio.EventDescription resonanceEventDescription;
        soundEvent.getDescription(out resonanceEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION resonanceParameterDescription;
        resonanceEventDescription.getParameterDescriptionByName("ResoIntensity", out resonanceParameterDescription);
        soundParameterId = resonanceParameterDescription.id;


        soundEvent.setParameterByID(soundParameterId, resonanceIntensity);
        soundEvent.start();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMaterial();

        if (exposureTimerActive)
        {
            exposureTimer += Time.deltaTime;

            //Changing FMOD intensity parameter
            changeSoundIntensity();

            //Setting new
            if (exposureTimer > maxExposureTime)
            {
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
            Debug.Log(exposureTimer);
        }

        if (exitCollisionTimerActive)
        {
            exitTimer += Time.deltaTime;
            Debug.Log(exitTimer);

            //Changing FMOD intensity parameter
            changeSoundIntensity();
        }
    }

    private void UpdateMaterial()
    {
        if (waves && waveAmount < 20)
        {
            waveAmount += 0.1f;
            //mat.SetFloat("_RadialPush", testCount);
            mat.SetFloat("_SineFrequency", waveAmount);
            mat.SetFloat("_SineSpeed", waveAmount);
            mat.SetFloat("_SineAmplitude", waveAmount);
        }
        if (!waves && waveAmount > 0)
        {
            waveAmount -= 0.1f;
            //mat.SetFloat("_RadialPush", testCount);
            mat.SetFloat("_SineFrequency", waveAmount);
            mat.SetFloat("_SineSpeed", waveAmount);
            mat.SetFloat("_SineAmplitude", waveAmount);
        }
    }


    /* Sent when another object enters a trigger collider attached to this object (2D physics only).
     * This message is sent to the trigger Collider2D and the Rigidbody2D (if any) that the trigger Collider2D belongs to, and to the Rigidbody2D (or the Collider2D if there is no Rigidbody2D) that touches the trigger.
     * Note: Trigger events are only sent if one of the Colliders also has a Rigidbody2D attached. Trigger events are sent to disabled MonoBehaviours, to allow enabling Behaviours in response to collisions.*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (! other.IsTouching(GetComponent<EdgeCollider2D>()))
        {
            return;
        }
        exitCollisionTimerActive = false;
        exitTimer = 0;

        exposureTimerActive = true;
        Debug.Log("Entering");
        if (other.gameObject.CompareTag("Resonator") && (!IsInvoking("EmitParticles")))
        {
            InvokeRepeating("EmitParticles", 0, 0.5f);
            waves = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Staying");

        if (exposureTimer >= maxExposureTime)
        {

            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exiting");
        exposureTimerActive = false;
        exposureTimer = 0;
        waves = false;

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

    void OnDestroy()
    {
        soundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        soundEvent.release();
    }

    public void changeSoundIntensity()
    {
        resonanceIntensity = (exposureTimer / maxExposureTime) * 100;
        soundEvent.setParameterByID(soundParameterId, resonanceIntensity);
    }
}

// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for handling resonator's effects on objects in the game such as particle emitting 
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
    private Material mat;
    float waveAmount = 0f;


    // FMOD event
    [FMODUnity.EventRef]
    public string ResonanceEvent = "event:/SFX/resonating";
    public FMOD.Studio.EventInstance soundEvent;

    public float resonanceStartingIntensity = 0.0f;
    private float resonanceIntensity;
    private bool isSoundPlaying = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateMaterial();

        if (exposureTimerActive)
        {
            exposureTimer += Time.deltaTime;

            //Changing FMOD intensity parameter
            changeSoundIntensity();

            //Setting new
            if (exposureTimer > maxExposureTime)
            {
                //Stop the sound
                soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                soundEvent.release();
                isSoundPlaying = false;

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

    // in earlier versions object started "resonating", making wavy effect when player used resonator,
    // was later switched to outline shader instead
    private void UpdateMaterial()
    {
        if (waves && waveAmount < 20)
        {
            waveAmount += 0.1f;
            mat.SetFloat("_SineFrequency", waveAmount);
            mat.SetFloat("_SineSpeed", waveAmount);
            mat.SetFloat("_SineAmplitude", waveAmount);
        }
        if (!waves && waveAmount > 0)
        {
            waveAmount -= 0.1f;
            mat.SetFloat("_SineFrequency", waveAmount);
            mat.SetFloat("_SineSpeed", waveAmount);
            mat.SetFloat("_SineAmplitude", waveAmount);
        }
    }

    // we check if resonating collider is touching edge collider in the middle of the object
    // -> otherwise would start emitting particles too easily
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.IsTouching(GetComponent<EdgeCollider2D>()))
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
            //Stop sounds
            soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            soundEvent.release();
            isSoundPlaying = false;

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
            exitCollisionTimerActive = false;
            exitTimer = 0;
            ringSize = 0;

            //Stop the sound
            soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            soundEvent.release();
            isSoundPlaying = false;
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
        //Stop the sound
        soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        soundEvent.release();
        isSoundPlaying = false;
    }

    public void changeSoundIntensity()
    {
        if (maxExposureTime != 0) resonanceIntensity = (exposureTimer / maxExposureTime) * 100;
        if (exposureTimer > 0.1f)
        {
            if (!isSoundPlaying)
            {
                soundEvent.start();
                isSoundPlaying = true;
            }

            soundEvent.setParameterByID(soundParameterId, resonanceIntensity);
        }
        else if (isSoundPlaying)
        {
            //Stop the sound
            soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isSoundPlaying = false;
        }
    }

    // we reset object for saving purposes
    public void ResetObject()
    {
        CancelInvoke();

        exposureTimer = 0;
        exitTimer = 0;
        exposureTimerActive = false;
        exitCollisionTimerActive = false;
    }
}
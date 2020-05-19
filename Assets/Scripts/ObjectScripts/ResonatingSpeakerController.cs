// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// script for handling speaker resonating when player uses resonator:
// after resonating for a while, speaker enters active state, and stuns enemy when they enter speaker's trigger area
public class ResonatingSpeakerController : MonoBehaviour
{
    private ParticleSystem particles;
    ParticleSystem.MainModule psMain;
    ParticleSystem.ColorOverLifetimeModule colorModule;

    private float minExitTime = 1f;
    private float maxExposureTime = 1.5f;

    public float exposureTimer = 0;
    public float exitTimer = 0;
    bool exposureTimerActive = false;
    bool exitCollisionTimerActive = false;

    public float ringSize = 2f;
    public float ringMaxSize = 6f;
    private float ringStep = 1f;

    private bool waves = false;
    public bool speakerActive = false;
    private bool coroutineRunning = false;

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
        if (exposureTimerActive)
        {
            exposureTimer += Time.deltaTime;
            //Changing FMOD intensity parameter
            changeSoundIntensity();

            //Setting new
            if (exposureTimer > maxExposureTime)
            {
                CancelInvoke("EmitParticles");
                StopTimers();
                speakerActive = true;
                ActivateSpeaker();
            }
        }
        if (exitCollisionTimerActive)
        {
            exitTimer += Time.deltaTime;
            //Changing FMOD intensity parameter
            changeSoundIntensity();
        }
    }

    private void StopTimers()
    {
        exposureTimerActive = false;
        exitCollisionTimerActive = false;
        exposureTimer = 0;
        exitTimer = 0;
    }

    // speaker either resonates or stuns enemy (if speaker is in active state) when OnTriggerEnter is called
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Resonator") && speakerActive) return;

        if (other.gameObject.CompareTag("Resonator") && (!IsInvoking("EmitParticles")))
        {
            InvokeRepeating("EmitParticles", 0, 0.5f);
            exitCollisionTimerActive = false;
            exitTimer = 0;
            exposureTimerActive = true;
        }

        if (other.gameObject.CompareTag("Enemy") && speakerActive)
        {
            StopAllCoroutines();
            coroutineRunning = false;
            psMain.startSize = 10f;
            psMain.startSpeed = 5f;
            particles.Emit(1);

            speakerActive = false;
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.stateMachine.ChangeState(new StunnedState(enemy));
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (speakerActive) return;
        if (other.gameObject.CompareTag("Resonator"))
        {
            if (exposureTimer >= maxExposureTime)
            {
                CancelInvoke("EmitParticles");
                StopTimers();
                speakerActive = true;
                ActivateSpeaker();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (speakerActive) return;

        if (other.gameObject.CompareTag("Resonator"))
        {
            exposureTimerActive = false;
            exposureTimer = 0;
            exitCollisionTimerActive = true;
            exitTimer = 0;
        }
    }


    // speaker starts to emit certain sized particles when active, compared to growing/shrinking particle size when resonating
    private void ActivateSpeaker()
    {
        if (!coroutineRunning)
        {
            coroutineRunning = true;
            StartCoroutine(EmitActivatedParticles());
        }
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

    public IEnumerator EmitActivatedParticles()
    {
        while (true)
        {
            psMain.startSize = 3f;
            particles.Emit(1);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnDestroy()
    {
        soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        soundEvent.release();
        isSoundPlaying = false;
    }

    public void changeSoundIntensity()
    {
        if (maxExposureTime != 0) resonanceIntensity = (exposureTimer / maxExposureTime) * 100;
        if (exposureTimer > 0.1f || speakerActive)
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


    // we reset speaker for saving purposes
    public void ResetSpeaker()
    {
        CancelInvoke();

        exposureTimer = 0;
        exitTimer = 0;
        exposureTimerActive = false;
        exitCollisionTimerActive = false;

        speakerActive = false;
    }
}
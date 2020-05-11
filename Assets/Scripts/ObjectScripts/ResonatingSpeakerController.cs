using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //UpdateMaterial();

        if (exposureTimerActive)
        {
            exposureTimer += Time.deltaTime;

            //Changing FMOD intensity parameter
            changeSoundIntensity();

            //Setting new
            if (exposureTimer > maxExposureTime)
            {
                //Destroy(gameObject);
                //gameObject.SetActive(false);
                CancelInvoke("EmitParticles");
                StopTimers();
                speakerActive = true;
                ActivateSpeaker();
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

    private void StopTimers()
    {
        exposureTimerActive = false;
        exitCollisionTimerActive = false;
        exposureTimer = 0;
        exitTimer = 0;
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
        if (other.gameObject.CompareTag("Resonator") &&  speakerActive) return;
        Debug.Log("Entering");
     
        if (other.gameObject.CompareTag("Resonator") && (!IsInvoking("EmitParticles")))
        {
            InvokeRepeating("EmitParticles", 0, 0.5f);
            exitCollisionTimerActive = false;
            exitTimer = 0;

            exposureTimerActive = true;
            //waves = true;
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
            Debug.Log("Stunned enemy");
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (speakerActive) return;
        if (other.gameObject.CompareTag("Resonator")) {
            Debug.Log("Staying");

            if (exposureTimer >= maxExposureTime)
            {
                //Destroy(gameObject);
                //gameObject.SetActive(false);
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
            Debug.Log("Exiting");
            exposureTimerActive = false;
            exposureTimer = 0;
            //waves = false;

            exitCollisionTimerActive = true;
            exitTimer = 0;
        }
    }

    private void ActivateSpeaker()
    {
        if (!coroutineRunning)
        {
            Debug.Log("Activated speaker");
            coroutineRunning = true;
            //StartCoroutine(EmitActivationSignal());

            StartCoroutine(EmitActivatedParticles());
        }
    }

    public IEnumerator EmitActivationSignal()
    {
        int i = 0;
        while (i < 5)
        {
            colorModule.color = Color.white;
            particles.Emit(1);
            yield return new WaitForSeconds(0.3f);
        }

        //colorModule.color = Color.blue;
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
        soundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        soundEvent.release();
    }

    public void changeSoundIntensity()
    {
        resonanceIntensity = (exposureTimer / maxExposureTime) * 100;
        soundEvent.setParameterByID(soundParameterId, resonanceIntensity);
    }
}

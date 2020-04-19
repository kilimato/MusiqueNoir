using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ResonatingNPCController : MonoBehaviour
{
    private float minExitTime = 2.0f;
    private float maxExposureTime = 5f;

    private float exposureTimer = 0;
    private float exitTimer = 0;
    bool exposureTimerActive = false;
    bool exitCollisionTimerActive = false;

    private float ringSize = 2f;
    private float ringMaxSize = 6f;
    private float ringStep = 1f;

    private bool waves = false;

    Renderer rend;
    private Material mat;

    float waveAmount = 0f;

    public Color32 enlightenedColor;
    public SpriteRenderer spriteRenderer;
    public Light2D pointLight;


    /*
     Billboard particle systemin asetuksissa:
     default partikkeli ei ole pallo, vaikka näyttää siltä, vaan neliö, johon on piirretty ympyrä
     billboarding kääntää partikkelin aina kameraa kohti, jolloin näyttää että se olisi pyöreä
         */
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
        mat = GetComponent<Renderer>().material;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(70, 70, 70, 255);
        enlightenedColor = new Color32(70, 70, 70, 255);

        pointLight.enabled = false;

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

        if (exposureTimerActive)
        {
            exposureTimer += Time.deltaTime;

            //Changing FMOD intensity parameter
            changeSoundIntensity();

            //Setting new
            if (exposureTimer > maxExposureTime)
            {
                //Destroy(gameObject);
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


    /* Sent when another object enters a trigger collider attached to this object (2D physics only).
     * This message is sent to the trigger Collider2D and the Rigidbody2D (if any) that the trigger Collider2D belongs to, and to the Rigidbody2D (or the Collider2D if there is no Rigidbody2D) that touches the trigger.
     * Note: Trigger events are only sent if one of the Colliders also has a Rigidbody2D attached. Trigger events are sent to disabled MonoBehaviours, to allow enabling Behaviours in response to collisions.*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Resonator") && (!IsInvoking("EmitParticles")))
        {
            StartCoroutine(Coloring());
        }
    }

    IEnumerator Coloring()
    {
        while (enlightenedColor.r < 255)
        {
            enlightenedColor.r += 1;
            enlightenedColor.g += 1;
            enlightenedColor.b += 1;
            //brainwashedColor.a += 1;
            if (enlightenedColor.r >= 255)
            {
                pointLight.enabled = true;
            }
            spriteRenderer.color = enlightenedColor;
            Debug.Log("Color: " + enlightenedColor.r + ", " + enlightenedColor.g
            + ", " + enlightenedColor.b + ", SpriteRenderer color: " + spriteRenderer.color);
            yield return new WaitForSeconds(0.015f);
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

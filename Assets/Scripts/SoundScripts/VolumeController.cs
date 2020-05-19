// @author Jethro Liukku
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    private bool hasStarted = false;

    [FMODUnity.EventRef]
    public string VolumeEvent = "";
    public FMOD.Studio.EventInstance soundEvent;

    public float StartingVolume = 0.0f;
    float volume;

    FMOD.Studio.PARAMETER_ID soundParameterId;

    CircleCollider2D cachedParticleCollider;

    // Start is called before the first frame update
    void Start()
    {
        cachedParticleCollider = GetComponent<CircleCollider2D>();
        volume = StartingVolume;

        //manually start event
        soundEvent = FMODUnity.RuntimeManager.CreateInstance(VolumeEvent);
        //soundEvent.start();

        FMOD.Studio.EventDescription volumeEventDescription;
        soundEvent.getDescription(out volumeEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeEventDescription.getParameterDescriptionByName("Volume", out volumeParameterDescription);
        soundParameterId = volumeParameterDescription.id;
    }

    void OnDestroy()
    {
        StopAllMusicEvents();
        soundEvent.release();
    }

    void StopAllMusicEvents()
    {
        FMOD.Studio.Bus musicBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        musicBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            //Constant is caluclated from circlecolliders radious. 0.75x = 100, solve  x
            volume = cachedParticleCollider.radius * 133.333f;
            soundEvent.setParameterByID(soundParameterId, volume);
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                hasStarted = true;
                soundEvent.start();
            }
        }

    }
}
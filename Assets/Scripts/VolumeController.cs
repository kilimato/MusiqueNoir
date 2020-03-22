﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{

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
        soundEvent.start();

        FMOD.Studio.EventDescription volumeEventDescription;
        soundEvent.getDescription(out volumeEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION volumeParameterDescription;
        volumeEventDescription.getParameterDescriptionByName("Volume", out volumeParameterDescription);
        soundParameterId = volumeParameterDescription.id;
    }

    // Update is called once per frame
    void Update()
    {
        //volume = cachedParticleSystem.StartSize;

        //Constant is caluclated from circlecolliders radious. 0.75x = 100, solve  x
        volume = cachedParticleCollider.radius * 133.333f;
        soundEvent.setParameterByID(soundParameterId, volume);

    }
}
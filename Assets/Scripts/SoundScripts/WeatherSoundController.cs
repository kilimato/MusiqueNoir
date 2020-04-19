﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSoundController : MonoBehaviour
{
    private bool isRaining = true;
    private bool isOutside = true;
    private bool hasStarted = false;

    public GameObject outsideTilemaps;

    [FMODUnity.EventRef]
    public string weatherEvent = "";
    public FMOD.Studio.EventInstance weatherInstance;

    FMOD.Studio.PARAMETER_ID outsideParameterId;

    CircleCollider2D cachedParticleCollider;

    // Start is called before the first frame update
    void Start()
    {
        weatherInstance = FMODUnity.RuntimeManager.CreateInstance(weatherEvent);

        FMOD.Studio.EventDescription weatherEventDescription;
        weatherInstance.getDescription(out weatherEventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION outsideParameterDescription;
        weatherEventDescription.getParameterDescriptionByName("isOutside", out outsideParameterDescription);
        outsideParameterId = outsideParameterDescription.id;
    }

    void OnDestroy()
    {
        weatherInstance.release();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            isOutside = outsideTilemaps.activeInHierarchy;
            weatherInstance.setParameterByID(outsideParameterId, isOutside ? 1 : 0);
        }
        else if (isRaining)
        {
            hasStarted = true;
            weatherInstance.start();
        }
    }

    public void setOutside(bool b)
    {
        isOutside = b;
    }
}
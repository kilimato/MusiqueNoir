// @author Jethro Liukku
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundEvent : MonoBehaviour
{
    public GameObject rain;
    public GameObject insideTilemaps;

    [FMODUnity.EventRef]
    public string StepEvent = "";

    private bool isInside = false;
    private bool isRaining = true;
    public bool isWet = false;

    public void PlayStepSound()
    {
        setWetness();
        
        FMOD.Studio.EventInstance step = FMODUnity.RuntimeManager.CreateInstance(StepEvent);
        step.setParameterByName("Wet", isWet ? 1 : 0);
        step.setParameterByName("isRunning", 0);
        step.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        step.start();
        step.release();
    }

    public void PlayRunSound()
    {
        setWetness();
        FMOD.Studio.EventInstance step = FMODUnity.RuntimeManager.CreateInstance(StepEvent);
        step.setParameterByName("Wet", isWet ? 1 : 0);
        step.setParameterByName("isRunning", 1);
        step.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        step.start();
        step.release();
    }

    public void setWetness()
    {
        rain = GameObject.FindWithTag("Rain");
        insideTilemaps = GameObject.FindWithTag("InsideTilemaps");

        if (rain == null)
        {
            isRaining = false;
        }
        else
        {
            isRaining = rain.activeInHierarchy;

        }

        if (insideTilemaps == null)
        {
            isInside = false;
        }
        else
        {
            isInside = insideTilemaps.activeInHierarchy;

        }
        isWet = (!isInside && isRaining);
    }
}
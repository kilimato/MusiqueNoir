using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string soundEvent1 = "";

    [FMODUnity.EventRef]
    public string soundEvent2 = "";


    public void play1()
    {
        FMOD.Studio.EventInstance soundInstance1 = FMODUnity.RuntimeManager.CreateInstance(soundEvent1);
        soundInstance1.start();
        soundInstance1.release();
    }

    public void play2()
    {
        FMOD.Studio.EventInstance soundInstance2 = FMODUnity.RuntimeManager.CreateInstance(soundEvent2);
        soundInstance2.start();
        soundInstance2.release();
    }
}

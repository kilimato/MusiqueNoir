using UnityEngine;
using System.Collections;

public class SoundEvent : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string Event = "";

    public void PlaySound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Event, transform.position);
    }
}

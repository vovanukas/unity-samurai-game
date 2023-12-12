using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODSoundEvent : MonoBehaviour
{
    public FMODUnity.EventReference eventSound;
    public void PlaySound()
    {
        if (!eventSound.IsNull)
        {
            FMODUnity.RuntimeManager.PlayOneShot(eventSound, this.transform.position);
        }
        else
        {
            Debug.LogError("EventSound is null");
        }
    }
}

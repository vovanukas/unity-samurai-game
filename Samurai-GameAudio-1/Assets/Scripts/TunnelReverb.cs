using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelReverb : MonoBehaviour
{
    void OnTriggerEnter()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("IsInTunnel", 1f);
    }

    void OnTriggerExit()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("IsInTunnel", 0f);
    }
}

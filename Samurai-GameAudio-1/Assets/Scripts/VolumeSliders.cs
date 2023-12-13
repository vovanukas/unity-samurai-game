using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    FMOD.Studio.Bus bus;
    Slider slider;
    void Awake()
    {
        bus = FMODUnity.RuntimeManager.GetBus(gameObject.name);
        slider = gameObject.GetComponent<Slider>();

        float busVolume;
        bus.getVolume(out busVolume);

        slider.value = busVolume;
    }

    // Update is called once per frame
    public void VolumeController(System.Single newVolume)
    {
        bus.setVolume(newVolume);
    }
}

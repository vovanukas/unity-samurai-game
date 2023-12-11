using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [Header("FMOD Events")]
    public FMODUnity.EventReference musicEvent;
    public FMOD.Studio.EventInstance musicEventInstance;

    [Header("Music Controllers")]
    public GameObject mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.StudioSystem.getParameterByName("IsInMainMenu", out float isPlayerInMainMenu) ;
        if (isPlayerInMainMenu == 0)
        {
            musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(musicEventInstance, this.transform);
            musicEventInstance.start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

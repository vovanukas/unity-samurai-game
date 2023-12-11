using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter eventEmitter;
    public bool onInteract;

    // Update is called once per frame
    void OnTriggerStay(Collider player)
    {
        if (onInteract)
        {
            if (player.gameObject.tag == "Player")
            {
                if (Input.GetKeyDown(KeyCode.E) && !eventEmitter.IsPlaying())
                {
                    eventEmitter.SendMessage("Play");
                }
            }
        }
    }

    void OnTriggerEnter(Collider player)
    {
        if (!onInteract)
        {
            if (player.gameObject.tag == "Player")
            {
                if (!eventEmitter.IsPlaying())
                {
                    eventEmitter.Play();
                }
            }
        }
    }

    void OnTriggerExit(Collider player)
    {
        if (!onInteract)
        {
            if (player.gameObject.tag == "Player")
            {
                eventEmitter.Stop();
            }
        }
    }
}

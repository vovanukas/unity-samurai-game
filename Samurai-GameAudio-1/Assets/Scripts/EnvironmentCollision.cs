using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCollision : MonoBehaviour
{
    FootstepAudio FootstepAudio;
    void Start()
    {
        FootstepAudio = GameObject.FindWithTag("Player").GetComponent<FootstepAudio>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player In Water");
            FootstepAudio.isInWater = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Left Water");
            FootstepAudio.isInWater = false;
        }
    }


}

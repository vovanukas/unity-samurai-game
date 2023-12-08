using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class EnvironmentCollision : MonoBehaviour
{
    [Header("FMOD")]
    public FMODUnity.EventReference environmentEvent;
    public FMOD.Studio.EventInstance environmentEventInstance;

    public float soundActiveTimer;

    string[] environmentType;

    string environmentTypeFMODParameter = "EnvironmentType";
    string playerMovementFMODParameter = "PlayerMovement";
    string environmentDepthFMODParameter = "WaterDepth";

    FMOD.Studio.PARAMETER_ID environmentTypeID, playerMovementID, environmentDepthID;

    bool isInEnvironment = false;

    GameObject playerObject, cube;
    Rigidbody playerRB;
    Animator animator;

    void Start()
    {
        playerObject = GameObject.Find("ThirdPersonController");
        playerRB = playerObject.GetComponent<Rigidbody>();

        FMOD.Studio.EventDescription environmentEventDescription = FMODUnity.RuntimeManager.GetEventDescription(environmentEvent);


        FMOD.Studio.PARAMETER_DESCRIPTION environmentTypePD;
        environmentEventDescription.getParameterDescriptionByName(environmentTypeFMODParameter, out environmentTypePD);
        environmentTypeID = environmentTypePD.id;
        environmentType = GetParameterLabelsNames(environmentTypePD, environmentEventDescription);

        FMOD.Studio.PARAMETER_DESCRIPTION playerMovementPD;
        environmentEventDescription.getParameterDescriptionByName(playerMovementFMODParameter, out playerMovementPD);
        playerMovementID = playerMovementPD.id;

        FMOD.Studio.PARAMETER_DESCRIPTION environmentDepthPD;
        environmentEventDescription.getParameterDescriptionByName(environmentDepthFMODParameter, out environmentDepthPD);
        environmentDepthID = environmentDepthPD.id;

        animator = playerObject.GetComponent<Animator>();
    }

    void Update()
    {
        Timer();
    }

    bool soundActive = false;
    void PlaySound(float environmentTypeFloat, float playerMovementTypeFloat, float environmentDepthFloat)
    {
        if (!soundActive)
        {
            environmentEventInstance = FMODUnity.RuntimeManager.CreateInstance(environmentEvent);

            environmentEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerObject));

            environmentEventInstance.setParameterByID(environmentTypeID, environmentTypeFloat);
            environmentEventInstance.setParameterByID(playerMovementID, playerMovementTypeFloat);
            environmentEventInstance.setParameterByID(environmentDepthID, environmentDepthFloat);

            environmentEventInstance.start();
            environmentEventInstance.release();

            soundActive = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInEnvironment = true;
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Airborne"))
                PlaySound(GetFloatForParameterLabel(environmentType, this.gameObject.tag), 0, GetEnvironmentDepthAtPlayerLocation());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInEnvironment = false;
            PlaySound(GetFloatForParameterLabel(environmentType, this.gameObject.tag), 1, GetEnvironmentDepthAtPlayerLocation());
            Debug.Log("Player Left Water");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (GetPlayerVelocity() > 3f)
                PlaySound(GetFloatForParameterLabel(environmentType, this.gameObject.tag), 2, GetEnvironmentDepthAtPlayerLocation());
        }
    }
    float GetEnvironmentDepthAtPlayerLocation()
    {
        RaycastHit hit;
        Vector3 playerPositionAtWaterHeight = playerObject.transform.position;
        playerPositionAtWaterHeight.y = this.transform.position.y - 0.01f;
        if (Physics.Raycast(playerPositionAtWaterHeight, Vector3.down, out hit))
        {
            Debug.Log(hit.distance);
            Debug.Log(hit.collider.gameObject.name);
            return hit.distance;
        }
        return -1;
    }


    Vector3 v;
    float GetPlayerVelocity()
    {
        v.x = Mathf.Abs(playerRB.velocity.x);
        v.z = Mathf.Abs(playerRB.velocity.z);
        v.y = Mathf.Abs(playerRB.velocity.y);

        return v.x + v.z + v.y;
    }

    float GetFloatForParameterLabel(string[] arr, string tag)
    {
        float f = -1;
        foreach (string label in arr)
        {
            if (tag == label)
            {
                f = Array.IndexOf(arr, label);
                return f;
            }
        }

        return f;
    }

    string[] GetParameterLabelsNames(FMOD.Studio.PARAMETER_DESCRIPTION parameterDescription, FMOD.Studio.EventDescription eventDescription)
    {
        List<string> output = new List<string>();

        for (int i = 0; i <= Convert.ToInt32(parameterDescription.maximum); i++)
        {
            string label;
            eventDescription.getParameterLabelByID(parameterDescription.id, i, out label);
            output.Add(label);
        }

        return output.ToArray();
    }

    void Timer()
    {
        if (soundActive)
        {
            soundActiveTimer += (1 * Time.deltaTime);
            if (soundActiveTimer > 0.5f)
            {
                soundActive = false;
                soundActiveTimer = 0f;
            }
        }
    }


}

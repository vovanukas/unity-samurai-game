using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FootstepAudio : MonoBehaviour
{
    [Header("FMOD")]
    public FMODUnity.EventReference footstepEvent;
    public FMOD.Studio.EventInstance footstepEventInstance;

    // Parameter IDs
    string surfaceTypeFMODParameter = "SurfaceType";
    string movementTypeFMODParameter = "MovementType";

    FMOD.Studio.PARAMETER_ID surfaceTypeID, movementTypeID;

    #region GameObjects and Physics

    GameObject playerObject;
    Rigidbody playerRB;

    // Velocity
    Vector3 v;
    [SerializeField]
    float playerVelocity;

    // Footsteps Game Object
    GameObject footstepObject;
    Transform footstepObjectTransform;
    #endregion

    [SerializeField]
    string[] surfaceType = { "Grass", "Gravel", "Stone" };
    [SerializeField]
    string terrainTag;

    [Header("Footstep Variables")]
    float movementTypeFloat = 0f;
    float surfaceTypeFloat = 0f;
    public bool footstepActive, spacePressed, onAccidentalFallCooldown;
    public float resetThreshold;
    public float movementGate = 0f;
    bool jumpActive = false;



    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        ObjectSetup();
        FMODSetup();
        animator = GetComponent<Animator>();

    }

    void GetPlayerVelocity()
    {
        v.x = Mathf.Abs(playerRB.velocity.x);
        v.z = Mathf.Abs(playerRB.velocity.z);

        playerVelocity = v.x + v.z;
    }

    void ObjectSetup()
    {
        playerObject = GameObject.Find("ThirdPersonController");
        playerRB = playerObject.GetComponent<Rigidbody>();

        footstepObject = GameObject.Find("FootstepEmitter");
        footstepObjectTransform = footstepObject.transform;
    }

    void FMODSetup()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footstepEventInstance, footstepObjectTransform);
        FMOD.Studio.EventDescription footstepEventDescription = FMODUnity.RuntimeManager.GetEventDescription(footstepEvent);

        // Parameter Setup

        FMOD.Studio.PARAMETER_DESCRIPTION surfaceTypePD;
        footstepEventDescription.getParameterDescriptionByName(surfaceTypeFMODParameter, out surfaceTypePD);
        surfaceTypeID = surfaceTypePD.id;

        FMOD.Studio.PARAMETER_DESCRIPTION movementTypePD;
        footstepEventDescription.getParameterDescriptionByName(movementTypeFMODParameter, out movementTypePD);
        movementTypeID = movementTypePD.id;
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerVelocity();
        RayCastSwitch();
        MovementTypeSwitch();
        Timer();
        IsFalling();
    }

    void IsFalling()
    {
        if (Input.GetKey(KeyCode.Space))
            spacePressed = true;

        if (!jumpActive && spacePressed && animator.GetNextAnimatorStateInfo(0).IsName("Airborne"))
        {
            PlayJump(3f);

            jumpActive = true;
        }

        else if (!jumpActive && !onAccidentalFallCooldown && animator.GetNextAnimatorStateInfo(0).IsName("Airborne"))
        {
            PlayJump(4f);

            jumpActive = true;
            onAccidentalFallCooldown = true;
        }

        if (jumpActive && animator.IsInTransition(0) && !animator.GetNextAnimatorStateInfo(0).IsName("Airborne"))
        {
            PlayJump(5f);

            jumpActive = false;
        }
    }

    void RayCastSwitch()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            terrainTag = hit.collider.gameObject.tag;
        }

        foreach (string surface in surfaceType)
        {
            if (terrainTag == surface)
            {
                float f = Array.IndexOf(surfaceType, surface);
                surfaceTypeFloat = f;
            }
        }
    }

    void MovementTypeSwitch()
    {
        if (playerVelocity > 3.5f)
            movementTypeFloat = 2f;

        if (Input.GetKey(KeyCode.LeftShift))
            movementTypeFloat = 1f;
        
        if (Input.GetKey(KeyCode.C))
            movementTypeFloat = 0f;
    }

    [SerializeField]
    float footstepTimer, spaceTimer, accidentalFallCooldownTimer = 0f;
    void Timer()
    {
        if (footstepActive)
        {
            footstepTimer += (1 * Time.deltaTime);
            if (footstepTimer > resetThreshold)
            {
                footstepActive = false;
                footstepTimer = 0f;
            }
        }

        if (spacePressed)
        {
            spaceTimer += (1 * Time.deltaTime);
            if (spaceTimer > 0.5)
            {
                spacePressed = false;
                spaceTimer = 0f;
            }
        }

        if (onAccidentalFallCooldown)
        {
            accidentalFallCooldownTimer += (1 * Time.deltaTime);
            if (accidentalFallCooldownTimer > 3f)
            {
                onAccidentalFallCooldown = false;
                accidentalFallCooldownTimer = 0f;
            }
        }
    }

    void PlayFootstep(string movementType)
    {
        if (!footstepActive)
        {
            if (playerVelocity > movementGate)
            {
                footstepEventInstance = FMODUnity.RuntimeManager.CreateInstance(footstepEvent);

                footstepEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(footstepObject));

                footstepEventInstance.setParameterByID(surfaceTypeID, surfaceTypeFloat);
                footstepEventInstance.setParameterByID(movementTypeID, movementTypeFloat);

                footstepEventInstance.start();
                footstepEventInstance.release();

                footstepActive = true;
            }
        }
    }

    void PlayJump(float movementTypeFloat)
    {   
        footstepEventInstance = FMODUnity.RuntimeManager.CreateInstance(footstepEvent);

        footstepEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(footstepObject));

        footstepEventInstance.setParameterByID(surfaceTypeID, surfaceTypeFloat);
        footstepEventInstance.setParameterByID(movementTypeID, movementTypeFloat);

        footstepEventInstance.start();
        footstepEventInstance.release();

        footstepActive = true;
    }
}




    // public string jumpCoroutineStatus;
    // IEnumerator jumpCoroutine()
    // {
    //     Debug.Log("Started Jump Coroutine");
    //     jumpCoroutineStatus = "Started";

    //     yield return new WaitForSeconds(0.2f);
    //     footstepEventInstance = FMODUnity.RuntimeManager.CreateInstance(footstepEvent);

    //     footstepEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(footstepObject));

    //     footstepEventInstance.setParameterByID(movementTypeID, movementTypeFloat);
    //     footstepEventInstance.setParameterByID(surfaceTypeID, surfaceTypeFloat);

    //     footstepEventInstance.start();
    //     footstepEventInstance.release();

    //     jumpCoroutineStatus = "Finished";

    //     yield break;
    // }

    // IEnumerator destroyInstance(FMOD.Studio.EventInstance eventInstance)
    // {
    //     yield return new WaitForSeconds(0.3f);
    //     eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //     yield break;
    // }

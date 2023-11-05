using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]

public class FollowCamera : MonoBehaviour
{
    public SplineContainer spline;
    public GameObject fmodObject;

    private GameObject listener;

    void Start()
    {
        // Find the active listener with the "Listener" tag
        listener = GameObject.FindGameObjectWithTag("Listener");

        if (!listener)
        {
            Debug.LogError("No GameObject with the 'Listener' tag found!");
        }
    }

    void Update()
    {
        if (fmodObject && listener)
        {
            Transform splineTransform = spline.transform;
            float3 cameraPositionLocalToSpline = splineTransform.InverseTransformPoint(listener.transform.position);


            SplineUtility.GetNearestPoint(spline.Spline, cameraPositionLocalToSpline, out float3 nearest, out float t);

            nearest = splineTransform.TransformPoint(nearest);
            fmodObject.transform.position = nearest;
        }
    }
}
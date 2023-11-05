using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[RequireComponent(typeof(SplineContainer))]

public class AudioAreaSpline : MonoBehaviour
{
    public SplineContainer splineContainer;
    public GameObject fmodObject;

    private GameObject listener;
    private Bounds splineBounds;
    
    public static bool IsInsideSpline(float3 point, SplineContainer splineContainer, Bounds splineBounds, out Vector3 nearestPointInSpline)
    {
        Vector3 pointPositionLocalToSpline = splineContainer.transform.InverseTransformPoint(point);

        SplineUtility.GetNearestPoint(splineContainer.Spline, pointPositionLocalToSpline, out var splinePoint, out var t);
        splinePoint.y = pointPositionLocalToSpline.y;
        // splinePoint = splineContainer.transform.TransformPoint(splinePoint);

        if(Vector3.Distance(point, splineContainer.transform.TransformPoint(splineBounds.center)) < Vector3.Distance(splinePoint, splineBounds.center))
        {
            // If point is inside of the spline...
            nearestPointInSpline = point;
            return true;
        }
        else
        {
            nearestPointInSpline = splineContainer.transform.TransformPoint(splinePoint);
            return false;
        }
        // return Vector3.Distance(point, splineBounds.center) < Vector3.Distance(splinePoint, splineBounds.center);
    }

    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.FindGameObjectWithTag("Listener");

        if (!listener)
        {
            Debug.LogError("No GameObject with the 'Listener' tag found!");
            return;
        }

        splineBounds = SplineUtility.GetBounds(splineContainer.Spline);
    }

    // Update is called once per frame
    void Update()
    {
        if (fmodObject && listener && splineContainer.Spline.Closed)
        {
            IsInsideSpline(listener.transform.position, splineContainer, splineBounds, out Vector3 nearestPointInSpline);

            fmodObject.transform.position = nearestPointInSpline;
        }
    }
}

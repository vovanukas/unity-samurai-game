/* NOTE: This whole thing will probably look better in an interface */
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
    public bool hasWind;

    private GameObject listener;
    private Bounds splineBounds;
    private FMODUnity.StudioEventEmitter windInstance;
    
    public static bool IsInsideSpline(float3 point, SplineContainer splineContainer, Bounds splineBounds, out Vector3 nearestPointInSpline)
    {
        Vector3 pointPositionLocalToSpline = splineContainer.transform.InverseTransformPoint(point);

        SplineUtility.GetNearestPoint(splineContainer.Spline, pointPositionLocalToSpline, out var splinePoint, out var t);
        splinePoint.y = pointPositionLocalToSpline.y;

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
    }

    // On AreaExit/Enter are called from the childs FMODEventEmitters GameObjects AreaSplineCollisionDetection Component.
    public void OnAreaExit(Collider other)
    {
        windInstance.SetParameter("Area Has Wind", System.Convert.ToSingle(true)); //default = there should be wind.
    }

    public void OnAreaEnter(Collider other)
    {
        windInstance.SetParameter("Area Has Wind", System.Convert.ToSingle(hasWind));
    }

    // Start is called before the first frame update
    void Start()
    {
        listener = GameObject.FindGameObjectWithTag("Listener");
        windInstance = listener.transform.Find("2D Wind").GetComponent<FMODUnity.StudioEventEmitter>();

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

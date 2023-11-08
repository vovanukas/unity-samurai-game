using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSplineCollisionDetection : MonoBehaviour
{

    public AudioAreaSpline AudioAreaSpline;

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            AudioAreaSpline.OnAreaEnter(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            AudioAreaSpline.OnAreaExit(other);

        }
    }
}

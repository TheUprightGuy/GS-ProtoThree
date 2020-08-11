using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WhaleData", menuName = "InfoObjects/WhaleData")]
public class WhaleInfo : ScriptableObject
{
    public bool leashed = false;
    public GameObject whale;
    public GameObject target;

    public void ResetOnPlay()
    {
        leashed = false;
        whale = null;
        target = null;
    }

    public void ToggleLeashed(bool _toggle)
    {
        leashed = _toggle;
    }

    public void GetAngle()
    {
        Vector3 dir = whale.transform.position - target.transform.position;

    }
}

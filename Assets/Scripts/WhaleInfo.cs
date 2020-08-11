using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WhaleData", menuName = "InfoObjects/WhaleData")]
public class WhaleInfo : ScriptableObject
{
    public bool leashed = false; 

    public void ResetOnPlay()
    {
        leashed = false;
    }

    public void ToggleLeashed(bool _toggle)
    {
        leashed = _toggle;
    }
}

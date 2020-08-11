using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassTarget : MonoBehaviour
{
    WhaleInfo whaleInfo;
    // Start is called before the first frame update
    void Start()
    {
        whaleInfo = CallbackHandler.instance.whaleInfo;
        whaleInfo.target = this.gameObject;
    }
}

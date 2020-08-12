using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private WhaleInfo whaleInfo;

    // Start is called before the first frame update
    void Start()
    {
        whaleInfo = CallbackHandler.instance.whaleInfo;
        whaleInfo.target = gameObject;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLineScript : MonoBehaviour
{
    public GameObject lure;
    LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, line.transform.position);
        line.SetPosition(1, lure.transform.position);
    }
}

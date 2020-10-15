using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorScript : MonoBehaviour
{
    public bool rotatingLocks;
    public bool slidingDoors;
    public float rotato;
    public DoorRotationScript door1;
    public DoorRotationScript door2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RotateLocks();
        }
    }

    public void RotateLocks()
    {
        door1.rotatingLocks = true;
        door2.rotatingLocks = true;
    }
}

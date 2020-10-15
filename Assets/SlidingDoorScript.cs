﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorScript : MonoBehaviour
{
    public DoorRotationScript door1;
    public DoorRotationScript door2;

    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.openDoors += OpenDoors;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.openDoors -= OpenDoors;
    }
    #endregion Callbacks

    public void OpenDoors()
    {
        door1.rotatingLocks = true;
        door2.rotatingLocks = true;
    }
}

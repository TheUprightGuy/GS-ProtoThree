﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishingCamController : MonoBehaviour
{
    public GameObject menuVCam;
    public GameObject menuToEstablishingBlendList;
    public GameObject establishingShotBlendList;
    // Start is called before the first frame update
    void Start()
    {
        EventHandler.instance.menuClosed += OnMenuClosed;
        EventHandler.instance.startEstablishingShot += OnStartEstablishingShot;
        EventHandler.instance.endEstablishingShot += OnEndEstablishingShot;
        EventHandler.instance.menuOpened += OnMenuOpened;
        EventHandler.instance.resumePressed += OnResume;
    }

    private void OnDestroy()
    {
    }

    private void OnMenuClosed()
    {
        menuToEstablishingBlendList.SetActive(true);
        EventHandler.instance.gameState.inCinematic = true;
    }

    private void OnResume()
    {
        menuVCam.SetActive(false);
    }
    
    private void OnMenuOpened()
    {
        menuToEstablishingBlendList.SetActive(false);
        establishingShotBlendList.SetActive(false);
        menuVCam.SetActive(true);
    }
    
    private void OnStartEstablishingShot()
    {
        establishingShotBlendList.SetActive(true);
        menuVCam.SetActive(false);
        menuToEstablishingBlendList.SetActive(false);
        EventHandler.instance.gameState.inCinematic = true;
    }
    
    private void OnEndEstablishingShot()
    {
        establishingShotBlendList.SetActive(false);
        EventHandler.instance.gameState.inCinematic = false;
        EventHandler.instance.menuOpened -= OnMenuOpened;
        EventHandler.instance.menuClosed -= OnMenuClosed;
        EventHandler.instance.startEstablishingShot -= OnStartEstablishingShot;
        EventHandler.instance.endEstablishingShot -= OnEndEstablishingShot;
    }
}

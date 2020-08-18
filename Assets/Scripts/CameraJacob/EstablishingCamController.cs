using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EstablishingCamController : MonoBehaviour
{
    #region Singleton
    public static EstablishingCamController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one EstablishingCamController exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            OnAwake();
        }
    }
    #endregion Singleton
    public GameObject menuVCam;
    public GameObject menuToGameViewBlendList;
    public GameObject establishingShotBlendList;
    public Transform whaleTransform;
    public List<Transform> objectivesSorted;
    public int currentObjectiveIndex;

    public void OnAwake()
    {
        currentObjectiveIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventHandler.instance.menuClosed += OnMenuClosed;
        EventHandler.instance.startEstablishingShot += OnStartEstablishingShot;
        EventHandler.instance.endEstablishingShot += OnEndEstablishingShot;
        EventHandler.instance.menuOpened += OnMenuOpened;
        EventHandler.instance.resumePressed += OnResume;
        EventHandler.instance.gameStart += OnGameStart;
    }
    // Remember to add cleanup for callbacks
    private void OnDestroy()
    {
        EventHandler.instance.menuClosed -= OnMenuClosed;
        EventHandler.instance.startEstablishingShot -= OnStartEstablishingShot;
        EventHandler.instance.endEstablishingShot -= OnEndEstablishingShot;
        EventHandler.instance.menuOpened -= OnMenuOpened;
        EventHandler.instance.resumePressed -= OnResume;
        EventHandler.instance.gameStart -= OnGameStart;
    }

    private void OnMenuClosed()
    {
        menuVCam.SetActive(false);
        menuToGameViewBlendList.SetActive(true);
    }

    private void OnGameStart()
    {
        menuToGameViewBlendList.SetActive(false);
        EventHandler.instance.menuOpened -= OnMenuOpened;
        EventHandler.instance.menuClosed -= OnMenuClosed;
    }

    private void OnResume()
    {
        menuVCam.SetActive(false);
    }
    
    private void OnMenuOpened()    //Not used currently
    {
        menuToGameViewBlendList.SetActive(false);
        establishingShotBlendList.SetActive(false);
        menuVCam.SetActive(true);
    }
    
    private void OnStartEstablishingShot()
    {
        //Set quest objective
        CallbackHandler.instance.SetQuestObjective(objectivesSorted[currentObjectiveIndex].gameObject);
        
        var lookAtObj = 
            establishingShotBlendList.GetComponent<CinemachineBlendListCamera>().ChildCameras[1];
        lookAtObj.Follow = whaleTransform;
        lookAtObj.LookAt = objectivesSorted[currentObjectiveIndex];
        
        var moveToObj = 
            establishingShotBlendList.GetComponent<CinemachineBlendListCamera>().ChildCameras[2];
        moveToObj.Follow = objectivesSorted[currentObjectiveIndex];
        moveToObj.LookAt = objectivesSorted[currentObjectiveIndex];
        
        establishingShotBlendList.SetActive(true);
        EventHandler.instance.gameState.inCinematic = true;
    }
    
    private void OnEndEstablishingShot()
    {
        establishingShotBlendList.SetActive(false);
        EventHandler.instance.gameState.inCinematic = false;
    }
}

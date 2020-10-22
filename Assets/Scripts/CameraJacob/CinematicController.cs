using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    #region Singleton
    public static CinematicController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one CinematicController exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            OnAwake();
        }
    }
    #endregion Singleton

    [Header("Solo VCam's for cinematics")]
    //First cam should always be menuCam
    public List<CinemachineVirtualCamera> VCams;
    [Header("Cinematic Blend List Cameras")]
    public List<CinemachineBlendListCamera> BlendLists;
    public Transform whaleTransform;
    //Objectives order (object of focus in cinematic):
    //0 is Shop objective
    //1 is island objective
    //2 is final objective
    //3 is switch puzzle objective
    //4 is opened door
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
        VCams[0].m_Priority = 0;
    }

    private void OnGameStart()
    {
        EventHandler.instance.menuOpened -= OnMenuOpened;
        EventHandler.instance.menuClosed -= OnMenuClosed;
    }

    private void OnResume()
    {
        VCams[0].m_Priority = 0;
    }
    
    private void OnMenuOpened()
    {
        BlendLists[0].gameObject.SetActive(false);
        VCams[0].m_Priority = 11;
    }
    
    private void OnStartEstablishingShot()
    {
        //Set quest objective
        CallbackHandler.instance.SetQuestObjective(objectivesSorted[currentObjectiveIndex].gameObject);
        
        var lookAtObj = 
            BlendLists[0].ChildCameras[1];
        lookAtObj.Follow = whaleTransform;
        lookAtObj.LookAt = objectivesSorted[currentObjectiveIndex];
        
        var moveToObj = 
            BlendLists[0].GetComponent<CinemachineBlendListCamera>().ChildCameras[2];
        moveToObj.Follow = objectivesSorted[currentObjectiveIndex];
        moveToObj.LookAt = objectivesSorted[currentObjectiveIndex];
        
        BlendLists[0].gameObject.SetActive(true);
        EventHandler.instance.gameState.inCinematic = true;
    }
    
    private void OnEndEstablishingShot()
    {
        BlendLists[0].gameObject.SetActive(false);
        EventHandler.instance.gameState.inCinematic = false;
    }
}

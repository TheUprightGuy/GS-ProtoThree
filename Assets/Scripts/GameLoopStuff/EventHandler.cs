using System;
using System.Collections;
using Audio;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    #region Singleton
    public static EventHandler instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one EventHandler in scene!");
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);
        OnAwake();
    }

    

    #endregion
    
    public GameState gameState;
    public float establishingShotDuration;
    public float menuToEstablishingDuration;
    public Action menuClosed;
    public Action startEstablishingShot;
    public Action endEstablishingShot;
    public Action menuOpened;
    public GameObject stageEndCanvas;
    public Action resumePressed;

    private void OnAwake()
    {
        gameState.inMenu = true;
        gameState.gamePaused = true;
        gameState.inCinematic = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameState.inCinematic)
        {
            gameState.inMenu = true;
            gameState.gamePaused = true;
            menuOpened?.Invoke();
        }
    }

    public void OnPlayPressed()
    {
        Debug.Log("Starting shot");
        gameState.inMenu = false;
        StartCoroutine(MenuToEstablishingShot());
        menuClosed?.Invoke();
    }
    
    private IEnumerator MenuToEstablishingShot()
    {
        yield return new WaitForSeconds(menuToEstablishingDuration);
        StartCoroutine(WaitForEstablishingShot());
        startEstablishingShot?.Invoke();
    }

    private IEnumerator WaitForEstablishingShot()
    {
        yield return new WaitForSeconds(establishingShotDuration);
        Debug.Log("EstablishingShotOver setting paused to false");
        gameState.gamePaused = false;
        endEstablishingShot?.Invoke();
    }

    public void OnEndTriggered()
    {
        stageEndCanvas.SetActive(true);
    }
}

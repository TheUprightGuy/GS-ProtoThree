using System;
using System.Collections;
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
    public Action startEstablishingShot;
    public Action play;
    public GameObject stageEndCanvas;

    private void OnAwake()
    {
        gameState.inMenu = true;
        gameState.gamePaused = true;
    }

    private void Update()
    {
        //Temp remove when menu's complete
        if (Input.GetMouseButtonDown(0))
        {
            OnPlayPressed();
        }
    }

    public void OnPlayPressed()
    {
        Debug.Log("Starting shot");
        gameState.inMenu = false;
        StartCoroutine(WaitForEstablishingShot());
        startEstablishingShot?.Invoke();
    }

    private IEnumerator WaitForEstablishingShot()
    {
        yield return new WaitForSeconds(establishingShotDuration);
        gameState.gamePaused = false;
        play?.Invoke();
    }

    public void OnEndTriggered()
    {
        stageEndCanvas.SetActive(true);
    }
}

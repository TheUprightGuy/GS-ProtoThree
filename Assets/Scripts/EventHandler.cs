using System.Collections;
using System.Collections.Generic;
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
    
    private void OnAwake()
    {
    }
}

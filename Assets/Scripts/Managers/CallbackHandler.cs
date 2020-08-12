using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackHandler : MonoBehaviour
{
    #region Singleton
    public static CallbackHandler instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Callback Handler exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            OnAwake();
        }
    }
    #endregion Singleton

    public WhaleInfo whaleInfo;

    private void OnAwake()
    {
        whaleInfo.ResetOnPlay();
    }

    public event Action<bool> landingTooltip;
    public void LandingTooltip(bool _toggle)
    {
        if (landingTooltip != null)
        {
            landingTooltip(_toggle);
        }
    }
}

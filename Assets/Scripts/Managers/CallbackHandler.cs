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

    private void Update()
    {
        whaleInfo.UpdateHunger(Time.deltaTime/2);
    }

    public event Action<bool> landingTooltip;
    public void LandingTooltip(bool _toggle)
    {
        if (landingTooltip != null)
        {
            landingTooltip(_toggle);
        }
    }

    public event Action<bool> toggleShop;
    public void ToggleShop(bool _toggle)
    {
        if (toggleShop != null)
        {
            toggleShop(_toggle);
        }
    }

    public event Action<GameObject> setQuestObjective;
    public void SetQuestObjective(GameObject _target)
    {
        if (setQuestObjective != null)
        {
            setQuestObjective(_target);
        }
    }

    public event Action<bool> orbit;
    public void Orbit(bool _toggle)
    {
        if (orbit != null)
        {
            orbit(_toggle);
        }
    }

    public event Action turnOffOrbit;
    public void TurnOffOrbit()
    {
        if (turnOffOrbit != null)
        {
            turnOffOrbit();
        }
    }
}

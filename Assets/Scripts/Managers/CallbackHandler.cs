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
        whaleInfo.UpdateHunger(Time.deltaTime / 2);
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

    public event Action moveToSaddle;
    public void MoveToSaddle()
    {
        if (moveToSaddle != null)
        {
            moveToSaddle();
        }
    }

    public event Action moveToFire;
    public void MoveToFire()
    {
        if (moveToFire != null)
        {
            moveToFire();
        }
    }

    public event Action shiftWhale;
    public void ShiftWhale()
    {
        if (shiftWhale != null)
        {
            shiftWhale();
        }
    }

    public event Action cutCam;
    public void CutCam()
    {
        if (cutCam != null)
        {
            cutCam();
        }
    }

    public event Action<string> supplyPopUp;
    public void SupplyPopUp(string _supplies)
    {
        if (supplyPopUp != null)
        {
            supplyPopUp(_supplies);
        }
    }

    public event Action<string> provisionPopUp;
    public void ProvisionPopUp(string _provisions)
    {
        if (supplyPopUp != null)
        {
            provisionPopUp(_provisions);
        }
    }

    public event Action<Transform> startHoming;
    public void StartHoming(Transform _player)
    {
        if (startHoming != null)
        {
            startHoming(_player);
        }
    }

    public event Action pickUpMC;
    public void PickUpMC()
    {
        if (pickUpMC != null)
        {
            pickUpMC();
        }
    }

    public event Action startExit;
    public void StartExit()
    {
        if (startExit != null)
        {
            startExit();
        }
    }

    public event Action openDoors;
    public void OpenDoors()
    {
        if (openDoors != null)
        {
            openDoors();
        }
    }

    public bool inShopRange;

    public event Action<Item, ShopItem> showDetails;
    public void ShowDetails(Item _item, ShopItem _shopItem)
    {
        if (showDetails != null)
        {
            showDetails(_item, _shopItem);
        }
    }

    public event Action hideDetails;
    public void HideDetails()
    {
        if (hideDetails != null)
        {
            hideDetails();
        }
    }

    public event Action<string,string> setDialogue;
    public void SetDialogue(string _speaker, string _dialogue)
    {
        if (setDialogue != null)
        {
            ToggleText(true);
            setDialogue(_speaker, _dialogue);
        }
    }

    public event Action<bool> toggleText;
    public void ToggleText(bool _toggle)
    {
        if (toggleText != null)
        {
            toggleText(_toggle);
        }
    }

    public event Action<bool> toggleLamp;
    public void ToggleLamp(bool _toggle)
    {
        if (toggleLamp != null)
        {
            toggleLamp(_toggle);
        }
    }

    public event Action unlockSaddle;
    public void UnlockSaddle()
    {
        if (unlockSaddle != null)
        {
            unlockSaddle();
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Audio;

public class MenuManager : MonoBehaviour
{
    private GameState _gameState;
    public GameObject menuCanvas;
    public GameObject playButton;

    private void Awake()
    {
        playButton.GetComponent<Button>().onClick.AddListener(OnPlayPressed);
    }

    void Start()
    {
        _gameState = EventHandler.instance.gameState;
        //On start menu will always be open except for testing
        _gameState.inMenu = true;
        EventHandler.instance.menuOpened += OnMenuOpened;
        Invoke("GoSit", 0.1f);
    }

    public void OnPlayPressed()
    {
        //EventHandler.instance.OnPlayPressed();
        PlayUISound();
        foreach (Transform n in playButton.transform)
        {
            n.GetComponent<TMPro.TextMeshProUGUI>().text = "RESUME";
        }
        var butComp = playButton.GetComponent<Button>();
        butComp.onClick.RemoveListener(OnPlayPressed);
        playButton.GetComponent<Button>().onClick.AddListener(OnResumePressed);
        playButton.name = "RESUME";
        menuCanvas.SetActive(false);
    }

    private void OnMenuOpened()
    {
        menuCanvas.SetActive(true);
        //CallbackHandler.instance.MoveToFire();
    }

    private void OnResumePressed()
    {
        PlayUISound();
        EventHandler.instance.resumePressed?.Invoke();
        EventHandler.instance.gameState.gamePaused = false;
        EventHandler.instance.gameState.inMenu = false;
        menuCanvas.SetActive(false);
        //CallbackHandler.instance.MoveToSaddle();
    }

    public void OnQuitPressed()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void PlayUISound()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.PlaySound("crackle");
        }
    }
}

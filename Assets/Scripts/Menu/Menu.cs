using System;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        private GameState _gameState;
        public GameObject menuCanvas;
        public GameObject playButton;

        private void Awake()
        {
            playButton.GetComponent<Button>().onClick.AddListener(OnPlayPressed);
        }

        // Start is called before the first frame update
        void Start()
        {
            _gameState = EventHandler.instance.gameState;
            //On start menu will always be open except for testing
            _gameState.inMenu = true;
            EventHandler.instance.menuOpened += OnMenuOpened;
            Invoke("GoSit", 0.1f);
        }

        public void GoSit()
        {
            CallbackHandler.instance.MoveToFire();
        }

        static bool popupDone = false;
        public void OnPlayPressed()
        {
            if(!popupDone)
            {
                PopUpHandler.instance.BasePopups(8);
                popupDone = true;
            }
            EventHandler.instance.OnPlayPressed();
            PlayUISound();
            playButton.GetComponent<TextMeshProUGUI>().text = "RESUME";
            var butComp = playButton.GetComponent<Button>();
            butComp.onClick.RemoveListener(OnPlayPressed);
            playButton.GetComponent<Button>().onClick.AddListener(OnResumePressed);
            playButton.name = "RESUME";
            menuCanvas.SetActive(false);
            CallbackHandler.instance.MoveToSaddle();
        }

        private void OnMenuOpened()
        {
            menuCanvas.SetActive(true);
            CallbackHandler.instance.MoveToFire();
        }

        private void OnResumePressed()
        {
            PlayUISound();
            EventHandler.instance.resumePressed?.Invoke();
            EventHandler.instance.gameState.gamePaused = false;
            EventHandler.instance.gameState.inMenu = false;
            menuCanvas.SetActive(false);
            CallbackHandler.instance.MoveToSaddle();
        }

        public void OnQuitPressed()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }

        public void PlayUISound()
        {
            AudioManager.instance.PlaySound("crackle");
        }
    }
}

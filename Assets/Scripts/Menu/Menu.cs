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
        // Start is called before the first frame update
        void Start()
        {
            _gameState = EventHandler.instance.gameState;
            //On start menu will always be open except for testing
            _gameState.inMenu = true;
            EventHandler.instance.menuOpened += () => menuCanvas.SetActive(true);
        }

        public void OnPlayPressed()
        {
            EventHandler.instance.OnPlayPressed();
            PlayUISound();
            playButton.GetComponent<TextMeshProUGUI>().text = "RESUME";
            playButton.GetComponent<Button>().onClick.RemoveListener(OnPlayPressed);
            playButton.GetComponent<Button>().onClick.AddListener(OnResumePressed);
            playButton.name = "RESUME";
            menuCanvas.SetActive(false);
        }

        private void OnResumePressed()
        {
            PlayUISound();
            EventHandler.instance.gameState.gamePaused = false;
            EventHandler.instance.gameState.inMenu = false;
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

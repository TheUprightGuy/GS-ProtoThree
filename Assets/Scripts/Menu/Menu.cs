using Audio;
using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        private GameState _gameState;
        public GameObject menuCanvas;
        // Start is called before the first frame update
        void Start()
        {
            _gameState = EventHandler.instance.gameState;
            //On start menu will always be open except for testing
            _gameState.inMenu = true;
            AudioManager.instance.PlaySound("loopingCampFire");
            EventHandler.instance.menuOpened += () => menuCanvas.SetActive(true);
        }

        public void OnPlayPressed()
        {
            EventHandler.instance.OnPlayPressed();
            AudioManager.instance.StopSound("loopingCampFire");
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

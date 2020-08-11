using Audio;
using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        private GameState _gameState;
        // Start is called before the first frame update
        void Start()
        {
            _gameState = EventHandler.instance.gameState;
            //On start menu will always be open except for testing
            //_gameState.inMenu = true;
            if(_gameState.inMenu) AudioManager.instance.PlaySound("loopingCampFire");
        }

        public void OnPlayPressed()
        {
            AudioManager.instance.PlaySound("crackle");
            AudioManager.instance.StopSound("loopingCampFire");
            _gameState.inMenu = false;
        }
    }
}

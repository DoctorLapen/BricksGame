using UnityEngine;

namespace SuperBricks
{
    public class PauseMenu:Menu
    {
        [SerializeField]
        private RectTransform _menuPanel;
        [SerializeField]
        private RectTransform _pauseButton;

        
        public void  StartPause()
        {
            Pause.StopTime();
            _pauseButton.gameObject.SetActive(false);
            _menuPanel.gameObject.SetActive(true);
        }

        public void ContinueGame()
        {
            _menuPanel.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            Pause.StartTime();
            
        }
    }
}
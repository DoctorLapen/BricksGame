using UnityEngine;

namespace SuperBricks
{
    public class PauseMenu:Menu
    {
        [SerializeField]
        private RectTransform _menuPanel;

        
        public void  StartPause()
        {
            Pause.StopTime();
            _menuPanel.gameObject.SetActive(true);
        }

        public void ContinueGame()
        {
            _menuPanel.gameObject.SetActive(false);
            Pause.StartTime();
        }
    }
}
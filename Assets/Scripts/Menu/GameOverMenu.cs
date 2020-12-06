using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperBricks
{
    public class GameOverMenu:Menu, IGameOverMenu
    {
        [SerializeField]
        private RectTransform _menuPane;

        
        [SerializeField]
        private Text _scoreText;

        [SerializeField]
        private Text _recordText;

        [SerializeField]
        private string _scoreLabel;
        
        

        public void ShowMenu()
        {
            _menuPane.gameObject.SetActive(true);
        }

        public void ChangeScore(int score,bool isNewRecord)
        {
            _scoreText.text = $"{_scoreLabel} {score}";
            if (isNewRecord)
            {
                _recordText.gameObject.SetActive(true);
            }
        }
        
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }




    }
}
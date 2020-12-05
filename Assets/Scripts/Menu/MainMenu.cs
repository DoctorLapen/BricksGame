using UnityEngine;
using UnityEngine.SceneManagement;

namespace SuperBricks
{
    public class MainMenu : Menu
    {
        public void Play()
        {
            SceneManager.LoadScene("BricksField");
        }
    }
}
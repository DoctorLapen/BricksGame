using System;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBricks
{
    public class ScoreView : MonoBehaviour, IScoreView
    {
        [SerializeField]
        private Text _text;

       

        private void Awake()
        {
            _text.text = "0";
        }

        public void DisplayScore(int score)
        {
            _text.text = $"{score}";
        }
    }
}
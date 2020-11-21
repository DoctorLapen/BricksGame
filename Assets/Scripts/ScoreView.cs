using System;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBricks
{
    public class ScoreView : MonoBehaviour, IScoreView
    {
        [SerializeField]
        private Text _text;

        [SerializeField]
        private string _label;

        private void Awake()
        {
            _text.text = $"{_label} 0";
        }

        public void DisplayScore(int score)
        {
            _text.text = $"{_label} {score}";
        }
    }
}
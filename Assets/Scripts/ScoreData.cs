using System;
using UnityEngine;

namespace SuperBricks
{
    [Serializable]
    public class ScoreData:IScoreData
    {
        public int Score
        {
            get { return _score; }

            set { _score = value; }
        }

        [SerializeField]
        private int _score;

        public ScoreData()
        {
            _score = 0;
        }
    }
}
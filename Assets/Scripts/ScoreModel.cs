using System;
using Zenject;

namespace SuperBricks
{
    public class ScoreModel : IScoreModel
    {
        public event Action<int> ScoreChange; 
        [Inject]
        private IScoreData _scoreData;

        public IScoreData Score => _scoreData;
        
        private int _lineCost;
        
        
        public ScoreModel(int oneLineCost)
        {
            _lineCost = oneLineCost;
        }

        

        public void AddScore(int deletedLinesAmount)
        {
            _scoreData.Score += _lineCost * deletedLinesAmount;
            ScoreChange?.Invoke(_scoreData.Score);
        }
    }
}
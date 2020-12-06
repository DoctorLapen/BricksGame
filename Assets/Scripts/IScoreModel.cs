using System;

namespace SuperBricks
{
    public interface IScoreModel
    {
        event Action<int> ScoreChange;
        IScoreData Score { get; }
        void AddScore(int deletedLinesAmount);
        
    }
}
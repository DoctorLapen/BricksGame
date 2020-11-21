using System;

namespace SuperBricks
{
    public interface IScoreModel
    {
        event Action<int> ScoreChange;
        void AddScore(int deletedLinesAmount);
    }
}
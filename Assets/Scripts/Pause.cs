using UnityEngine;

namespace SuperBricks
{
    public static class Pause
    {
        public static bool IsPause => _isPause;
        private static bool _isPause = false;

        public static void StopTime()
        {
            _isPause = true;
        }
        public static void StartTime()
        {
            _isPause = false;
        }
    }
}
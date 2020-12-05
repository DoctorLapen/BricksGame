using UnityEngine;

namespace SuperBricks
{
    public static class Pause
    {
        public static void StopTime()
        {
            Time.timeScale = 0f;
        }
        public static void StartTime()
        {
            Time.timeScale = 1f;
        }
    }
}
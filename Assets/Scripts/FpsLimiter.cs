using UnityEngine;
namespace SuperBricks
{
    public class FpsLimiter : MonoBehaviour 
    {
        [SerializeField]
        private int _targetFrameRate;
 
        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}
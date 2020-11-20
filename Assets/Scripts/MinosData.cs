using UnityEngine;

namespace SuperBricks
{
    [CreateAssetMenu(fileName = "MinosData", menuName = "MinosData", order = 0)]
    public class MinosData : ScriptableObject, IMinosData
    {
        [SerializeField]
        private Mino[] _minos;

        public Mino[] Minos
        {
            get { return this._minos; }
        }

        [SerializeField]
        private Color[] _minoColors;

        public Color[] MinoColors
        {
            get { return this._minoColors; }
        }

        

        

        
    }
}
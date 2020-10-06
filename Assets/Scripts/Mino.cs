using UnityEngine;

namespace SuperBricks
{
    [CreateAssetMenu(fileName = "Mino", menuName = "MinoShape", order = 1)]
    public class Mino : ScriptableObject
    {
        [SerializeField]
        private Vector2Int[] _blocksLocalCoordinates;

        public Vector2Int[] BlocksLocalCoordinates
        {
            get { return this._blocksLocalCoordinates; }
        }

        
    }
}
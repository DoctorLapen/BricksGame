using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class MinoModel
    {
        private List<Vector2Int> _blockslocalCoordinates = new List<Vector2Int>();
        public List<Vector2Int> BlocksLocalCoordinates => _blockslocalCoordinates;
        public Dictionary<MinoBorder, List<Vector2Int>> BorderIndexes{ get; }

        public MinoModel(List<Vector2Int> blocksCoordinates )
        {
            _blockslocalCoordinates.AddRange(blocksCoordinates);
        }

       
    }
}
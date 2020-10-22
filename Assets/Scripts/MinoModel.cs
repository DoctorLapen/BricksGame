using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class MinoModel
    {
        private List<Vector2Int> _blockslocalCoordinates = new List<Vector2Int>();
        public List<Vector2Int> BlocksLocalCoordinates => _blockslocalCoordinates;
        private List<int> borderIndexes = new List<int>();

        public  List<int> BorderIndexes => borderIndexes;

        public MinoModel(List<Vector2Int> blocksCoordinates,List<int> borderIndexes )
        {
            _blockslocalCoordinates.AddRange(blocksCoordinates);
            this.borderIndexes.AddRange(borderIndexes);
        }

       
    }
}
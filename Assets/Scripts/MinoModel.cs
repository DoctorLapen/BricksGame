using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class MinoModel
    {
        private List<Vector2Int> _blockslocalCoordinates = new List<Vector2Int>();
        public List<Vector2Int> BlocksLocalCoordinates => _blockslocalCoordinates;
        private Dictionary<MinoBorder,IntList>  borderIndexes = new Dictionary<MinoBorder,IntList> ();

        public  Dictionary<MinoBorder,IntList> BorderIndexes => borderIndexes;

        public MinoModel(List<Vector2Int> blocksCoordinates,Dictionary<MinoBorder,IntList> borderIndexes )
        {
            _blockslocalCoordinates.AddRange(blocksCoordinates);
            foreach (var border in borderIndexes)
            {
                this.borderIndexes.Add(border.Key,border.Value);
            }

            
        }

       
    }
}
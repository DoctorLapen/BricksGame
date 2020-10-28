﻿using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class MinoModel
    {
        public List<Vector2Int> BlocksLocalCoordinates => _blockslocalCoordinates ;
        private List<Vector2Int> _blockslocalCoordinates = new List<Vector2Int>();

        
        
        private Queue<MinoSide> _minoSides;
        private SortedDictionary<MinoSide, MinoSide> _rotatedSidesToRealSides;
       
        

        private Dictionary<MinoSide,IntList>  borderIndexes = new Dictionary<MinoSide,IntList> ();
        

        public MinoModel(List<Vector2Int> blocksCoordinates,Dictionary<MinoSide,IntList> borderIndexes )
        {
            _blockslocalCoordinates.AddRange(blocksCoordinates);
            foreach (var border in borderIndexes)
            {
                this.borderIndexes.Add(border.Key,border.Value);
            }

            
        }

        public void RotateMino()
        {
            MinoSide oldSide = _minoSides.Dequeue();
            _minoSides.Enqueue(oldSide);
            RotateBorders();
        }

        public IntList GetBorderIndexes(MinoSide rotatedBorder)
        {
            MinoSide realSide = _rotatedSidesToRealSides[rotatedBorder];
            return borderIndexes[realSide];
        }

       

        private void RotateBorders()
        {
            Queue<MinoSide> minoSides = new Queue<MinoSide>(_minoSides);
            foreach (var key in _rotatedSidesToRealSides.Keys )
            {
                _rotatedSidesToRealSides[key] = minoSides.Dequeue();
            }
        }


    }
}
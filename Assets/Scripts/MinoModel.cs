using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class MinoModel
    {
        public List<Vector2Int> BlocksCoordinates => blocksCoordinates;
    
        public List<Vector2Int> BlockslocalCoordinates=> _blockslocalCoordinates[_currentRotateDirection].List;

        private List<Vector2Int> blocksCoordinates;
        private Dictionary<MinoSide,Vector2IntList> _blockslocalCoordinates = new Dictionary<MinoSide,Vector2IntList>();
        private MinoSide _currentRotateDirection;
        
        
        private Queue<MinoSide> _minoSides = new Queue<MinoSide>();
        private SortedDictionary<MinoSide, MinoSide> _rotatedSidesToRealSides = new SortedDictionary<MinoSide, MinoSide>();
       
        

        private Dictionary<MinoSide,IntList>  borderIndexes = new Dictionary<MinoSide,IntList> ();
        

        public MinoModel(List<Vector2Int> blocksCoordinates,Dictionary<MinoSide,Vector2IntList> localBlocksCoordinates,Dictionary<MinoSide,IntList> borderIndexes )
        {
            this.blocksCoordinates = new List<Vector2Int>(blocksCoordinates);
            foreach (var border in localBlocksCoordinates)
            {
                _blockslocalCoordinates.Add(border.Key,border.Value);
            }
            foreach (var border in borderIndexes)
            {
                this.borderIndexes.Add(border.Key,border.Value);
            }

            InitializeRotateVariables();

        }

        public void RotateMino()
        {
            MinoSide oldSide = _minoSides.Dequeue();
            _currentRotateDirection = _minoSides.Peek();
            _minoSides.Enqueue(oldSide);
            RotateBorders();
        }
        

        public IntList GetBorderIndexes(MinoSide rotatedBorder)
        {
            MinoSide realSide = _rotatedSidesToRealSides[rotatedBorder];
            return borderIndexes[realSide];
        }

        public List<Vector2Int> GetCheckBlockCoordinates()
        {
            Queue<MinoSide> checkSides = new Queue<MinoSide>(_minoSides);
            checkSides.Dequeue();
            MinoSide newRotateDirection = checkSides.Peek();
            return _blockslocalCoordinates[newRotateDirection].List;
        }
        public IntList GetCheckBorderIndexes(MinoSide rotatedBorder)
        {
            Queue<MinoSide> minoSides = new Queue<MinoSide>(_minoSides);
            List<MinoSide> values = new List<MinoSide>( _rotatedSidesToRealSides.Values);
            SortedDictionary<MinoSide, MinoSide> checkDictionary = new SortedDictionary<MinoSide, MinoSide>();
            foreach (var value in values)
            {
                MinoSide key = minoSides.Dequeue();
                checkDictionary.Add( key,value);
            }
            MinoSide realSide = checkDictionary[rotatedBorder];
            return borderIndexes[realSide];
        }



        private void RotateBorders()
        {
            Queue<MinoSide> minoSides = new Queue<MinoSide>(_minoSides);
            List<MinoSide> values = new List<MinoSide>( _rotatedSidesToRealSides.Values);
            foreach (var value in values)
            {
                MinoSide key = minoSides.Dequeue();
                _rotatedSidesToRealSides[key] = value;
            }
        }

        private void InitializeRotateVariables()
        {
            foreach( MinoSide side in (MinoSide[]) Enum.GetValues(typeof(MinoSide)))
            {
                
                _rotatedSidesToRealSides.Add(side,side);
                _minoSides.Enqueue(side);
            }

            _currentRotateDirection = _minoSides.Peek();

        }
        


    }
}
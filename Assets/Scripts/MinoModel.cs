using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class MinoModel
    {
        public List<Vector2Int> BlocksCoordinates => blocksCoordinates;
    
        public List<Vector2Int> BlocksLocalCoordinates=> _mino.BlocksLocalCoordinates[_currentRotateDirection];
        private IMino _mino;

        private List<Vector2Int> blocksCoordinates;
        private MinoSide _currentRotateDirection;
        
        
        private Queue<MinoSide> _minoSides = new Queue<MinoSide>();
      
       
        

        

        public MinoModel(List<Vector2Int> blocksCoordinates,IMino mino )
        {
            this.blocksCoordinates = new List<Vector2Int>(blocksCoordinates);
            _mino = mino;

            InitializeRotateVariables();

        }

        public void RotateMino()
        {
            MinoSide oldSide = _minoSides.Dequeue();
            _currentRotateDirection = _minoSides.Peek();
            _minoSides.Enqueue(oldSide);
           
        }
        

      

        public List<Vector2Int> GetCheckBlockCoordinates()
        {
            Queue<MinoSide> checkSides = new Queue<MinoSide>(_minoSides);
            checkSides.Dequeue();
            MinoSide newRotateDirection = checkSides.Peek();
            return _mino.BlocksLocalCoordinates[newRotateDirection];
        }
        

        

        private void InitializeRotateVariables()
        {
            foreach( MinoSide side in (MinoSide[]) Enum.GetValues(typeof(MinoSide)))
            {
                
                _minoSides.Enqueue(side);
            }

            _currentRotateDirection = _minoSides.Peek();

        }
        


    }
}
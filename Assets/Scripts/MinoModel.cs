using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SuperBricks
{
    public class MinoModel
    {
        
        public ObservableCollection<Vector2Int> BlocksCoordinates => blocksCoordinates;
    
        public List<Vector2Int> BlocksLocalCoordinates=> _mino.BlocksLocalCoordinates[_currentRotateDirection];
        private IMino _mino;

        private ObservableCollection<Vector2Int>  blocksCoordinates;
        private MinoSide _currentRotateDirection;
        
        
        private Queue<MinoSide> _minoSides = new Queue<MinoSide>();
      
       
        

        

        public MinoModel(List<Vector2Int> blocksCoordinates,IMino mino )
        {
            this.blocksCoordinates = new ObservableCollection<Vector2Int>(blocksCoordinates);
            _mino = mino;

            InitializeRotateVariables();

        }

        private void RotateMinoQueue()
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
        public void Move(Vector2Int direction)
        {
            var size = BlocksCoordinates.Count;
            for (int i = 0; i < size; i++)
            {
                Vector2Int oldCoordinate = BlocksCoordinates[i];
                Vector2Int newCoordinates = oldCoordinate + direction;
                BlocksCoordinates[i] = newCoordinates;
            }
            
        }
        public void Rotate()
        {
            Vector2Int startBlock = BlocksCoordinates[0];
            RotateMinoQueue();
            var size = BlocksCoordinates.Count;
            for (int i = 0; i < size; i++)
            {
                Vector2Int oldCoordinate = BlocksLocalCoordinates[i];
                Vector2Int newCoordinates = oldCoordinate + startBlock;
                BlocksCoordinates[i] = newCoordinates;
               
            }
            
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
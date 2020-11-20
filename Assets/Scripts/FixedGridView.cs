using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SuperBricks
{
    public class FixedGridView:MonoBehaviour, IFixedGridView
    {
        [Inject]
        private IMainGameSettings _mainGameSettings;

        [SerializeField]
        private SpriteRenderer _cellPrefab;

        [SerializeField]
        private Transform _startPoint;

        [SerializeField]
        private float _horizontalOffset;
        [SerializeField]
        private float _verticalOffset;
        
        private Vector3 _startPosition;
        private Queue<Transform> _blocksInMove;

        private Transform[,] _staticBlocks;
        private Queue<Transform> _deletedBlocks = new Queue<Transform>();
       


        private void Awake()
        {
            _blocksInMove = new Queue<Transform>();
            _startPosition = _startPoint.localPosition;
            _staticBlocks = new Transform[_mainGameSettings.ColumnAmount,_mainGameSettings.RowAmount];
            

        }

        private Vector3 CalculateCellPosition(int x,int y)
        {
            Vector3 cellPosition= _startPosition;
            
            cellPosition.x += (_cellPrefab.sprite.bounds.size.x+ _horizontalOffset) * x;
            cellPosition.y -= (_cellPrefab.sprite.bounds.size.y + _verticalOffset) * y;
            
            return cellPosition;

        }

        public void SpawnSprite(Vector2Int coord)
        {
            Vector3 spawnPosition = CalculateCellPosition(coord.x, coord.y);
            Transform block = Instantiate(_cellPrefab, spawnPosition, Quaternion.identity,transform).transform;
            _blocksInMove.Enqueue(block);
        }


        public void MoveSprite(Vector2Int newCoordinates)
        {
            var newX = newCoordinates.x;
            var newY = newCoordinates.y;
            Transform block = _blocksInMove.Dequeue();
            block .localPosition = CalculateCellPosition(newX, newY);
            _blocksInMove.Enqueue(block);
            
        }
        public void DeleteStaticSprite(int newX, int newY)
        {
           
            if (_staticBlocks[newX, newY] != null)
            {
              
                Transform block = _staticBlocks[newX, newY];
                block.gameObject.SetActive(false);
                _deletedBlocks.Enqueue(block);
                _staticBlocks[newX, newY] = null;
            }

        }
      

        public void MoveStaticSprite(int newX, int newY)
        {
            DeleteStaticSprite( newX , newY);
            Transform  block = _deletedBlocks.Dequeue();
            block.gameObject.SetActive(true);
            block.localPosition = CalculateCellPosition(newX, newY);
            _staticBlocks[newX, newY] = block;
        }

        public void ClearMoveBlocks()
        {
            int size = _blocksInMove.Count;
            for (int i = 0; i < size; i++)
            {
                Transform block = _blocksInMove.Dequeue();
                block.gameObject.SetActive(false);
                _deletedBlocks.Enqueue(block);
            }
            
            _blocksInMove.Clear();
        }
    }
}
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
        private Queue<SpriteRenderer> _blocksInMove;

        private SpriteRenderer[,] _staticBlocks;
        private Queue<SpriteRenderer> _deletedBlocks = new Queue<SpriteRenderer>();
       


        private void Awake()
        {
            _blocksInMove = new Queue<SpriteRenderer>();
            _startPosition = _startPoint.localPosition;
            _staticBlocks = new SpriteRenderer[_mainGameSettings.ColumnAmount,_mainGameSettings.RowAmount];
            

        }

        private Vector3 CalculateCellPosition(int x,int y)
        {
            Vector3 cellPosition= _startPosition;
            
            cellPosition.x += (_cellPrefab.sprite.bounds.size.x+ _horizontalOffset) * x;
            cellPosition.y -= (_cellPrefab.sprite.bounds.size.y + _verticalOffset) * y;
            
            return cellPosition;

        }

        public void SpawnSprite(Vector2Int coord, Color spriteColor)
        {
            Vector3 spawnPosition = CalculateCellPosition(coord.x, coord.y);
            SpriteRenderer block = Instantiate(_cellPrefab, spawnPosition, Quaternion.identity,transform);
            block.color = spriteColor;
            _blocksInMove.Enqueue(block);
        }


        public void MoveSprite(Vector2Int newCoordinates, Color spriteColor)
        {
            var newX = newCoordinates.x;
            var newY = newCoordinates.y;
            SpriteRenderer block = _blocksInMove.Dequeue();
            block.color = spriteColor;
            block.transform.localPosition = CalculateCellPosition(newX, newY);
            _blocksInMove.Enqueue(block);
            
        }
        public void DeleteStaticSprite(int newX, int newY)
        {
           
            if (_staticBlocks[newX, newY] != null)
            {
              
                SpriteRenderer block = _staticBlocks[newX, newY];
                block.gameObject.SetActive(false);
                _deletedBlocks.Enqueue(block);
                _staticBlocks[newX, newY] = null;
            }

        }
      

        public void MoveStaticSprite(int newX, int newY, Color spriteColor)
        {
            DeleteStaticSprite( newX , newY);
            SpriteRenderer  block = _deletedBlocks.Dequeue();
            block.gameObject.SetActive(true);
            block.transform.localPosition = CalculateCellPosition(newX, newY);
            block.color = spriteColor;
            _staticBlocks[newX, newY] = block;
        }

        public void ClearMoveBlocks()
        {
            int size = _blocksInMove.Count;
            for (int i = 0; i < size; i++)
            {
                SpriteRenderer block = _blocksInMove.Dequeue();
                block.gameObject.SetActive(false);
                _deletedBlocks.Enqueue(block);
            }
            
            _blocksInMove.Clear();
        }
    }
}
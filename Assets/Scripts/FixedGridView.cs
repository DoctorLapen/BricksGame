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

        [SerializeField]
        private uint _columns;
        [SerializeField]
        private uint _rows;

        private Vector3 _startPosition;
        private Queue<Transform> _blocksInMove;

        private Transform[,] _staticBlocks;


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
            _staticBlocks[newX, newY] = block;
            _blocksInMove.Enqueue(block);
        }
        public void DeleteSprite(Vector2Int coordinate)
        {
            var newX = coordinate.x;
            var newY = coordinate.y;
            Transform block = _staticBlocks[newX, newY] ;
            Destroy(block.gameObject);
        }
        public Transform GetStaticSprite(int x,int y)
        {
            var newX = x;
            var newY = y;
            return _staticBlocks[newX, newY] ;
          
        }

        public void MoveStaticSprite(int newX,int newY,Transform spriteTransform)
        {
            spriteTransform.localPosition = CalculateCellPosition(newX, newY);
            _staticBlocks[newX, newY] = spriteTransform;
        }

        public void ClearMoveBlocks()
        {
            _blocksInMove.Clear();
        }
    }
}
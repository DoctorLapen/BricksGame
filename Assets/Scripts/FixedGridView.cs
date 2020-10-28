using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class FixedGridView:MonoBehaviour, IFixedGridView
    {
        

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
        private Queue<Transform> _blocks;
        

        private void Awake()
        {
            _blocks = new Queue<Transform>();
            _startPosition = _startPoint.localPosition;
            

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
            _blocks.Enqueue(block);
        }


        public void MoveSprite(Vector2Int newCoordinates)
        {
            var newX = newCoordinates.x;
            var newY = newCoordinates.y;
            Transform block = _blocks.Dequeue();
            block .localPosition = CalculateCellPosition(newX, newY);

            _blocks.Enqueue(block);
        }

        public void ClearMoveBlocks()
        {
            _blocks.Clear();
        }
    }
}
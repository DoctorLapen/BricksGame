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
        private const uint _startIndex = 0;
        private Vector3[,] _cellsPositions;

        private Vector3 _startPosition;
        

        private void Awake()
        {
            _cellsPositions = new Vector3[_columns,_rows];
            _startPosition = _startPoint.localPosition;
            CalculateCellsPositions();

        }

        private void CalculateCellsPositions()
        {
            Vector3 rowPosition = _startPosition;
            for (uint row = _startIndex; row < _rows; row++)
            {
                Vector3 columnPosition = rowPosition;
                for (uint column = _startIndex; column < _columns; column++)
                {
                    
                    _cellsPositions[column, row] = columnPosition;
                    columnPosition.x += _cellPrefab.sprite.bounds.size.x+ _horizontalOffset;

                }

                rowPosition.y -= _cellPrefab.sprite.bounds.size.y + _verticalOffset;
            }
        }

        public void SpawnSpriteInCell(uint x, uint y)
        {
            Vector3 spawnPosition = _cellsPositions[x, y];
            Instantiate(_cellPrefab, spawnPosition, Quaternion.identity,transform);
        }

    }
}
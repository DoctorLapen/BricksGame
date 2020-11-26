using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SuperBricks
{
    public class NextMinosView : MonoBehaviour, INextMinosView
    {
        [Inject]
        private IMainGameSettings _mainGameSettings;

        [Inject]
        private IMinosData _minosData;

        [SerializeField]
        private SpriteRenderer _cellPrefab;

        [SerializeField]
        private Transform _startPoint;

        [SerializeField]
        private float _horizontalOffset;
        [SerializeField]
        private float _verticalOffset;
        [SerializeField]
        private float _distanceBetweenMinosByY;
        
        private Vector3 _startPosition;
        private Queue<SpriteRenderer> _deletedBlocks = new Queue<SpriteRenderer>();
        private Queue<SpriteRenderer> _showedBlocks = new Queue<SpriteRenderer>();
        private Vector3[] _startPositions;

        private void Awake()
        {
            _startPosition = _startPoint.localPosition;
            _startPositions = new Vector3[_minosData.AmountMinosToShow];
            CalculateStartPositions();
        }


        private Vector3 CalculateCellPosition(int x,int y,Vector3 startPosition)
        {
           
            
            startPosition.x += (_cellPrefab.sprite.bounds.size.x+ _horizontalOffset) * x;
            startPosition.y -= (_cellPrefab.sprite.bounds.size.y + _verticalOffset) * y;
            
            return startPosition;

        }

        public void SpawnMinos(List<List<Vector2Int>> blockCoordinates)
        {
            ClearView();
            for (int minoNum = 0; minoNum < _minosData.AmountMinosToShow; minoNum++)
            {
                foreach (Vector2Int blockCoordinate in blockCoordinates[minoNum])
                {
                    SpriteRenderer block = GetBlock();
                    block.transform.localPosition = CalculateCellPosition(blockCoordinate.x,blockCoordinate.y,_startPositions[minoNum]);
                    _showedBlocks.Enqueue(block);
                }
            }
           
        }

        private SpriteRenderer GetBlock()
        {
            
            SpriteRenderer block = null;
            if (_deletedBlocks.Count > 0)
            {
                block = _deletedBlocks.Dequeue();
            }
            else
            {
                block = SpawnBlock();
            }
            block.gameObject.SetActive(true);
            return block;
        }

        private SpriteRenderer SpawnBlock()
        {
            return Instantiate(_cellPrefab, Vector3.zero, Quaternion.identity,transform);;
        }

        private void ClearView()
        {
            int size = _showedBlocks.Count;
            for (int i = 0; i < size; i++)
            {
                SpriteRenderer block  = _showedBlocks.Dequeue();
                block.gameObject.SetActive(false);
                _deletedBlocks.Enqueue(block);
                
            }
        }

        private void CalculateStartPositions()
        {
            for (int num = 0; num < _minosData.AmountMinosToShow; num++)
            {
                Vector3 newStartPosition = _startPosition;
                newStartPosition.y += _distanceBetweenMinosByY * (float)num;
                _startPositions[num] = newStartPosition;
            }
        }
    }
  
}
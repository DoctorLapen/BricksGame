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

        [SerializeField]
        private SpriteRenderer _cellPrefab;

        [SerializeField]
        private Transform _startPoint;

        [SerializeField]
        private float _horizontalOffset;
        [SerializeField]
        private float _verticalOffset;
        
        private Vector3 _startPosition;
        private Queue<SpriteRenderer> _deletedBlocks = new Queue<SpriteRenderer>();
        private Queue<SpriteRenderer> _showedBlocks = new Queue<SpriteRenderer>();

        private void Awake()
        {
            _startPosition = _startPoint.localPosition;
        }


        private Vector3 CalculateCellPosition(int x,int y)
        {
            Vector3 cellPosition= _startPosition;
            
            cellPosition.x += (_cellPrefab.sprite.bounds.size.x+ _horizontalOffset) * x;
            cellPosition.y -= (_cellPrefab.sprite.bounds.size.y + _verticalOffset) * y;
            
            return cellPosition;

        }

        public void SpawnMino(List<Vector2Int> blockCoordinates)
        {
            ClearView();
            foreach (Vector2Int blockCoordinate in blockCoordinates)
            {
                SpriteRenderer block = GetBlock();
                block.transform.localPosition = CalculateCellPosition(blockCoordinate.x,blockCoordinate.y);
                _showedBlocks.Enqueue(block);
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
    }
  
}
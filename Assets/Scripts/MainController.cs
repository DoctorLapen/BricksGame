using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace SuperBricks
{
    public class MainController : MonoBehaviour
    {
        [Inject]
        private IFixedGridView _gridView;
        [Inject]
        private IFieldModel _fieldModel;
        
        private const int FIRST_INDEX = 0;
        [SerializeField]
        private Mino [] _minos;

        [SerializeField]
        private Vector2Int _spawnCell;

        private MinoModel _minoModel; 

        
        

        
        private void Start()
        {
            var mino = SelectRandomMino();
            
            SpawnMino(mino);

        }

        private void Awake()
        {
            if (Input.touchCount > 0)
            {
            }
        }

        private void Update()
        {
            Vector2Int direction = new Vector2Int(0,0);
            //Move
            //Down
            if (Input.GetKeyDown(KeyCode.S))
            {
                direction  = new Vector2Int(0,1);
                MoveMino(direction);
            }
            //Right
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction = new Vector2Int(1, 0);
                MoveMino(direction);
            }
            //Left
            else if (Input.GetKeyDown(KeyCode.A))
            {
                direction = new Vector2Int(-1, 0);
                MoveMino(direction);
            }
            


        }

        private void MoveMino(Vector2Int direction)
        {
            var size = _minoModel.BlocksLocalCoordinates.Count;
            Transform[] spriteTransforms = new Transform[size];
            for (int i = 0; i < size; i++)
            {
                var oldCoordinates = _minoModel.BlocksLocalCoordinates[i];
                spriteTransforms[i] = _gridView.GetSpriteTransform(oldCoordinates);

            }
            for (int i = 0; i < size; i++)
            {
                var oldCoordinates = _minoModel.BlocksLocalCoordinates[i];
                var newCoordinates = oldCoordinates + direction;
                _minoModel.BlocksLocalCoordinates[i] = newCoordinates;
                _gridView.MoveSprite(newCoordinates,spriteTransforms[i]);
                

            }
        }

        private Mino SelectRandomMino()
        {
            int minoIndex = Random.Range(FIRST_INDEX, _minos.Length);
            return _minos[minoIndex];
        }

        private void SpawnMino(Mino mino)
        {
            List<Vector2Int> blockCoordinates = new List<Vector2Int>();
          
            foreach (Vector2Int localBlockCoordinates in mino.BlocksLocalCoordinates)
            {
                Vector2Int cell = _spawnCell + localBlockCoordinates;
                blockCoordinates.Add(cell);
                _gridView.SpawnSprite(cell);

            }
            _minoModel = new MinoModel(blockCoordinates);
        }
        

    }
}
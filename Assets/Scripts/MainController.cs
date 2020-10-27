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
        [Inject]
        private IMainGameSettings _mainGameSettings;
        
        private const int FIRST_INDEX = 0;
        [SerializeField]
        private Mino [] _minos;

        [SerializeField]
        private Vector2Int _spawnCell;

        private MinoModel _minoModel;
        private List<KeyCode> _correctInputKeys; 

        
        

        
        private void Start()
        {
            CreateNewMino();
            InitializeCorrectKeyCodes();

        }

        private void Awake()
        {
            if (Input.touchCount > 0)
            {
            }
        }

        private void Update()
        {
            foreach (KeyCode key in _correctInputKeys)
            {
                Vector2Int direction = new Vector2Int(0, 0);
                //Move
                //Down
                if (Input.GetKeyDown(key))
                {
                    MinoBorder border = MinoBorder.Bottom;
                    if (key == KeyCode.S)
                    {
                        direction = new Vector2Int(0, 1);
                        border = MinoBorder.Bottom;
                    }

                    //Right
                    else if (key == KeyCode.D)
                    {
                        direction = new Vector2Int(1, 0);
                        border = MinoBorder.Right;
                    }
                    //Left
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        direction = new Vector2Int(-1, 0);
                        border = MinoBorder.Left;
                        
                    }

                    bool isInField = IsMoveInField(border, direction);
                    bool isMovingPossible = true;
                    if (isInField)
                    {
                        isMovingPossible = IsMovePossible(border, direction);
                        if (isMovingPossible)
                        {
                            MoveMino(direction);
                        }


                    }
                    if(!(isInField && isMovingPossible) && border == MinoBorder.Bottom)
                    {
                        Debug.Log("New Mino");
                        
                            AddMinoToFieldModel();
                            CreateNewMino();
                        
                    }


                }
                
            }



        }

        private void InitializeCorrectKeyCodes()
        {
            _correctInputKeys = new List<KeyCode>() {KeyCode.A,KeyCode.S,KeyCode.D};
        }

        private bool IsMovePossible(MinoBorder border,Vector2Int direction)
        {
            List<int> BorderIndexes = _minoModel.BorderIndexes[border].List;
            
            foreach (int borderIndex in BorderIndexes)
            {
                Vector2Int coordinate = _minoModel.BlocksLocalCoordinates[borderIndex] + direction;
                bool isCellEmpty = _fieldModel.IsCellEmpty((uint)coordinate.x,(uint) coordinate.y);
               if (!isCellEmpty)
               {
                   return false;
               }
            }

            return true;
        }

        
        private bool IsMoveInField(MinoBorder border,Vector2Int direction)
        {
            List<int> BorderIndexes = _minoModel.BorderIndexes[border].List;
           
            int startCoordinate = 0;
            foreach (int borderIndex in BorderIndexes)
            {
                Vector2Int coordinate = _minoModel.BlocksLocalCoordinates[borderIndex] + direction;
                bool isXInField = startCoordinate <= coordinate.x  && coordinate.x  < _mainGameSettings.ColumnAmount ;
                bool isYInField = startCoordinate <= coordinate.y  && coordinate.y  < _mainGameSettings.RowAmount ;
                Debug.Log(coordinate);
                Debug.Log(!(isXInField && isYInField));
                if (!(isXInField && isYInField))
                {
                    return false;
                }
            }

            return true;
            
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
            Debug.Log(mino.BorderIndexes.Count);
            _minoModel = new MinoModel(blockCoordinates,mino.BorderIndexes);
          
        }

        private void CreateNewMino()
        {
            var mino = SelectRandomMino();
            
            SpawnMino(mino);
        }

        private void AddMinoToFieldModel()
        {
            foreach (Vector2Int block in _minoModel.BlocksLocalCoordinates)
            {
                _fieldModel.FillCell((uint)block.x,(uint)block.y);
            }
        }


    }
}
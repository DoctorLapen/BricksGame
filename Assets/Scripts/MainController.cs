using System;
using System.Collections.Generic;
using System.Linq;
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
        private const float TARGET_TIME_AMOUNT= 1f;
        [SerializeField]
        private Mino [] _minos;

        [SerializeField]
        private float _minoFallSpeed ;

        

        [SerializeField]
        private Vector2Int _spawnCell;

        private MinoModel _minoModel;
        private List<KeyCode> _correctInputKeys;
        private float _currentTimeAmount = 0f;

        
        

        
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
            if (TARGET_TIME_AMOUNT < _currentTimeAmount)
            {
                Vector2Int direction = new Vector2Int(0, 1);
                MinoSide side = MinoSide.Bottom;
                MoveMinoWithChecking(side, direction);
                _currentTimeAmount = 0f;
            }
            else
            {
                _currentTimeAmount += Time.deltaTime * _minoFallSpeed;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                List<Vector2Int> checkBlockCoordinates = _minoModel.GetCheckBlockCoordinates();
                if (IsRotateInField(checkBlockCoordinates))
                {
                    if (IsRotatePossible(checkBlockCoordinates))
                    {
                        RotateMino();
                    }

                    
                }
                
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Vector2Int direction = CalculateCoordinatesForBottomEndMove();
                MoveMino(direction);
                AddMinoToFieldModel();
                CreateNewMino();
            }

            foreach (KeyCode key in _correctInputKeys)
            {
                Vector2Int direction = new Vector2Int(0, 0);
                //Move
                //Down
                if (Input.GetKeyDown(key))
                {
                    MinoSide side = MinoSide.Bottom;

                    //Right
                    if (key == KeyCode.D)
                    {
                        direction = new Vector2Int(1, 0);
                        side = MinoSide.Right;
                    }
                    //Left
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        direction = new Vector2Int(-1, 0);
                        side = MinoSide.Left;
                        
                    }

                    MoveMinoWithChecking(side, direction);
                }
                
            }



        }

        private void MoveMinoWithChecking(MinoSide side, Vector2Int direction)
        {
            bool isInField = IsMoveInField(side, direction);
            bool isMovingPossible = true;
            if (isInField)
            {
                isMovingPossible = IsMovePossible(side, direction);
                if (isMovingPossible)
                {
                    MoveMino(direction);
                }
            }

            if (!(isInField && isMovingPossible) && side == MinoSide.Bottom)
            {
                AddMinoToFieldModel();
                CreateNewMino();
            }
        }

        private void InitializeCorrectKeyCodes()
        {
            _correctInputKeys = new List<KeyCode>() {KeyCode.A,KeyCode.D};
        }

        private bool IsMovePossible(MinoSide side,Vector2Int direction)
        {
            List<int> BorderIndexes = _minoModel.GetBorderIndexes(side).List;
            
            foreach (int borderIndex in BorderIndexes)
            {
                Vector2Int coordinate = _minoModel.BlocksCoordinates[borderIndex] + direction;
                bool isCellEmpty = _fieldModel.IsCellEmpty((uint)coordinate.x,(uint) coordinate.y);
               if (!isCellEmpty)
               {
                   return false;
               }
            }

            return true;
        }
        private Vector2Int CalculateCoordinatesForBottomEndMove()
        {
            List<int> BorderIndexes = _minoModel.GetBorderIndexes(MinoSide.Bottom).List;
            List<Vector2Int> endCoordinates = new List<Vector2Int>();

           

            foreach(int borderIndex in BorderIndexes)
            {
                int vectorIndex = 0;
                Vector2Int endCoordinate = _minoModel.BlocksCoordinates[borderIndex];
                Vector2Int checkCoordinate = endCoordinate;
                while (checkCoordinate.y < _mainGameSettings.RowAmount - 1)
                {
                    int oneBlockDownX = 1;
                    checkCoordinate.y += oneBlockDownX;
                    bool isCellEmpty = _fieldModel.IsCellEmpty((uint)checkCoordinate.x,(uint) checkCoordinate.y);
                    if (!isCellEmpty)
                    {
                        checkCoordinate.y -= 1;
                        Vector2Int moveVector = checkCoordinate - endCoordinate;
                        endCoordinates.Add(moveVector);
                        break;
                    }

                }
                vectorIndex++;
                if (endCoordinates.Count  != vectorIndex)
                {
                    Vector2Int moveVector = checkCoordinate - endCoordinate;
                    endCoordinates.Add(moveVector);
                }


            }

            int minY = endCoordinates.Min(v => v.y);
            return endCoordinates.First(v => v.y == minY);

        }

        
        private bool IsMoveInField(MinoSide side,Vector2Int direction)
        {
            List<int> BorderIndexes = _minoModel.GetBorderIndexes(side).List;
           
            int startCoordinate = 0;
            foreach (int borderIndex in BorderIndexes)
            {
                Vector2Int coordinate = _minoModel.BlocksCoordinates[borderIndex] + direction;
                bool isXInField = startCoordinate <= coordinate.x  && coordinate.x  < _mainGameSettings.ColumnAmount ;
                bool isYInField = startCoordinate <= coordinate.y  && coordinate.y  < _mainGameSettings.RowAmount ;
              
                if (!(isXInField && isYInField))
                {
                    return false;
                }
            }

            return true;
            
        }
        private bool IsRotateInField(List<Vector2Int> blocksCoordinates)
        {
            Vector2Int startBlock = _minoModel.BlocksCoordinates[0];
           
            int startCoordinate = 0;
            foreach (Vector2Int blockCoordinates in blocksCoordinates)
            {
                Vector2Int coordinate = startBlock + blockCoordinates;
                bool isXInField = startCoordinate <= coordinate.x  && coordinate.x  < _mainGameSettings.ColumnAmount ;
                bool isYInField = startCoordinate <= coordinate.y  && coordinate.y  < _mainGameSettings.RowAmount ;
              
                if (!(isXInField && isYInField))
                {
                    return false;
                }
            }

            return true;
            
        }
        private bool IsRotatePossible(List<Vector2Int> blocksCoordinates)
        {
            
            Vector2Int startBlock = _minoModel.BlocksCoordinates[0];
            foreach (Vector2Int blockCoordinates in blocksCoordinates)
            {
                Vector2Int coordinate = startBlock + blockCoordinates;
                bool isCellEmpty = _fieldModel.IsCellEmpty((uint)coordinate.x,(uint) coordinate.y);
                if (!isCellEmpty)
                {
                    return false;
                }
            }

            return true;
        }

        private void MoveMino(Vector2Int direction)
        {
            var size = _minoModel.BlocksCoordinates.Count;
            for (int i = 0; i < size; i++)
            {
                Vector2Int oldCoordinate = _minoModel.BlocksCoordinates[i];
                Vector2Int newCoordinates = oldCoordinate + direction;
                _minoModel.BlocksCoordinates[i] = newCoordinates;
                _gridView.MoveSprite(newCoordinates);
            }
            
        }
        private void RotateMino()
        {
            Vector2Int startBlock = _minoModel.BlocksCoordinates[0];
            _minoModel.RotateMino();
            var size = _minoModel.BlocksCoordinates.Count;
            for (int i = 0; i < size; i++)
            {
                Vector2Int oldCoordinate = _minoModel.BlockslocalCoordinates[i];
                Vector2Int newCoordinates = oldCoordinate + startBlock;
                _minoModel.BlocksCoordinates[i] = newCoordinates;
                _gridView.MoveSprite(newCoordinates);
            }
            
        }
        
        

        private Mino SelectRandomMino()
        {
            int minoIndex = Random.Range(FIRST_INDEX, _minos.Length);
            return _minos[minoIndex];
        }

        private void SpawnMino(Mino mino)
        {
            _gridView.ClearMoveBlocks();
            List<Vector2Int> bottomBlockCoordinates = new List<Vector2Int>();
          
             foreach (var localBlockCoordinates in mino.BlocksLocalCoordinates[MinoSide.Bottom].List)
             {
                 Vector2Int cell = _spawnCell + localBlockCoordinates;
                 bottomBlockCoordinates.Add(cell);
                 _gridView.SpawnSprite(cell);
            
             }
             
            _minoModel = new MinoModel(bottomBlockCoordinates ,mino.BlocksLocalCoordinates,mino.BorderIndexes);
          
        }

        private void CreateNewMino()
        {
            var mino = SelectRandomMino();
            
            SpawnMino(mino);
        }

        private void AddMinoToFieldModel()
        {
            foreach (Vector2Int block in _minoModel.BlocksCoordinates)
            {
                _fieldModel.FillCell((uint)block.x,(uint)block.y);
            }
        }


    }
}
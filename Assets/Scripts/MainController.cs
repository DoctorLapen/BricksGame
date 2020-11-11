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
           Mino newMino =  SelectRandomMino();
           if (IsGameOver(newMino))
           {
               SpawnMino(newMino);
           }
           else
           {
               Debug.Log("GameOver");
           }

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
                 MoveMinoToBottomEnd();
                 UpdateGameState();
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
            bool isInField = IsMoveInField(direction,_minoModel.BlocksCoordinates);
            bool isMovingPossible = true;
            if (isInField)
            {
                isMovingPossible = IsMovePossible(direction,_minoModel.BlocksCoordinates);
                if (isMovingPossible)
                {
                    MoveMino(direction);
                }
            }

            if (!(isInField && isMovingPossible) && side == MinoSide.Bottom)
            {
                UpdateGameState();
            }
        }

        private void UpdateGameState()
        {
            AddMinoToFieldModel();
            List<int> deleteLineIndexes = FindFilledHorizontalLines();
            if (deleteLineIndexes.Count > 0)
            {
                DeleteHorizontalLines(deleteLineIndexes);
                MoveLinesDown(deleteLineIndexes);
            }

            Mino newMino = SelectRandomMino();
            if (IsGameOver(newMino))
            {
                SpawnMino(newMino);
            }
            else
            {
                Debug.Log("GameOver");
            }
        }

        private void InitializeCorrectKeyCodes()
        {
            _correctInputKeys = new List<KeyCode>() {KeyCode.A,KeyCode.D};
        }

        private bool IsMovePossible(Vector2Int direction, List<Vector2Int> blocksCoordinates)
        {
           
            
            foreach (Vector2Int coordinate in blocksCoordinates)
            {
                Vector2Int newCoordinate = coordinate + direction;
                bool isCellEmpty = _fieldModel.IsCellEmpty((uint)newCoordinate.x,(uint) newCoordinate.y);
               if (!isCellEmpty)
               {
                   return false;
               }
            }

            return true;
        }
        private void MoveMinoToBottomEnd()
        {
            Vector2Int direction = new Vector2Int(0,1);

            
            for (int y = _minoModel.BlocksCoordinates[0].y ;y < _mainGameSettings.RowAmount - 1;y++)
            {
                bool isInField = IsMoveInField(direction,_minoModel.BlocksCoordinates);
                bool isMovingPossible = false;
                if (isInField)
                {
                    isMovingPossible = IsMovePossible(direction,_minoModel.BlocksCoordinates);
                    if (isMovingPossible)
                    {
                        MoveMino(direction);
                        
                    }
                }
                
               
                if(!isInField || !isMovingPossible)
                {
                   
                    break;
                }

            }
            

        }

        
        private bool IsMoveInField(Vector2Int direction,List<Vector2Int> blocksCoordinates)
        {
           
           
            int startCoordinate = 0;
            foreach (Vector2Int coordinate in blocksCoordinates)
            {
                Vector2Int newCoordinate = coordinate  + direction;
                bool isXInField = startCoordinate <= newCoordinate.x  && newCoordinate.x  < _mainGameSettings.ColumnAmount ;
                bool isYInField = startCoordinate <= newCoordinate.y  && newCoordinate.y  < _mainGameSettings.RowAmount ;
              
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
                Vector2Int oldCoordinate = _minoModel.BlocksLocalCoordinates[i];
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
          
             foreach (var localBlockCoordinates in mino.BlocksLocalCoordinates[MinoSide.Bottom])
             {
                 Vector2Int cell = _spawnCell + localBlockCoordinates;
                 bottomBlockCoordinates.Add(cell);
                 _gridView.SpawnSprite(cell);
            
             }
             
            _minoModel = new MinoModel(bottomBlockCoordinates ,mino);
          
        }

        private List<int> FindFilledHorizontalLines()
        {
            List<int> lineIndexes = new List<int>();
            int startRowIndex =(int) _mainGameSettings.RowAmount - 1;
            for (int row = startRowIndex; row >= 0; row--)
            {
                for (int column = 0; column < _mainGameSettings.ColumnAmount; column++)
                {
                    if (_fieldModel.IsCellEmpty((uint) column, (uint) row))
                    {
                        break;
                    }
                    if (column == _mainGameSettings.ColumnAmount - 1)
                    {
                        lineIndexes.Add(row);
                    }
                }
            }

            return lineIndexes;
        }

        private void DeleteHorizontalLines(List<int> lineIndexes)
        {
            foreach (int lineIndex in lineIndexes)
            {
                for (int column = 0; column < _mainGameSettings.ColumnAmount; column++)
                {
                    Vector2Int coordinate = new Vector2Int(column,lineIndex);
                    _gridView.DeleteSprite(coordinate);
                    _fieldModel.MakeCellEmpty((uint)column,(uint)lineIndex);
                }
            }
        }
        private void MoveLinesDown(List<int> lineIndexes)
        {
            foreach (int lineIndex in lineIndexes)
            {
                int emptyLineIndex = lineIndex;
                for (int moveLineIndex = lineIndex - 1; moveLineIndex != -1; moveLineIndex--)
                {
                    for (int column = 0; column < _mainGameSettings.ColumnAmount; column++)
                    {

                        if (!_fieldModel.IsCellEmpty((uint) column, (uint) moveLineIndex))
                        {
                            _fieldModel.MakeCellEmpty((uint) column,(uint) moveLineIndex);
                            _fieldModel.FillCell((uint) column,(uint) emptyLineIndex);
                            Transform spriteTransform = _gridView.GetStaticSprite(column, moveLineIndex);
                            _gridView.MoveStaticSprite(column, emptyLineIndex,spriteTransform);
                        }
                    }

                    emptyLineIndex--;


                }
            }
        }

       

        private void AddMinoToFieldModel()
        {
            foreach (Vector2Int block in _minoModel.BlocksCoordinates)
            {
                _fieldModel.FillCell((uint)block.x,(uint)block.y);
            }
        }

        private bool IsGameOver(Mino mino)
        {
            foreach (Vector2Int localCoordinate in mino.BlocksLocalCoordinates[MinoSide.Bottom])
            {
                Vector2Int coordinate = localCoordinate + _spawnCell ;
                bool isCellEmpty = _fieldModel.IsCellEmpty((uint)coordinate.x,(uint) coordinate.y);
                if (!isCellEmpty)
                {
                    return false;
                }
            }

            return true;
        }


    }
}
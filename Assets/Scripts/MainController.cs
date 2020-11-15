using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
           _minoModel.BlocksCoordinates.CollectionChanged += MoveSprite;
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
                if (_fieldModel.IsRotateInField(_minoModel.BlocksCoordinates[0],checkBlockCoordinates))
                {
                    if (IsRotatePossible(checkBlockCoordinates))
                    {
                        _minoModel.Rotate();
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
            bool isInField = _fieldModel.IsMoveInField(direction,_minoModel.BlocksCoordinates);
            bool isMovingPossible = true;
            if (isInField)
            {
                isMovingPossible = IsMovePossible(direction,_minoModel.BlocksCoordinates);
                if (isMovingPossible)
                {
                    _minoModel.Move(direction);
                }
            }

            if (!(isInField && isMovingPossible) && side == MinoSide.Bottom)
            {
                UpdateGameState();
            }
        }

        private void UpdateGameState()
        {
            _fieldModel.AddMino(_minoModel.BlocksCoordinates);
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

        private bool IsMovePossible(Vector2Int direction, IList<Vector2Int> blocksCoordinates)
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
                bool isInField = _fieldModel.IsMoveInField(direction,_minoModel.BlocksCoordinates);
                bool isMovingPossible = false;
                if (isInField)
                {
                    isMovingPossible = IsMovePossible(direction,_minoModel.BlocksCoordinates);
                    if (isMovingPossible)
                    {
                        _minoModel.Move(direction);
                        
                    }
                }
                
               
                if(!isInField || !isMovingPossible)
                {
                   
                    break;
                }

            }
            

        }

        
      
       
        private bool IsRotatePossible(IList<Vector2Int> blocksCoordinates)
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

        private void MoveSprite(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
           _gridView.MoveSprite((Vector2Int)eventArgs.NewItems[0]);
        }


    }
}
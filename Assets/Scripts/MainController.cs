using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using Zenject;


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

        [Inject]
        private IMinoSelector _minoSelector;
        
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
            _fieldModel.CellChanged += ChangeStaticSprite;
            IMino newMino =  _minoSelector.SelectRandomMino();
           
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
                    if (_fieldModel.IsRotatePossible(_minoModel.BlocksCoordinates[0],checkBlockCoordinates))
                    {
                        _minoModel.Rotate();
                    }

                    
                }
                
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                 Vector2Int distance = _fieldModel.CalculateDistanceToBottom(_minoModel.BlocksCoordinates);
                 _minoModel.Move(distance);
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
                isMovingPossible = _fieldModel.IsMovePossible(direction,_minoModel.BlocksCoordinates);
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
            _gridView.ClearMoveBlocks();
            _fieldModel.AddMino(_minoModel.BlocksCoordinates, _minoModel.Color);
            List<int> deleteLineIndexes = _fieldModel.FindFilledHorizontalLines();
            if (deleteLineIndexes.Count > 0)
            {
                
                _fieldModel.MoveLinesDown(deleteLineIndexes);
                
            }

            IMino newMino = _minoSelector.SelectRandomMino();
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
       

        private void SpawnMino(IMino mino)
        {
            

             MinoModel previousMinoModel = _minoModel;
             Color color = _minoSelector.SelectRandomColor();
            _minoModel = new MinoModel( mino, color);
            Debug.Log("ntcnntcn");
            _minoModel.BlocksCoordinates.CollectionChanged += MoveSprite;
            _minoModel.InitializeBlockCoordinates(_spawnCell);
            if (previousMinoModel != null)
            {
                previousMinoModel.BlocksCoordinates.CollectionChanged -= MoveSprite;
            }

        }
        private bool IsGameOver(IMino mino)
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
            if (eventArgs.Action == NotifyCollectionChangedAction.Add)
            {
                _gridView.SpawnSprite((Vector2Int)eventArgs.NewItems[0],_minoModel.Color );
            }
            else if(eventArgs.Action == NotifyCollectionChangedAction.Replace)
            {
                _gridView.MoveSprite((Vector2Int)eventArgs.NewItems[0], _minoModel.Color);
            }
            
        }

        private void ChangeStaticSprite(CellChangedEventArgs<ICell> eventArgs)
        {
            if (eventArgs.cellObject.Type == CellType.Empty)
            {
                _gridView.DeleteStaticSprite(eventArgs.x,eventArgs.y);
            }
            else if (eventArgs.cellObject.Type == CellType.Filled)
            {
                _gridView.MoveStaticSprite(eventArgs.x,eventArgs.y, eventArgs.cellObject.Color);
            }
        }


    }
}
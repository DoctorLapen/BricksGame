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

        [Inject]
        private IScoreModel _scoreModel;

        [Inject]
        private INextMinosView _nextMinosView;

        [Inject]
        private IScoreView _scoreView;

        [Inject]
        private IMinoInput _minoInput;

        [Inject]
        private IRecordSaver _recordSaver;

        [Inject]
        private IGameOverMenu _gameOverMenu;
        private const int FIRST_INDEX = 0;
        private const float TARGET_TIME_AMOUNT= 1f;
        

        [SerializeField]
        private float _minoFallSpeed ;

        [SerializeField]
        private string _saveFileName;
        

        [SerializeField]
        private Vector2Int _spawnCell;

        private MinoModel _minoModel;
        private float _currentTimeAmount = 0f;
        

        
        

        
        private void Start()
        {
            Pause.StartTime();
            _scoreModel.ScoreChange += _scoreView.DisplayScore;
            _fieldModel.CellChanged += ChangeStaticSprite;
            _minoSelector.MinoAdded += OnMinoAddedInSelector;
            IMino newMino =  _minoSelector.SelectRandomMino();
            SpawnMino(newMino);
          
        }

        private void OnMinoAddedInSelector(IList<IMino> minos)
        {
            _nextMinosView.SpawnMinos(minos);
           
        }

        private void Update()
        {
            if (!Pause.IsPause)
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

                ActionData actionData = _minoInput.DetectAction();
                if (actionData.isActionHappened)
                {

                    if (actionData.action == MoveAction.Rotate)
                    {
                        List<Vector2Int> checkBlockCoordinates = _minoModel.GetCheckBlockCoordinates();
                        if (_fieldModel.IsRotateInField(_minoModel.BlocksCoordinates[0], checkBlockCoordinates))
                        {
                            if (_fieldModel.IsRotatePossible(_minoModel.BlocksCoordinates[0], checkBlockCoordinates))
                            {
                                _minoModel.Rotate();
                            }
                        }
                    }
                    else if (actionData.action == MoveAction.ToBottomEnd)
                    {
                        Vector2Int distance = _fieldModel.CalculateDistanceToBottom(_minoModel.BlocksCoordinates);
                        _minoModel.Move(distance);
                        UpdateGameState();
                    }
                    else if (actionData.action == MoveAction.Right)
                    {
                        Vector2Int direction = new Vector2Int(1, 0);
                        MinoSide side = MinoSide.Right;
                        MoveMinoWithChecking(side, direction);
                    }
                    else if (actionData.action == MoveAction.Left)
                    {
                        Vector2Int direction = new Vector2Int(-1, 0);
                        MinoSide side = MinoSide.Left;
                        MoveMinoWithChecking(side, direction);
                    }
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
            else if(!isInField && (side == MinoSide.Left || side == MinoSide.Right))
            {
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
                _scoreModel.AddScore(deleteLineIndexes.Count);
                _fieldModel.MoveLinesDown(deleteLineIndexes);
                
            }

            IMino newMino = _minoSelector.SelectRandomMino();
            if (IsGameOver(newMino))
            {
                StartGameOver();
            }
            else
            {
                SpawnMino(newMino);
            }
        }

        private void StartGameOver()
        {
            Pause.StopTime();
            _gameOverMenu.ShowMenu();
            IScoreData recordData = _recordSaver.Load(_saveFileName);
            IScoreData scoreData = _scoreModel.Score;
            
            if(scoreData.Score > recordData.Score)
            {
                _recordSaver.Save(_saveFileName,scoreData);
                _gameOverMenu.ChangeScore(scoreData.Score,true);
            }
            else
            {
                _gameOverMenu.ChangeScore( scoreData.Score,false);
            }
        }
        private void SpawnMino(IMino mino)
        {
            MinoModel previousMinoModel = _minoModel;
            Color color = _minoSelector.SelectRandomColor();
            _minoModel = new MinoModel( mino, color);
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
                    return true;
                }
            }

            return false;
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
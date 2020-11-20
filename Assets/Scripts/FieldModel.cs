using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class FieldModel : IFieldModel
    {
        public event Action<CellChangedEventArgs<ICell>> CellChanged;
        private IMainGameSettings _mainGameSettings;
        private ICell[,] _field;

        public FieldModel(IMainGameSettings mainGameSettings)
        {
            _mainGameSettings = mainGameSettings;
            uint columns = mainGameSettings.ColumnAmount;
            uint rows = mainGameSettings.RowAmount;
            
            _field = new Cell[columns, rows];
            InitializeField();
        }

        public bool IsCellEmpty(uint x,uint y)
        {
            return _field[x, y].Type == CellType.Empty;
        }

        
        public ICell GetCell(int x, int y)
        {
            return _field[x, y];
        }

        public void AddMino(IList<Vector2Int> blocksCoordinates, Color minoColor)
        {
            foreach (Vector2Int block in blocksCoordinates)
            {
                ICell cell = new Cell(CellType.Filled, minoColor);
                MakeCell(block.x,block.y,cell);
            }
        }
       public bool IsMoveInField(Vector2Int direction,IList<Vector2Int> blocksCoordinates)
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
        public bool IsRotateInField( Vector2Int startBlock,List<Vector2Int> blocksCoordinates)
        {
            return  IsMoveInField(startBlock, blocksCoordinates);

        }
       public bool IsMovePossible(Vector2Int direction, IList<Vector2Int> blocksCoordinates)
        {
           
            
            foreach (Vector2Int coordinate in blocksCoordinates)
            {
                Vector2Int newCoordinate = coordinate + direction;
                bool isCellEmpty = IsCellEmpty((uint)newCoordinate.x,(uint) newCoordinate.y);
                if (!isCellEmpty)
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsRotatePossible( Vector2Int startBlock,IList<Vector2Int> blocksCoordinates)
        {
            return IsMovePossible(startBlock, blocksCoordinates);
        }
        public Vector2Int CalculateDistanceToBottom(IList<Vector2Int> blocksCoordinates)
        {
            Vector2Int direction = new Vector2Int(0,1);
            Vector2Int distance = Vector2Int.zero;
            List<Vector2Int> minoCoordinates = new List<Vector2Int>( blocksCoordinates);
            
            for (int y = minoCoordinates[0].y ;y < _mainGameSettings.RowAmount - 1;y++)
            {
                bool isInField = IsMoveInField(direction,minoCoordinates);
                bool isMovingPossible = false;
                if (isInField)
                {
                    isMovingPossible = IsMovePossible(direction,minoCoordinates);
                    if (isMovingPossible)
                    {
                        for (int index = 0; index < minoCoordinates.Count; index++)
                        {
                            minoCoordinates[index] += direction;
                        }
                        distance += direction;
                    }
                }
                if(!isInField || !isMovingPossible)
                {
                    break;
                }
            }
            return distance;
        }
        public List<int> FindFilledHorizontalLines()
        {
            List<int> lineIndexes = new List<int>();
            int startRowIndex =(int) _mainGameSettings.RowAmount - 1;
            for (int row = startRowIndex; row >= 0; row--)
            {
                for (int column = 0; column < _mainGameSettings.ColumnAmount; column++)
                {
                    if (IsCellEmpty((uint) column, (uint) row))
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
       
        public void MoveLinesDown(List<int> lineIndexes)
        {
            int linesDeleted = 0;
            foreach (int lineIndex in lineIndexes)
            {
                int emptyLineIndex = lineIndex + linesDeleted;
                for (int moveLineIndex = lineIndex - 1; moveLineIndex != -1; moveLineIndex--)
                {
                    for (int column = 0; column < _mainGameSettings.ColumnAmount; column++)
                    {
                        ICell movingDownCell = GetCell(column, moveLineIndex);

                        MakeCell(column, emptyLineIndex, movingDownCell); ;

                    }

                    emptyLineIndex--;
                    
                }

                linesDeleted++;
            }
        }
        private void MakeCell(int x, int y,ICell cell )
        {
            _field[x, y] = cell;
            CellChanged?.Invoke(new CellChangedEventArgs<ICell>(x,y,cell));
        }

        private void InitializeField()
        {
            for (int x = 0; x < _mainGameSettings.ColumnAmount; x++)
            {
                for (int y = 0; y < _mainGameSettings.RowAmount; y++)
                {
                    _field[x,y] = new Cell();
                }
            }
        }


    }

    
}
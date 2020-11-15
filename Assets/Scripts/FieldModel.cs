using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public class FieldModel : IFieldModel
    {
        private IMainGameSettings _mainGameSettings;
        private CellType[,] _field;

        public FieldModel(IMainGameSettings mainGameSettings)
        {
            _mainGameSettings = mainGameSettings;
            uint columns = mainGameSettings.ColumnAmount;
            uint rows = mainGameSettings.RowAmount;
            
            _field = new CellType[columns, rows];
        }

        public bool IsCellEmpty(uint x,uint y)
        {
            return _field[x, y] == CellType.Empty;
        }

        public void FillCell(uint x,uint y)
        {
            _field[x, y] = CellType.Filled;
        }

        public void MakeCellEmpty(uint x, uint y)
        {
            _field[x, y] = CellType.Empty;
        }
        public CellType GetCell(uint x, uint y)
        {
            return _field[x, y];
        }

        public void AddMino(IList<Vector2Int> blocksCoordinates)
        {
            foreach (Vector2Int block in blocksCoordinates)
            {
                FillCell((uint)block.x,(uint)block.y);
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
       
        
    }
}
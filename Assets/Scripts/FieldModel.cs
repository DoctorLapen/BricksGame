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
        
    }
}
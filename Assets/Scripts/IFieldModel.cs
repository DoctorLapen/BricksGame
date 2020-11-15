using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface IFieldModel
    {
        bool IsCellEmpty(uint x,uint y);
        void FillCell(uint x,uint y);
        void MakeCellEmpty(uint x, uint y);
        CellType GetCell(uint x, uint y);
        void AddMino(IList<Vector2Int> blocksCoordinates);
    }
}
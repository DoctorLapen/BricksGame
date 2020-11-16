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
        bool IsMoveInField(Vector2Int direction,IList<Vector2Int> blocksCoordinates);
        bool IsRotateInField( Vector2Int startBlock,List<Vector2Int> blocksCoordinates);
        bool IsMovePossible(Vector2Int direction, IList<Vector2Int> blocksCoordinates);
        bool IsRotatePossible( Vector2Int startBlock,IList<Vector2Int> blocksCoordinates);
        Vector2Int CalculateDistanceToBottom(IList<Vector2Int> blocksCoordinates);
    }
}
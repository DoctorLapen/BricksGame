using System.Data.OleDb;
using UnityEngine;

namespace SuperBricks
{
    public interface IFixedGridView
    {
        void SpawnSprite(Vector2Int coordinates);
        void MoveSprite(Vector2Int newCoordinates);
        void ClearMoveBlocks();
        void DeleteStaticSprite(int newX, int newY);
        
        void MoveStaticSprite(int newX, int newY);
    }

}
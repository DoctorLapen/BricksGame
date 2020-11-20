using System.Data.OleDb;
using UnityEngine;

namespace SuperBricks
{
    public interface IFixedGridView
    {
        void SpawnSprite(Vector2Int coordinates, Color spriteColor);
        void MoveSprite(Vector2Int newCoordinates, Color spriteColor);
        void ClearMoveBlocks();
        void DeleteStaticSprite(int newX, int newY);
        
        void MoveStaticSprite(int newX, int newY, Color spriteColor);
    }

}
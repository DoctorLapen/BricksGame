using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface INextMinosView
    {
        void SpawnMino(List<Vector2Int> blockCoordinates);
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface INextMinosView
    {
        void SpawnMinos(List<List<Vector2Int>>blockCoordinates);
    }
}
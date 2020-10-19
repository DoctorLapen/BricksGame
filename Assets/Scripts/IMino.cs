using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface IMino
    {
        List<Vector2Int> BlocksLocalCoordinates { get; }
        Dictionary<MinoBorder, List<Vector2Int>> BorderIndexes { get; }
    }
}
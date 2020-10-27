using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface IMino
    {
        List<Vector2Int> BlocksLocalCoordinates { get; }
         MinoBordersDictionary BorderIndexes { get; }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface IMino
    {
        Dictionary<MinoSide,List<Vector2Int>> BlocksLocalCoordinates { get; }
       
        Vector2Int AligmentValue { get; }

    }
}
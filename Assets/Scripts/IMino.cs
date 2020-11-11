using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface IMino
    {
        Vector2IntDictionary BlocksLocalCoordinates { get; }
         
    }
}
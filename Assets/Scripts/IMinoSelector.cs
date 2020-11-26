using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface IMinoSelector
    {
        IMino SelectRandomMino();
        Color SelectRandomColor();
        event Action<IList<IMino>> MinoAdded;
    }
}
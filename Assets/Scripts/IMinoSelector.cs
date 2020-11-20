using UnityEngine;

namespace SuperBricks
{
    public interface IMinoSelector
    {
        IMino SelectRandomMino();
        Color SelectRandomColor();
    }
}
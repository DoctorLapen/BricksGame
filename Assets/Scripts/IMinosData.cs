using UnityEngine;

namespace SuperBricks
{
    public interface IMinosData
    {
        Mino[] Minos { get; }
        Color[] MinoColors { get; }
    }
}
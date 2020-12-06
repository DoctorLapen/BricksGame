using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    public interface INextMinosView
    {
        void SpawnMinos(IList<IMino> minos);
    }
}
﻿using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    [CreateAssetMenu(fileName = "Mino", menuName = "MinoShape", order = 1)]
    public class Mino : ScriptableObject, IMino
    {
        [SerializeField]
        private List<Vector2Int> _blocksLocalCoordinates = new List<Vector2Int>();

        public List<Vector2Int> BlocksLocalCoordinates
        {
            get { return this._blocksLocalCoordinates; }
        }
        [SerializeField]
        private MinoBordersDictionary borderIndexes = new MinoBordersDictionary();

        public MinoBordersDictionary BorderIndexes => borderIndexes;
       

    }
}
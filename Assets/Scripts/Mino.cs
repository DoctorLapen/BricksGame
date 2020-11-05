using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    [CreateAssetMenu(fileName = "Mino", menuName = "MinoShape", order = 1)]
    public class Mino : ScriptableObject, IMino
    {
       
        [SerializeField]
        private MinoBordersDictionary borderIndexes = new MinoBordersDictionary();

        [SerializeField]
        private Vector2IntDictionary _blocksLocalCoordinates = new Vector2IntDictionary();


        public Vector2IntDictionary BlocksLocalCoordinates => _blocksLocalCoordinates;
        public MinoBordersDictionary BorderIndexes => borderIndexes;
       

    }
}
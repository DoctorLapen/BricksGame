using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperBricks
{
    [CreateAssetMenu(fileName = "Mino", menuName = "MinoShape", order = 1)]
    public class Mino : ScriptableObject, IMino
    {
        

        [SerializeField]
        private Vector2IntDictionary _blocksLocalCoordinates = new Vector2IntDictionary();

        private Dictionary<MinoSide, List<Vector2Int>> blocksLocalCoordinates = new Dictionary<MinoSide, List<Vector2Int>>();


        public Dictionary<MinoSide,List<Vector2Int>> BlocksLocalCoordinates => blocksLocalCoordinates;

        private void OnEnable()
        {
            foreach (KeyValuePair<MinoSide, Vector2IntList> pair in _blocksLocalCoordinates )
            {
                blocksLocalCoordinates.Add(pair.Key,pair.Value.List);
            }

        }

        public void AddPairInCreating(MinoSide key, Vector2IntList value)
        {
            _blocksLocalCoordinates.Add(key,value);
        }

    }
}
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace SuperBricks
{
    public class MainController : MonoBehaviour
    {
        [Inject]
        private IFixedGridView _gridView;
        
        private const int FIRST_INDEX = 0;
        [SerializeField]
        private Mino [] _minos;

        [SerializeField]
        private Vector2Int _spawnCell;

        
        

        
        private void Start()
        {
            var mino = SelectRandomMino();
            SpawnMino(mino);

        }

        private Mino SelectRandomMino()
        {
            int minoIndex = Random.Range(FIRST_INDEX, _minos.Length);
            return _minos[minoIndex];
        }

        private void SpawnMino(Mino mino)
        {
            foreach (Vector2Int localBlockCoordinates in mino.BlocksLocalCoordinates)
            {
                Vector2Int cell = _spawnCell + localBlockCoordinates;
                _gridView.SpawnSpriteInCell((uint)cell.x,(uint)cell.y);

            }
        }

    }
}
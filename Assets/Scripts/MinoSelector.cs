using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SuperBricks
{
    public class MinoSelector : IMinoSelector
    {
        public event Action<IList<IMino>> MinoAdded; 
        public event Action<Color> ColorAdded; 
        private  IMinosData _minosData;
        private Queue<IMino> _selectedMinos = new Queue<IMino>();
        private Queue<Color> _selectedColors = new Queue<Color>();
        

        public MinoSelector(IMinosData minosData)
        {
            _minosData = minosData;
            InitializeSelectedMinos();
        }
        public IMino SelectRandomMino()
        {
            IMino mino = _selectedMinos.Dequeue();
            IMino newMino = SelectRandomItem(_minosData.Minos);
            AddToMinoQueue(newMino);
            return mino;
        }

        public Color SelectRandomColor()
        {
             
            return SelectRandomItem(_minosData.MinoColors);
        }

        private T SelectRandomItem<T>(T[] items)
        {
            int itemIndex = Random.Range(0, items.Length);
            return items[itemIndex];
        }

        private void AddToMinoQueue(IMino mino)
        {
            _selectedMinos.Enqueue(mino);
            MinoAdded?.Invoke( new List<IMino>(_selectedMinos));
        }
        private void AddToColorQueue(Color color)
        {
            _selectedColors.Enqueue(color);
            ColorAdded?.Invoke(color);
        }

        private void InitializeSelectedMinos()
        {
            for (int c = 0; c < _minosData.AmountMinosToShow; c++)
            {
                IMino mino = SelectRandomItem(_minosData.Minos);
                _selectedMinos.Enqueue(mino);

            }
            MinoAdded?.Invoke( new List<IMino>(_selectedMinos));
        }
    }
}
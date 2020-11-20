using UnityEngine;

namespace SuperBricks
{
    public class MinoSelector : IMinoSelector
    {
        private  IMinosData _minosData;

        public MinoSelector(IMinosData minosData)
        {
            _minosData = minosData;
        }
        public IMino SelectRandomMino()
        {
            
            return SelectRandomItem(_minosData.Minos);
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
    }
}
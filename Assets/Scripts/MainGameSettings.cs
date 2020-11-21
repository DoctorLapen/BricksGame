using UnityEngine;

namespace SuperBricks
{
    [CreateAssetMenu(fileName = "MainGameSettings", menuName = "MainGameSettings", order = 0)]
    public class MainGameSettings : ScriptableObject, IMainGameSettings
    {
        [SerializeField]
        private uint _rowAmount;

        public uint RowAmount
        {
            get { return this._rowAmount; }
        }

        [SerializeField]
        private uint _columnAmount;

        public uint ColumnAmount
        {
            get { return this._columnAmount; }
        }

        [SerializeField]
        private uint _oneLineCost;


        public uint OneLineCost => _oneLineCost;
    }
}
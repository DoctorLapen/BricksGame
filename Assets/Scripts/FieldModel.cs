namespace SuperBricks
{
    public class FieldModel
    {
        private IMainGameSettings _mainGameSettings;
        private ICell[,] _field;

        public FieldModel(IMainGameSettings mainGameSettings)
        {
            _mainGameSettings = mainGameSettings;
            uint columns = mainGameSettings.ColumnAmount;
            uint rows = mainGameSettings.RowAmount;
            
            _field = new Cell[columns, rows];
        }
    }
}
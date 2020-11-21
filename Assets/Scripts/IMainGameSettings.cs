namespace SuperBricks
{
    public interface IMainGameSettings
    {
        uint RowAmount { get; }
        uint ColumnAmount { get; }
        uint OneLineCost { get; }
    }
}
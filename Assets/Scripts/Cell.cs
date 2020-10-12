namespace SuperBricks
{
    public class Cell:ICell
    {
        public CellType Type { get; }
        public Cell(CellType type)
        {
            Type = type;
        }

       
    }
}
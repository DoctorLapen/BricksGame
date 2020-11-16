namespace SuperBricks
{
    public class CellChangedEventArgs<T>
    {
        public int x;
        public int y;
        public T cellObject;

        public CellChangedEventArgs(int x, int y, T cellObject)
        {
            this.x = x;
            this.y = y;
            this.cellObject = cellObject;
        }
    }
}
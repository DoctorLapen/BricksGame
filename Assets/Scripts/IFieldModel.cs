namespace SuperBricks
{
    public interface IFieldModel
    {
        bool IsCellEmpty(uint x,uint y);
        void FillCell(uint x,uint y);
    }
}
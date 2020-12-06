namespace SuperBricks
{
    public interface IRecordSaver
    {
        IScoreData Load(string fileName);

        void Save(string fileName, IScoreData newRecord);


    }
}
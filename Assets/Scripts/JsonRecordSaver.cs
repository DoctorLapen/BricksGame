using System.IO;
using UnityEngine;

namespace SuperBricks
{
    public class JsonRecordSaver:IRecordSaver
    {
        public IScoreData Load(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                string loadedJsonDataString = File.ReadAllText(path);
 
                return JsonUtility.FromJson<ScoreData>(loadedJsonDataString);
            }
            return new ScoreData();
        }

        public void Save(string fileName, IScoreData newRecord)
        {
            string jsonDataString = JsonUtility.ToJson(newRecord, true);

            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, jsonDataString);
        }
    }
}
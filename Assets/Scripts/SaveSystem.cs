using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/level.fun";

    public static void SaveLevel()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        int? levelData = LevelData.getLastLevelLoaded();

        formatter.Serialize(stream, levelData);
        stream.Close();
    }

    public static int? LoadLevel()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            int? levelData = formatter.Deserialize(stream) as int?;
            stream.Close();

            return levelData;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}

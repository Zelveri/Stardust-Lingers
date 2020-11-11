using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Yarn.Unity;

public static class SaveHandler
{
    public static void SaveGameState (string filename="savestate")
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + filename + ".stardust";
        FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);

        SaveState data = new SaveState();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveState LoadGameState(string filename="savestate")
    {
        string path = Application.persistentDataPath + "/" + filename + ".stardust";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            SaveState data = formatter.Deserialize(stream) as SaveState;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Savefile not found in " + path);
            return null;
        }
    }
}

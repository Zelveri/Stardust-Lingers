using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Yarn.Unity;
using System;
using System.Text;
using Newtonsoft.Json;

/// <summary>
/// Handles serialization of gamestate
/// </summary>
public static class DataSaver
{
    // obsolete
    public static void SaveGameState (string filename="savestate")
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + filename + ".stardust";
        FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);

        SaveState data = new SaveState();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // obsolete
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

    /// <summary>
    /// Create json representation of serializable T type object
    /// </summary>
    /// <typeparam name="T">Type to json-ize</typeparam>
    /// <param name="dataToSave">object containtng the saveable data</param>
    /// <param name="dataFileName">name of save file</param>
    public static void saveData<T>(T dataToSave, string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Convert To Json then to bytes
        string jsonData = JsonConvert.SerializeObject(dataToSave,Formatting.Indented);
        byte[] jsonByte = Encoding.ASCII.GetBytes(jsonData);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
        }

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            Debug.Log("Saved Data to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To PlayerInfo Data to: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    /// <summary>
    /// Create T type object from json representation
    /// </summary>
    /// <typeparam name="T">Type to un-json-ize</typeparam>
    /// <param name="dataFileName">file name to load</param>
    public static T loadData<T>(string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return default(T);
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return default(T);
        }

        //Load saved Json
        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(tempPath);
            Debug.Log("Loaded Data from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to json string
        string jsonData = Encoding.ASCII.GetString(jsonByte);

        //Convert to Object
        //       object resultValue = JsonUtility.FromJson<T>(jsonData);
        T resultValue = JsonConvert.DeserializeObject<T>(jsonData);
        return resultValue; //(T)Convert.ChangeType(resultValue, typeof(T));
    }

    public static bool deleteData(string dataFileName)
    {
        bool success = false;

        //Load Data
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return false;
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return false;
        }

        try
        {
            File.Delete(tempPath);
            Debug.Log("Data deleted from: " + tempPath.Replace("/", "\\"));
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Delete Data: " + e.Message);
        }

        return success;
    }
}

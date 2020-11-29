using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to use DataSaver with SaveState
/// </summary>
public class SaveFileHandler : MonoBehaviour
{
    /// <summary>
    /// Save data to slot
    /// </summary>
    /// <param name="slot">slot number</param>
    public static void Save(int slot)
    {
        // called from save button in Menu with fixed slot
        // create new game state representation
        var saveData = new SaveState();
        saveData.GatherData();
        // save it to file
        DataSaver.SaveData(saveData, "save_slot_" + slot.ToString());
    }

    /// <summary>
    /// Load save data from slot
    /// </summary>
    /// <param name="slot">slot number</param>
    public static void Load(int slot)
    {
        // load sava  data
        SaveState save = DataSaver.LoadData<SaveState>("save_slot_" + slot.ToString());
        GameManager.dataController.LoadState(save);
    }

    public static bool CheckSaveFileExists(int slot)
    {
        return System.IO.File.Exists(Application.persistentDataPath + "/data/save_slot_" + slot.ToString() + ".txt");
    }
}

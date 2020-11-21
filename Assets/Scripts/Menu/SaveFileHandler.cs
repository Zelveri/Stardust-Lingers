﻿using System;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        DataSaver.saveData(saveData, "slot" + slot.ToString());
    }

    /// <summary>
    /// Load save data from slot
    /// </summary>
    /// <param name="slot">slot number</param>
    public static void Load(int slot)
    {
        // load sava  data
        SaveState save = DataSaver.loadData<SaveState>("slot" + slot.ToString());
        GameManager.dataController.LoadState(save);
    }
}

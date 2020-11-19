using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void Save(int slot)
    {
        // called from save button in Menu with fixed slot
        // create new game state representation
        var saveData = new SaveState();
        // save it to file
        DataSaver.saveData(saveData, "slot" + slot.ToString());
    }

    /// <summary>
    /// Load save data from slot
    /// </summary>
    /// <param name="slot">slot number</param>
    public void Load(int slot)
    {
        // load sava  data
       DataSaver.loadData<SaveState>("slot" + slot.ToString()).LoadSave();
    }
}

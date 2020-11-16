using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Save(int slot)
    {
        var saveData = new SaveState();
        // save array with all data
        DataSaver.saveData(saveData, "slot" + slot.ToString());
    }

    public void Load(int slot)
    {
        //SaveState save = DataSaver.LoadGameState("slot" + slot.ToString());
        //DataController.dialogueTracker.LoadState(save);
        // load sava  data
        SaveState saveData =  DataSaver.loadData<SaveState>("slot" + slot.ToString());

        // this will finalize the loading, apply the loaded settingss and will restart the dialogue
        GameManager.dataController.LoadState(saveData);
    }
}

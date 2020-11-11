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
        SaveHandler.SaveGameState("slot" + slot.ToString());
    }

    public void Load(int slot)
    {
        SaveState save = SaveHandler.LoadGameState("slot" + slot.ToString());
        DialogueTracker.dialogueTracker.LoadState(save);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

// marks class to be convertible to be written to file
[System.Serializable]
public class SaveState
{
    // all important game variables
    public string currentNode;
    public string prevNode;
    public string curNameTag;
    public string[] lines;
    public string backdrop;
    [SerializeField]
    public Dictionary<string, Yarn.Value> variables;
    // string[] choices;

    //public SaveState(string curNode = "Start", string prevNode = "", string[] lines = null, string[] choices = null, string nameTag="", string themeColor="Light")
    //{
    //    this.currentNode = curNode;
    //    this.prevNode = prevNode;
    //    this.lines = lines;
    //    //this.choices = choices;
    //    this.curNameTag = nameTag;
    //    this.themeColor = themeColor;
    //}


    public SaveState()
    {
        // get all the variables from the DataController
        this.currentNode = DataController.GetCurrentNode();
        this.prevNode = DataController.GetPrevNode();
        this.lines = DataController.GetLines().ToArray();
        //this.choices = choices;
        this.curNameTag = DataController.GetNametag();
        this.backdrop = DataController.GetBackdrop();
        this.variables = DataController.GetVariablesAsDict();
    }

    /// <summary>
    /// Apply given gamestate to game
    /// </summary>
    public void LoadSave()
    {
        // this will finalize the loading, apply the loaded settings and will restart the dialogue
        GameManager.dataController.LoadState(this);
    }
}

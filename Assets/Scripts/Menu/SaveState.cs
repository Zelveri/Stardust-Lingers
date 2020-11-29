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
    public int curScene;
    public string curMusic;
    public string[] curSounds;
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

    /// <summary>
    /// default ctor
    /// </summary>
    public SaveState()
    {
        this.currentNode = "";
        this.prevNode = "";
        this.lines = null;
        //this.choices = choices;
        this.curNameTag = "";
        this.backdrop = "";
        this.variables = null;
        this.curScene = -1;
        this.curSounds = null;
        this.curMusic = "";
    }

    /// <summary>
    /// fetch data from the control structures that hold it
    /// </summary>
    public void GatherData()
    {
        // get all the variables from the DataController
        this.currentNode = DataController.CurrentNode;
        this.prevNode = DataController.PrevousNode;
        this.lines = DataController.Lines;
        //this.choices = choices;
        this.curNameTag = DataController.Nametag;
        this.backdrop = DataController.Backdrop;
        this.variables = DataController.Variables;
        this.curScene = (int)SceneController.CurMainScene;
        this.curMusic = DataController.Music;
        this.curSounds = DataController.Sounds;
    }

}

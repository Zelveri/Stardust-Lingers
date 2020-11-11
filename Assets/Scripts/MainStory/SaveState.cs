using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[System.Serializable]
public class SaveState
{
    public string currentNode;
    public string prevNode;
    public string curNameTag;
    public string themeColor;
    public string[] lines;
    public string backdrop;
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

    public SaveState(DialogueTracker dlgTckr, InMemoryVariableStorage varStorage)
    {
        this.currentNode = dlgTckr.GetCurrentNode();
        this.prevNode = dlgTckr.GetPrevNode();
        this.lines = dlgTckr.GetLines().ToArray();
        //this.choices = choices;
        this.curNameTag = dlgTckr.GetNametag();
        this.themeColor = varStorage.GetValue("$theme_color").AsString;
        this.backdrop = dlgTckr.GetBackdrop();
    }
}

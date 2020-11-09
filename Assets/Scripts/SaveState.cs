using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveState
{
    string currentNode;
    string prevNode;
    string[] lines;
    string[] choices;

    SaveState(string curNode = "Start", string prevNode = "", string[] lines = null, string[] choices= null)
    {
        this.currentNode = curNode;
        this.prevNode = prevNode;
        this.lines = lines;
        this.choices = choices;
    }
}

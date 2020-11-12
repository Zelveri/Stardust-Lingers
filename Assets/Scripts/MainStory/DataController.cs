using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class DataController : MonoBehaviour
{
    public static DataController dialogueTracker;
    public static InMemoryVariableStorage variableStorage;
    public DialogueRunner dialogueRunner;
    public BackgroundChange backgroundChange;
    DialogueUI dlgUI;
    static List<string> lines;
    static string currentNode;
    static string prevNode;
    static string curNametag;
    static string backdrop;
    static bool lineIncomplete = true;

    // Start is called before the first frame update
    void Awake()
    {
        dialogueTracker = this;
        dlgUI = dialogueRunner.GetComponent<DialogueUI>();
        variableStorage = GameObject.Find("Variable Storage").GetComponent<InMemoryVariableStorage>();
        DontDestroyOnLoad(dialogueTracker);
        lines = new List<string>();
    }

    public void LineStart()
    {
        lineIncomplete = true;
    }

    public void LineEnd()
    {
        lineIncomplete = false;
    }

    public void AddLineToTracker(TMPro.TextMeshProUGUI dialogueTextContainer)
    {
        lines.Add(dialogueTextContainer.text);
    }

    public void AddLineToTracker(string text)
    {
        lines.Add(text);
    }

    public void LoadState(SaveState save)
    {
        lines = new List<string>(save.lines);
        curNametag = save.curNameTag;
        currentNode = save.currentNode;
        prevNode = save.prevNode;
        foreach (var entry in save.variables)
        {
            variableStorage.SetValue(entry.Key, entry.Value);
        }

        if (lineIncomplete) dlgUI.MarkLineComplete();
        StartCoroutine(backgroundChange.DoChangeFast(save.backdrop));
        dialogueRunner.StartDialogue(save.currentNode);
    }

    public void UpdateNametag(string text)
    {
        AddLineToTracker("\n" + text + ":");
    }

    public void UpdateCurrentNode()
    {
        currentNode = dialogueRunner.CurrentNodeName;
        AddLineToTracker("-------------------\nScene " + currentNode +":");
    }

    public void UpdateFinishedNode()
    {
        prevNode = dialogueRunner.CurrentNodeName;
        AddLineToTracker("-------------------");
    }

    public void UpdateBackdrop(string b)
    {
        backdrop = b;

    }

    public static List<string> GetLines()
    {
        return lines;
    }

    public static string GetCurrentNode()
    {
        return currentNode;
    }

    public static string GetPrevNode()
    {
        return prevNode;
    }

    public static string GetNametag()
    {
        return curNametag;
    }
    public static string GetBackdrop()
    {
        return backdrop;
    }

    public static Dictionary<string,Yarn.Value> GetVariablesAsDict()
    {
        var dict = new Dictionary<string, Yarn.Value>();
        foreach (var entry in variableStorage)
        {
            dict.Add(entry.Key, entry.Value);
        }
        return dict;
    }
}

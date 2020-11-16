﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class DataController : MonoBehaviour
{
    public static InMemoryVariableStorage variableStorage;
    public DialogueRunner dialogueRunner;
    public BackgroundChangeHandler backgroundChange;
    DialogueUI dlgUI;
    static List<string> lines;
    static string currentNode;
    static string prevNode;
    static string curNametag;
    static string backdrop;
    static bool lineIncomplete = true;

    // Start is called before the first frame update
    public void Awake()
    {
        dlgUI = SingletonDialogueUI.Instance;
        variableStorage = SingletonVariableStorage.Instance;
        lines = new List<string>();
    }

    private void Update()
    {

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
        StartCoroutine(backgroundChange.DoChangeFast(save.backdrop, null));
        dialogueRunner.StartDialogue(save.currentNode);
    }

    public void UpdateNametag(string text)
    {
        AddLineToTracker("\n" + text + ":");
    }

    public void UpdateCurrentNode(string node)
    {
        currentNode = node;
        AddLineToTracker("-------------------\nScene " + currentNode +":");
    }

    public void UpdateFinishedNode(string node)
    {
        prevNode = node;
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
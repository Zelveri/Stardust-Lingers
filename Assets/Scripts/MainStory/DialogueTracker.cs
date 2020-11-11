using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class DialogueTracker : MonoBehaviour
{
    public static DialogueTracker dialogueTracker;
    public DialogueRunner dialogueRunner;
    public BackgroundChange backgroundChange;
    static List<string> lines;
    static string currentNode;
    static string prevNode;
    static string curNametag;
    static string backdrop;

    // Start is called before the first frame update
    void Awake()
    {
        dialogueTracker = this;
        DontDestroyOnLoad(dialogueTracker);
        DontDestroyOnLoad(dialogueRunner);
        DontDestroyOnLoad(backgroundChange);
        lines = new List<string>();
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
        dialogueRunner.Stop();
        dialogueRunner.StopAllCoroutines();
        StartCoroutine(backgroundChange.DoChangeFast(save.backdrop));
        dialogueRunner.startNode = save.currentNode;
        dialogueRunner.ResetDialogue();
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
}

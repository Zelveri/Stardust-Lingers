using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class DialogueTracker : MonoBehaviour
{

    public DialogueRunner dialogueRunner;
    public BackgroundChange BackgroundChange;
    List<string> lines;
    string currentNode;
    string prevNode;
    string curNametag;
    string backdrop;

    // Start is called before the first frame update
    void Awake()
    {
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

    public void LoadState(string[] arr, string cnd, string pnd, string cnt, string bckdrp)
    {
        lines = new List<string>(arr);
        curNametag = cnt;
        currentNode = cnd;
        prevNode = pnd;
        dialogueRunner.Stop();
        dialogueRunner.StopAllCoroutines();
        StartCoroutine(BackgroundChange.DoChangeFast(bckdrp));
        dialogueRunner.startNode = cnd;
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

    public List<string> GetLines()
    {
        return lines;
    }

    public string GetCurrentNode()
    {
        return currentNode;
    }

    public string GetPrevNode()
    {
        return prevNode;
    }

    public string GetNametag()
    {
        return curNametag;
    }
    public string GetBackdrop()
    {
        return backdrop;
    }
}

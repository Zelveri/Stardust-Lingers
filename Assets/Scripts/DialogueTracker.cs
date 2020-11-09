using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueTracker : MonoBehaviour
{

    public DialogueRunner dialogueRunner;
    List<string> lines;
    string currentNode;
    string prevNode;

    // Start is called before the first frame update
    void Awake()
    {
        lines = new List<string>();
    }

    public void AddLineToTracker(string line)
    {
        lines.Add(line);
    }

    public void UpdateCurrentNode(string node)
    {
        currentNode = node;
    }

    public void UpdateFinishedNode(string node)
    {
        prevNode = node;
    }
}

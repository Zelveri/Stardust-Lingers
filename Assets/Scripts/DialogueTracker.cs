using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
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

    public void AddLineToTracker(TMPro.TextMeshProUGUI dialogueTextContainer)
    {
        lines.Add(dialogueTextContainer.text);
    }

    public void UpdateCurrentNode()
    {
        currentNode = dialogueRunner.CurrentNodeName;
    }

    public void UpdateFinishedNode()
    {
        prevNode = dialogueRunner.CurrentNodeName;
    }
}

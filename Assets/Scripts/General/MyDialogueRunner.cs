using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MyDialogueRunner : DialogueRunner
{
    // dialoguerunner overload, to embed GameManager and dataController
    // registers events with the DataController
    private void Awake()
    {
        GameManager.RegisterDialogueRunner(this);
        // gamemanager adds variableStorage and dialogueUI
        onDialogueComplete.AddListener(OnDialogueComplete);
        onNodeStart.AddListener(OnNodeStart);
        onNodeComplete.AddListener(OnNodeComplete);
    }

    public void OnNodeComplete(string node)
    {
        GameManager.dataController.UpdateFinishedNode(node);
        GameManager.dialogueUI.OnNodeComplete.Invoke(node);
    }

    public void OnNodeStart(string node)
    {
        GameManager.dataController.UpdateCurrentNode(node);
        GameManager.dialogueUI.OnNodeStart.Invoke(node);
    }


    void OnDialogueComplete()
    {
        GameManager.dialogueUI.OnDialogueComplete.Invoke();
    }
}

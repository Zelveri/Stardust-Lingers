using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MyDialogueRunner : DialogueRunner
{

    List<int> loadedPrograms;
    // dialoguerunner overload, to embed GameManager and dataController
    // registers events with the DataController
    private void Awake()
    {
        GameManager.RegisterDialogueRunner(this);
        loadedPrograms = new List<int>();
        // gamemanager adds variableStorage and dialogueUI
        // mydialogueUI expands these events
        onDialogueComplete.AddListener(OnDialogueComplete);
        onNodeStart.AddListener(OnNodeStart);
        onNodeComplete.AddListener(OnNodeComplete);
    }

    public new void Add(YarnProgram program)
    {
        if (!loadedPrograms.Contains((int)SceneController.CurMainScene))
        {
            loadedPrograms.Add((int)SceneController.CurMainScene);
            base.Add(program);
        }
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

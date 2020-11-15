using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DataInterfaceBehaviour : MonoBehaviour
{
    DataController dataController;

    string curLine = "";

    private void Awake()
    {
        dataController = DataController.dataController;
    }

    public void OnLineStart()
    {
        dataController.LineStart();
    }

    public void OnLineUpdate(string text)
    {
        curLine = text;
    }

    public void OnLineEnd()
    {
        dataController.AddLineToTracker(curLine);
        dataController.LineEnd();
    }

    public void AddLineToTracker(TMPro.TextMeshProUGUI dialogueTextContainer)
    {
        dataController.AddLineToTracker(dialogueTextContainer);
    }

    public void AddLineToTracker(string text)
    {
        dataController.AddLineToTracker(text);
    }

    public void UpdateNametag(string text)
    {
        dataController.UpdateNametag(text);
    }

    public void OnNodeStart(string node)
    {
        dataController.UpdateCurrentNode(node);
    }

    public void OnNodeComplete(string node)
    {
        dataController.UpdateFinishedNode(node);
    }

    public void UpdateBackdrop(string b)
    {
        dataController.UpdateBackdrop(b);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MyDialogueUI : DialogueUI
{
    // overload of DialogueUI, that registers itself to the GameManager on load
    // also connects all the events to the DataController
    string curLine = "";
    private void Awake()
    {
        GameManager.RegisterDialogueUI(this);
        onCommand.AddListener(OnCommand);
        onDialogueStart.AddListener(OnDialogueStart);
        onDialogueEnd.AddListener(OnDialogueEnd);
        onLineStart.AddListener(OnLineStart);
        onLineUpdate.AddListener(OnLineUpdate);
        onLineFinishDisplaying.AddListener(OnLineFinishDisplaying);
        onLineEnd.AddListener(OnLineEnd);
        onOptionsStart.AddListener(OnOptionsStart);
        onOptionsEnd.AddListener(OnOptionsEnd);
    }

    public void OnCommand(string command)
    {

    }

    public void OnDialogueComplete()
    {

    }

    public void OnDialogueEnd()
    {

    }

    public void OnDialogueStart()
    {

    }

    public void OnLineEnd()
    {
        GameManager.dataController.LineEnd();
    }

    public void OnLineFinishDisplaying()
    {
        GameManager.dataController.AddLineToTracker(curLine);
    }

    public void OnLineStart()
    {
        GameManager.dataController.LineStart();
    }

    public void OnLineUpdate(string line)
    {
        curLine = line;
    }

    public void OnOptionsEnd()
    {

    }

    public void OnOptionsStart()
    {

    }
}

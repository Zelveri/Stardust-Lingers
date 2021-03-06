﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;
using UnityEngine.Events;

public class MyDialogueUI : DialogueUI
{
    // overload of DialogueUI, that registers itself to the GameManager on load
    // also connects all the events to the DataController
    public YarnProgram[] yarnScripts;
    public string startNode;
    public bool startAutomatically;

    public DialogueRunner.StringUnityEvent OnNodeStart;
    public DialogueRunner.StringUnityEvent OnNodeComplete;
    public UnityEvent OnDialogueComplete;

    string curLine = "";
    private void Awake()
    {
        // register this instance with gamemanager
        GameManager.RegisterDialogueUI(this);
        if(GameManager.dialogueRunner != null)
        {
            // load yarn scripts given in inspector
            foreach (var elm in yarnScripts)
            {
                GameManager.dialogueRunner.Add(elm);
            }
            // set start node as given
            GameManager.dialogueRunner.startNode = startNode;
        }

        if (!PlayerPrefs.HasKey("text_speed")) PlayerPrefs.SetFloat("text_speed", 0.025f);

        if (dialogueContainer) dialogueContainer.SetActive(false);

        // register default event handlers ( add to any handlers given in inspector )
        onCommand.AddListener(OnCommand);
        onDialogueStart.AddListener(OnDialogueStart);
        onDialogueEnd.AddListener(OnDialogueEnd);
        onLineStart.AddListener(OnLineStart);
        onLineUpdate.AddListener(OnLineUpdate);
        onLineFinishDisplaying.AddListener(OnLineFinishDisplaying);
        onLineEnd.AddListener(OnLineEnd);
        onOptionsStart.AddListener(OnOptionsStart);
        onOptionsEnd.AddListener(OnOptionsEnd);

        GameManager.OnPrefsChanged.AddListener(UpdateSettings);
    }

    private void UpdateSettings()
    {
        // update text speed from menu
        float speed = PlayerPrefs.GetFloat("text_speed");
        textSpeed = speed;
    }

    // The functions below are all listeners to the event of the same name from the base class

    public void OnCommand(string command)
    {
        Debug.LogWarning("DialogueRunner: Command \"" + command + "\" not registered!");
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

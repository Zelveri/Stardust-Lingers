using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameManager : SingletonTemplate<GameManager>
{
    public static DataController dataController;
    public static InMemoryVariableStorage variableStorage;

    // should register themselves
    public static DialogueRunner dialogueRunner;
    public static DialogueUI dialogueUI;

    public static SceneController sceneController;
    public static InputHandlerBehaviour input;



    public override void Awake()
    {
        // GameManager script will load last ( set in Project settings )
        base.Awake();
        variableStorage = gameObject.transform.Find("VariableStorage").gameObject.GetComponent<InMemoryVariableStorage>();
        //dialogueRunner = gameObject.transform.Find("DialogueRunner").gameObject.GetComponent<DialogueRunner>();
        // dialogueRunner registers itself on module load, Project settings state
        dialogueRunner.variableStorage = variableStorage;
        dataController = gameObject.transform.Find("DataController").gameObject.GetComponent<DataController>();
        
        sceneController = gameObject.transform.Find("SceneController").gameObject.GetComponent<SceneController>();
        input = gameObject.transform.Find("InputHandler").gameObject.GetComponent<InputHandlerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void RegisterDialogueUI(MyDialogueUI ui)
    {
        dialogueUI = ui;
        // assigns itself to the current dialogueRunner if exists, does not on first load, only afterwards
        if (dialogueRunner != null) dialogueRunner.dialogueUI = ui;
    }

    public static void RegisterDialogueRunner(MyDialogueRunner runner)
    {
        dialogueRunner = runner;
        // if dialogueUI was initialized before dialogueRunner we assign it here
        if (dialogueUI != null) dialogueRunner.dialogueUI = dialogueUI;
    }
}

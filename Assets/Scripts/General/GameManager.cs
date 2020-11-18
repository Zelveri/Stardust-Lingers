using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameManager : SingletonTemplate<GameManager>
{
    public static DataController dataController;
    public static InMemoryVariableStorage variableStorage;
    public static DialogueRunner dialogueRunner;
    public static DialogueUI dialogueUI;

    public static SceneController sceneController;



    public override void Awake()
    {
        base.Awake();
        dataController = gameObject.transform.Find("Data Controller").gameObject.GetComponent<DataController>();
        variableStorage = gameObject.transform.Find("Variable Storage").gameObject.GetComponent<InMemoryVariableStorage>();
        dialogueRunner = gameObject.transform.Find("Dialogue Runner").gameObject.GetComponent<DialogueRunner>();
        dialogueUI = gameObject.transform.Find("Dialogue Runner").gameObject.GetComponent<DialogueUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ConnectToInterface(IDialogueInterface dI)
    {
        dialogueRunner.onDialogueComplete.AddListener(dI.OnDialogueComplete);
        dialogueRunner.onNodeStart.AddListener(dI.OnNodeStart);
        dialogueRunner.onNodeComplete.AddListener(dI.OnNodeComplete);

        dialogueUI.onCommand.AddListener(dI.OnCommand);
        dialogueUI.onDialogueStart.AddListener(dI.OnDialogueStart);
        dialogueUI.onDialogueEnd.AddListener(dI.OnDialogueEnd);
        dialogueUI.onLineStart.AddListener(dI.OnLineStart);
        dialogueUI.onLineUpdate.AddListener(dI.OnLineUpdate);
        dialogueUI.onLineFinishDisplaying.AddListener(dI.OnLineFinishDisplaying);
        dialogueUI.onLineEnd.AddListener(dI.OnLineEnd);
        dialogueUI.onOptionsStart.AddListener(dI.OnOptionsStart);
        dialogueUI.onOptionsEnd.AddListener(dI.OnOptionsEnd);

    }
}

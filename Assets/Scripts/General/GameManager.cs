#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;


// singleton object that persists troughout the game
// non static member access with .Instance
public class GameManager : SingletonTemplate<GameManager>
{
    // persistent objects
    public static DataController dataController;
    public static InMemoryVariableStorage variableStorage;
    public static SceneController sceneController;
    public static InputHandlerBehaviour input;
    public static MyDialogueRunner dialogueRunner;
    public static SoundEffectsBehaviour soundEffects;

    // new instance per scene, should register itself
    public static MyDialogueUI dialogueUI;

    /// <summary>
    /// called when menu changes prefs 
    /// called from SceneController.ReturnToStory()
    /// </summary>
    public static UnityEvent OnPrefsChanged = new UnityEvent();

    public override void Awake()
    {
        // GameManager script will load last ( set in Project settings )
        base.Awake();
        // get all the persistent scripts from child game objects
        variableStorage = gameObject.transform.Find("VariableStorage").gameObject.GetComponent<InMemoryVariableStorage>();
        dataController = gameObject.transform.Find("DataController").gameObject.GetComponent<DataController>();
        sceneController = gameObject.transform.Find("SceneController").gameObject.GetComponent<SceneController>();
        input = gameObject.transform.Find("InputHandler").gameObject.GetComponent<InputHandlerBehaviour>();
        soundEffects = gameObject.transform.Find("SoundEffects").gameObject.GetComponent<SoundEffectsBehaviour>();

        // dialogueRunner registers itself on module load ( happens before GameManager laods)
        dialogueRunner.variableStorage = variableStorage;
    }

    // gets called when MyDialogueUI is initialized
    public static void RegisterDialogueUI(MyDialogueUI ui)
    {
        dialogueUI = ui;
        // assigns itself to the current dialogueRunner if exists, does not on first load, only afterwards
        if (dialogueRunner != null) dialogueRunner.dialogueUI = ui;
    }

    // gets calles, when MyDialogueRunner is initialized
    public static void RegisterDialogueRunner(MyDialogueRunner runner)
    {
        dialogueRunner = runner;
        // if dialogueUI was initialized before dialogueRunner we assign it here
        if (dialogueUI != null) dialogueRunner.dialogueUI = dialogueUI;
    }

    public static void Quit()
    {
        Application.Quit();
    }
}

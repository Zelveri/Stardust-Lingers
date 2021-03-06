﻿#define DEBUG

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
    public static MusicPlayerBehaviour musicPlayer;

    // new instance per scene, should register itself
    public static MyDialogueUI dialogueUI;
    // returns current active transition handler for scene loading
    public static TransitionHandler TransitionHandler
    {
        get { return transitionHandlers.Peek(); }
    }
    //data structure to keep track of the current active transition handler
    private static readonly Stack<TransitionHandler> transitionHandlers = new Stack<TransitionHandler>();

    /// <summary>
    /// called when menu changes prefs 
    /// called from SceneController.ReturnToStory()
    /// </summary>
    public static UnityEvent OnPrefsChanged = new UnityEvent();

    public static UnityEvent OnVolumeChanged = new UnityEvent();

    public static UnityEvent OnMenuClose = new UnityEvent();

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
        musicPlayer = gameObject.transform.Find("MusicPlayer").gameObject.GetComponent<MusicPlayerBehaviour>();

        // dialogueRunner registers itself on module load ( happens before GameManager laods)
        dialogueRunner.variableStorage = variableStorage;

        // save menu prefs to disk when manu is closed
        OnPrefsChanged.AddListener(SavePrefs);

        sceneController.InitialTitleLoad();
    }

    //public void Start()
    //{
    //    StartCoroutine(TransitionHandler.SceneFadeIn());
    //}

    // gets called when MyDialogueUI is initialized
    public static void RegisterDialogueUI(MyDialogueUI ui)
    {
        dialogueUI = ui;
        // assigns itself to the current dialogueRunner if exists, does not on first load, only afterwards
        if (dialogueRunner) dialogueRunner.dialogueUI = ui;
    }

    // gets calles, when MyDialogueRunner is initialized
    public static void RegisterDialogueRunner(MyDialogueRunner runner)
    {
        dialogueRunner = runner;
        // if dialogueUI was initialized before dialogueRunner we assign it here
        if (dialogueUI) dialogueRunner.dialogueUI = dialogueUI;
    }

    // gets called when transition object initializes
    public static void RegisterTransitionHandler(TransitionHandler trans)
    {
        transitionHandlers.Push(trans);
    }

    public static void UnregisterTransitionHandler()
    {
        if(transitionHandlers.Count > 0) _ = transitionHandlers.Pop();
    }

    public static void ClearTransitionHandlers()
    {
        if (transitionHandlers.Count > 0) transitionHandlers.Clear();
    }


    public void SavePrefs()
    {
        PlayerPrefs.Save();
    }

    public static void Quit()
    {
        Application.Quit();
    }

    // non static fcn just for the main screen quit button because iT DoEs NoT LiKe StaTiC FunCTioNs
    public void Exit()
    {
        GameManager.Quit();
    }
}

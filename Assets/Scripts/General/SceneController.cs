using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn;
using Yarn.Unity;

public class SceneController : MonoBehaviour
{
    //public Dialogue dialogue;
    public enum Scenes
    {
        main=0,
        Menus,
        Log,
        Story,
        Phone_One,
        Meeting,
        Walk,
        Unknown=999999
    }

    static Scenes CurActiveScene = Scenes.main;
    public static Scenes CurMainScene = Scenes.main;
    Scenes OldScene = 0;

    public static bool SceneIsLoading { get; set; }

    public static bool IsMainMenu
    {
        get { return CurMainScene == Scenes.main; }
    }

    // start the dialogue runner if the scene has loaded, set in DataController.DoStateLoad
    public bool startDialogueOnLoad = true;


    private void Awake()
    {
        //dialogue = GameObject.Find("Dialogue Runner").GetComponent<DialogueRunner>().Dialogue;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneIsLoading = false;

    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex >= (int)Scenes.Story)
        {
            if (GameManager.dialogueUI.startAutomatically && startDialogueOnLoad) GameManager.dialogueRunner.StartDialogue();
            SceneIsLoading = false;
            startDialogueOnLoad = true;
        }
    }

    public void StartStory()
    {
        ReturnToStory();
        SceneLoad(Scenes.Story);
    }

    public void ScenePhone()
    {
        ReturnToStory();
        SceneLoad(Scenes.Phone_One);
    }

    public void SceneMeeting()
    {
        ReturnToStory();
        SceneLoad(Scenes.Meeting);
    }

    public void LoadScene(string sceneName)
    {
        try
        {
            if (Enum.TryParse<Scenes>(sceneName, out Scenes scene))
            {
                //ReturnToStory();
                SceneLoad(scene);
            }
            else
            {
                //ReturnToStory();
                Debug.LogWarning("Scene \"" + sceneName + "\" not registered with the SceneController!");
                SceneLoad(sceneName);
            }
        }
        catch(ArgumentException  ex)
        {
            Debug.LogError("LoadScene: No scene named \"" + sceneName + "\" exists!\n" + ex.Message);
        }
    }

    public IEnumerator LoadScene(int scene)
    {

        if (Enum.IsDefined(typeof(Scenes), scene))
        {
            GameManager.soundEffects.StopAll();
            GameManager.musicPlayer.Stop();
            yield return StartCoroutine(DoSceneLoad((Scenes)scene));
        }
        else
        {
            Debug.LogWarning("Load Scene: No scene with value {" + scene.ToString() + "} ");
        }
    }


    // returns to previous scene
    public void ReturnToStory()
    {
        GameManager.OnMenuClose.Invoke();
        StartCoroutine(DoReturn());
    }

    IEnumerator DoReturn()
    {
        if (CurActiveScene < Scenes.Story && CurActiveScene > 0)
        {
            yield return StartCoroutine(GameManager.TransitionHandler.SceneFadeOut());
            var op = SceneManager.UnloadSceneAsync((int)CurActiveScene);
            yield return new WaitUntil(() => op.isDone);
            UndoScenePreps();
            yield return StartCoroutine(GameManager.TransitionHandler.SceneFadeIn());
            Time.timeScale = 1;
            // on menu exit, cause settings reload
            if (CurActiveScene == Scenes.Menus) GameManager.OnPrefsChanged.Invoke();
            CurActiveScene = OldScene;
            if (GameManager.dialogueUI && GameManager.dialogueUI.dialogueContainer) GameManager.dialogueUI.dialogueContainer.SetActive(true);
        }
    }

    /// <summary>
    /// Toggles the Menus Scene
    /// </summary>
    public void ToggleMenu()
    {
        if (CurActiveScene >= Scenes.Story || CurActiveScene == 0)
        {
            GameManager.soundEffects.PauseAll();
            GameManager.musicPlayer.Pause();
            OverlaySceneLoad(Scenes.Menus);
        }
        else
        {
            ReturnToStory();
            GameManager.soundEffects.UnPauseAll();
            GameManager.musicPlayer.UnPause();
        }
    }

    /// <summary>
    /// Will open the log if current scene is not a menu scene
    /// </summary>
    public void ToggleLog()
    {
        if (CurActiveScene != Scenes.Log && CurActiveScene >= Scenes.Story)
        {
            OverlaySceneLoad(Scenes.Log);
        }
        else if(CurActiveScene != Scenes.Menus)
        {
            ReturnToStory();
        }
    }


    /// <summary>
    /// Prepare current scene for additional scene load
    /// </summary>
    void DoScenePreps()
    {
        // disable main scene audio listener
        //GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
        if(GameManager.dialogueUI.dialogueContainer) GameManager.dialogueUI.dialogueContainer.SetActive(false);
    }
    /// <summary>
    /// Reset scene after returning from overlay
    /// </summary>
    void UndoScenePreps()
    {
        // disable main scene audio listener
        //GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = true;
        if(GameManager.dialogueUI.dialogueContainer) GameManager.dialogueUI.dialogueContainer.SetActive(true);
    }


    /// <summary>
    /// Load the given scene
    /// </summary>
    /// <param name="scene">scene to load</param>
    public void SceneLoad(string scene)
    {
        SceneIsLoading = true;
        DoScenePreps();
        
        SceneManager.LoadScene(scene);
        CurActiveScene = Scenes.Unknown;
    }

    /// <summary>
    /// Load the given scene
    /// </summary>
    /// <param name="scene">scene to load</param>
    public void SceneLoad(Scenes scene)
    {
        StartCoroutine(DoSceneLoad(scene));
    }

    IEnumerator DoSceneLoad(Scenes scene)
    {

        SceneIsLoading = true;
        DoScenePreps();
        if (CurActiveScene == Scenes.main || CurActiveScene == Scenes.Menus)
        {
            yield return StartCoroutine(GameManager.TransitionHandler.SceneFadeOut());
            // menus pauses game, return to normal time if loading a savefile
            Time.timeScale = 1;
        }
        GameManager.ClearTransitionHandlers();
        //if (CurActiveScene == Scenes.Menus)
        //{
        //    var async_op = SceneManager.UnloadSceneAsync((int)CurMainScene);
        //    yield return new WaitUntil(() => async_op.isDone);
        //}
        SceneManager.LoadScene((int)scene);
        if (scene == Scenes.main)
        {
            yield return StartCoroutine(GameManager.TransitionHandler.SceneFadeIn());
        }
        CurActiveScene = scene;
        CurMainScene = scene;

    }

    /// <summary>
    /// Load scene additive
    /// </summary>
    /// <param name="name">scene to load</param>
    public void OverlaySceneLoad(string name)
    {
        // parse string to enum
        Scenes scene = (Scenes)Enum.Parse(typeof(Scenes), name);
        OverlaySceneLoad(scene);
    }

    /// <summary>
    /// load scene additive, pause game
    /// </summary>
    /// <param name="scene"></param>
    public void OverlaySceneLoad(Scenes scene)
    {
        StartCoroutine(DoOverlaySceneLoad(scene));
    }

    IEnumerator DoOverlaySceneLoad(Scenes scene)
    {

        DoScenePreps();
        // pause game
        Time.timeScale = 0;
        //dialogue.Stop();
        OldScene = CurActiveScene;
            
        yield return StartCoroutine(GameManager.TransitionHandler.SceneFadeOut());
        SceneManager.LoadScene((int)scene, LoadSceneMode.Additive);
        yield return StartCoroutine(GameManager.TransitionHandler.SceneFadeIn());
        CurActiveScene = scene;
    }

    public void OverlaySceneLoad(int scene)
    {
        OverlaySceneLoad((Scenes)scene);
    }

    /// <summary>
    /// toggle additive scene
    /// </summary>
    /// <param name="name"></param>
    public void ToggleOverlayScene(string name)
    {
        var scene = (Scenes)Enum.Parse(typeof(Scenes), name);
        if (CurActiveScene != scene)
        {
            OverlaySceneLoad(scene);
        }
        else
        {
            ReturnToStory();
        }
    }
}

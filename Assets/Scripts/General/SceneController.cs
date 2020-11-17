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
        Phone,
        Meeting
    }

    static Scenes CurActiveScene = Scenes.main;
    Scenes OldScene = 0;

    private void Awake()
    {
        //dialogue = GameObject.Find("Dialogue Runner").GetComponent<DialogueRunner>().Dialogue;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex >= (int)Scenes.Story)
        {
            GameManager.dialogueRunner.StartDialogue();
        }
    }

    public void StartStory()
    {
        ReturnToStory();
        GameManager.dialogueRunner.startNode = "Start";
        SceneLoad(Scenes.Story);
    }

    public void ScenePhone()
    {
        ReturnToStory();
        GameManager.dialogueRunner.startNode = "Phone_Start";
        SceneLoad(Scenes.Phone);
    }


    // returns to previous scene
    public void ReturnToStory()
    {
        if (CurActiveScene < Scenes.Story && CurActiveScene > 0)
        {
            SceneManager.UnloadSceneAsync((int)CurActiveScene);
            GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = true;
            Time.timeScale = 1;
            //dialogue.Continue();
            CurActiveScene = OldScene;
        }
    }

    /// <summary>
    /// Toggles the Menus Scene
    /// </summary>
    public void ToggleMenu()
    {
        if (CurActiveScene >= Scenes.Story)
        {
            OverlaySceneLoad(Scenes.Menus);
        }
        else
        {
            ReturnToStory();
        }
    }

    /// <summary>
    /// Will open the log if current scene is not a menu scene
    /// </summary>
    public void TryOpenLog()
    {
        if (CurActiveScene >= Scenes.Story)
        {
            OverlaySceneLoad(Scenes.Log);
        }
    }


    /// <summary>
    /// Prepare current scene for additional scene load, by disabling Audio Listener
    /// </summary>
    void DoScenePreps()
    {
        // disable main scene audio listener
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
    }

    /// <summary>
    /// Load the given scene
    /// </summary>
    /// <param name="scene">scene to load</param>
    public void SceneLoad(Scenes scene)
    {
        if (scene == Scenes.main) { ReturnToStory(); }
        else
        {
            DoScenePreps();
            //dialogue.Stop
            SceneManager.LoadScene((int)scene);
            //SceneManager.UnloadSceneAsync((int)CurActiveScene);
            CurActiveScene = scene;
        }
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
        if (scene == Scenes.main) { ReturnToStory(); }
        else
        {
            DoScenePreps();
            Time.timeScale = 0; // pause game
            //dialogue.Stop();
            OldScene = CurActiveScene;
            SceneManager.LoadScene((int)scene, LoadSceneMode.Additive);
            CurActiveScene = scene;
        }
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

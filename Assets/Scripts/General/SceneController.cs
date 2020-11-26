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
    public static Scenes CurMainScene = Scenes.Unknown;
    Scenes OldScene = 0;

    public static bool SceneIsLoading { get; set; }

    // start the dialogue runner if the scene has loaded, set in DataController.DoStateLoad
    public bool startDialogueOnLoad = true;


    private void Awake()
    {
        //dialogue = GameObject.Find("Dialogue Runner").GetComponent<DialogueRunner>().Dialogue;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneIsLoading = false;

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
            if(GameManager.dialogueUI.startAutomatically && startDialogueOnLoad) GameManager.dialogueRunner.StartDialogue();
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
            Scenes scene;
            if (Enum.TryParse<Scenes>(sceneName, out scene))
            {
                ReturnToStory();
                SceneLoad(scene);
            }
            else
            {
                ReturnToStory();
                Debug.LogWarning("Scene \"" + sceneName +  "\" not registered with the SceneController!");
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
            ReturnToStory();
            SceneLoad((Scenes)scene);
            while (SceneIsLoading)
            {
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("Load Scene: No scene with value {" + scene.ToString() + "} ");
        }
    }


    // returns to previous scene
    public void ReturnToStory()
    {
        if (CurActiveScene < Scenes.Story && CurActiveScene > 0)
        {
            SceneManager.UnloadSceneAsync((int)CurActiveScene);
            // GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = true;
            UndoScenePreps();
            Time.timeScale = 1;
            // on menu exit, cause settings reload
            if (CurActiveScene == Scenes.Menus) GameManager.OnPrefsChanged.Invoke();
            CurActiveScene = OldScene;
            if(GameManager.dialogueUI && GameManager.dialogueUI.dialogueContainer) GameManager.dialogueUI.dialogueContainer.SetActive(true);
        }
    }

    /// <summary>
    /// Toggles the Menus Scene
    /// </summary>
    public void ToggleMenu()
    {
        if (CurActiveScene >= Scenes.Story || CurActiveScene == 0)
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
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = true;
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
        
        //dialogue.Stop
        SceneManager.LoadScene(scene);
        //SceneManager.UnloadSceneAsync((int)CurActiveScene);
        CurActiveScene = Scenes.Unknown;
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
            SceneIsLoading = true;
            DoScenePreps();
            //dialogue.Stop
            SceneManager.LoadScene((int)scene);
            //SceneManager.UnloadSceneAsync((int)CurActiveScene);
            CurActiveScene = scene;
            CurMainScene = scene;
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

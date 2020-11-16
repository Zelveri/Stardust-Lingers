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
        Log
    }

    static Scenes CurActiveScene = Scenes.main;

    private void Awake()
    {
        //dialogue = GameObject.Find("Dialogue Runner").GetComponent<DialogueRunner>().Dialogue;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(CurActiveScene == Scenes.main)
            {
                SwitchToScene(Scenes.Menus);
            }
            else
            {
                ReturnToMain();
            }
        }
       // if (Input.GetKeyUp(KeyCode.Escape)) keyDown = false;

        if (Input.GetKeyDown(KeyCode.L) && (CurActiveScene == Scenes.main))
        {
            SwitchToScene(Scenes.Log);
        }
    }

    // returns to previous scene
    public void ReturnToMain()
    {        
        if(CurActiveScene != Scenes.main)
        {
            SceneManager.UnloadSceneAsync((int)CurActiveScene);
            GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = true;
            Time.timeScale = 1;
            //dialogue.Continue();
            CurActiveScene = Scenes.main;
        }
    }

    void DoScenePreps()
    {
        // disable main scene audio listener
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
    }

    public void SwitchToScene(string name)
    {
        // parse string to enum
        Scenes scene = (Scenes)Enum.Parse(typeof(Scenes), name);
        SwitchToScene(scene);
    }

    public void SwitchToScene(Scenes scene)
    {
        if (scene == Scenes.main) { ReturnToMain(); }
        else
        {
            DoScenePreps();
            Time.timeScale = 0; // pause game
            //dialogue.Stop();
            SceneManager.LoadScene((int)scene, LoadSceneMode.Additive);
            CurActiveScene = scene;
        }
    }

    public void SwitchToScene(int scene)
    {
        SwitchToScene((Scenes)scene);
    }

    public void ToggleScene(string name)
    {
        var scene = (Scenes)Enum.Parse(typeof(Scenes), name);
        if (CurActiveScene != scene)
        {
            SwitchToScene(scene);
        }
        else
        {
            ReturnToMain();
        }
    }
}

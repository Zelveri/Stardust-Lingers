using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    enum Scenes
    {
        main=0,
        Menus,
        Log
    }

    static Scenes CurActiveScene = Scenes.main;
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
                DoScenePreps();
                Time.timeScale = 0; // pause game
                SceneManager.LoadScene((int)Scenes.Menus, LoadSceneMode.Additive);
                CurActiveScene = Scenes.Menus;
            }
            else
            {
                ReturnToMain();
            }
        }
       // if (Input.GetKeyUp(KeyCode.Escape)) keyDown = false;

        if (Input.GetKeyDown(KeyCode.L) && (CurActiveScene == Scenes.main))
        {
            SceneManager.LoadScene((int)Scenes.Log, LoadSceneMode.Additive);
            CurActiveScene = Scenes.Log;
        }
    }

    // returns to previous scene
    public void ReturnToMain()
    {
        //switch (CurActiveScene)
        //{
        //    case Scene.Menus:
        //        SceneManager.UnloadSceneAsync("Menus");
        //        break;
        //    case Scene.Log:
        //        SceneManager.UnloadSceneAsync("Log");
        //        break;
        //    default:
        //        break;
        //}
        
        if(CurActiveScene != Scenes.main)
        {
            SceneManager.UnloadSceneAsync((int)CurActiveScene);
            GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = true;
            Time.timeScale = 1;
            CurActiveScene = Scenes.main;
        }
    }

    void DoScenePreps()
    {
        // disable main scene audio listener
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
    }
}

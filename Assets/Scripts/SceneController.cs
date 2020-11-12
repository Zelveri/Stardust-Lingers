using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    enum Scene
    {
        main=0,
        Log,
        Menus
    }

    static Scene CurActiveScene = Scene.main;
    static bool keyDown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(CurActiveScene == Scene.main)
            {
                Time.timeScale = 0; // pause game
                SceneManager.LoadScene("Menus", LoadSceneMode.Additive);
                CurActiveScene = Scene.Menus;
            }
            else
            {
                ReturnToMain();
            }
        }
       // if (Input.GetKeyUp(KeyCode.Escape)) keyDown = false;

        if (Input.GetKeyDown(KeyCode.L) && (CurActiveScene == Scene.main))
        {
            SceneManager.LoadScene("Log", LoadSceneMode.Additive);
            CurActiveScene = Scene.Log;
        }
    }

    // returns to previous scene
    public void ReturnToMain()
    {
        switch (CurActiveScene)
        {
            case Scene.Menus:
                SceneManager.UnloadSceneAsync("Menus");
                break;
            case Scene.Log:
                SceneManager.UnloadSceneAsync("Log");
                break;
            default:
                break;
        }
        if(CurActiveScene != Scene.main)
        {
            Time.timeScale = 1;
            CurActiveScene = Scene.main;
        }
    }
}

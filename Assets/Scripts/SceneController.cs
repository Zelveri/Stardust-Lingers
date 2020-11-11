using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    static bool isMenuActive = false;
    static bool keyDown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !keyDown)
        {
            if(!isMenuActive)
            {
                Time.timeScale = 0; // pause game
                SceneManager.LoadScene("Menus", LoadSceneMode.Additive);
                isMenuActive = true;
            }
            else
            {
                ReturnToMain();
            }
            keyDown = true;
        }
        if (Input.GetKeyUp(KeyCode.Escape)) keyDown = false;
    }

    // returns to previous scene
    public void ReturnToMain()
    {
        if (isMenuActive)
        {
            SceneManager.UnloadSceneAsync("Menus");
            Time.timeScale = 1;
            isMenuActive = false;
        }
    }
}

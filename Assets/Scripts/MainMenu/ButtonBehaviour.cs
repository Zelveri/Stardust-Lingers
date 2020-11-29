using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{

    public void LoadScene(string scene)
    {
        GameManager.sceneController.LoadScene(scene);
    }
    public void StartStory()
    {
        GameManager.sceneController.StartStory();
    }

    public void ToggleMenu()
    {
        GameManager.sceneController.ToggleMenu();
    }

    public void ExitGame()
    {
        GameManager.Quit();
    }

    public void ReturnToMainMenu()
    {
        GameManager.sceneController.ToMainMenu();
    }
}

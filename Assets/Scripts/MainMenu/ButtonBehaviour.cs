using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{

    public void LoadScene(string scene)
    {
        GameManager.sceneController.LoadScene(scene);
    }
    public void StartStory()
    {
        gameObject.GetComponent<Button>().interactable = false;
        GameManager.sceneController.StartStory();
    }

    public void ToggleMenu()
    {
        gameObject.GetComponent<Button>().interactable = false;
        GameManager.sceneController.ToggleMenu();
        gameObject.GetComponent<Button>().interactable = true;
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

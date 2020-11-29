using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitBtnBehaviour : MonoBehaviour
{
    public void BackToMainMenu()
    {
        //GameManager.Quit();
        GameManager.sceneController.ToMainMenu(); // main menu
    }
}

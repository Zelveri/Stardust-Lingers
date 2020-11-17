using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InputHandlerBehaviour : MonoBehaviour
{
    // Handles Keyboard and Mouse Input
    // Integrates with GameManager


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) GameManager.dialogueUI.MarkLineComplete();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.sceneController.ToggleMenu();
        }
        // if (Input.GetKeyUp(KeyCode.Escape)) keyDown = false;

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.sceneController.TryOpenLog();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.sceneController.ScenePhone();
        }
    }
}

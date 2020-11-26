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
            GameManager.sceneController.ToggleLog();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.sceneController.ScenePhone();
        }
#if (DEBUG)
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

        }
#endif
    }
}

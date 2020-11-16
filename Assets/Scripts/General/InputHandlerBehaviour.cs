using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InputHandlerBehaviour : MonoBehaviour
{
    // Handles Keyboard and Mouse Input


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) GameManager.dialogueUI.MarkLineComplete();
    }
}

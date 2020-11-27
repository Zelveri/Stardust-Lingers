using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogButtonBehaviour : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.sceneController.ToggleLog();
    }
}

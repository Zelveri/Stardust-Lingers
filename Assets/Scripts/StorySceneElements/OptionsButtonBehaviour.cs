using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButtonBehaviour : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.sceneController.ToggleMenu();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GameObject textLog;
    // Start is called before the first frame update
    public void OnClick(string function)
    {
        if (function == "OpenLog")
        {
            textLog.SetActive(!textLog.activeInHierarchy);
        }
    }
}
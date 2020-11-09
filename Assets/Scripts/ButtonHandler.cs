using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GameObject textLog;
    public DialogueTracker dialogueTracker;
    TMPro.TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = textLog.gameObject.transform.Find("TextContainer").gameObject.transform.Find("Text").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    public void OnClick(string function)
    {
        if (function == "OpenLog")
        {
            if(!textLog.activeInHierarchy)
            {
                textMesh.text = string.Join("\n", dialogueTracker.GetLines());
            }
            textLog.SetActive(!textLog.activeInHierarchy);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class ButtonHandler : MonoBehaviour
{
    public GameObject textLog;
    public DataController dialogueTracker;
    public InMemoryVariableStorage variableStorage;
    public BackgroundChange backgroundChange;
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
                textMesh.text = string.Join("\n\n", DataController.GetLines());
            }
            textLog.SetActive(!textLog.activeInHierarchy);
        }
    }
}
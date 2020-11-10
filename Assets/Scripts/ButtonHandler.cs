using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class ButtonHandler : MonoBehaviour
{
    public GameObject textLog;
    public DialogueTracker dialogueTracker;
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
                textMesh.text = string.Join("\n\n", dialogueTracker.GetLines());
            }
            textLog.SetActive(!textLog.activeInHierarchy);
        }
        if (function == "Save")
        {
            SaveHandler.SaveGameState(dialogueTracker, variableStorage);
        }
        if (function == "Load")
        {
            SaveState data = SaveHandler.LoadGameState();
            variableStorage.SetValue("$theme_color", data.themeColor);
            dialogueTracker.LoadState(data.lines, data.currentNode, data.prevNode, data.curNameTag, data.backdrop);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class BoxViewChange : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public Canvas dialogueCanvas;
    // Start is called before the first frame update
    void Awake()
    {
        dialogueRunner.AddCommandHandler("hide_dialogue", HideDialogue);
        dialogueRunner.AddCommandHandler("show_dialogue", ShowDialogue);
    }

    public void HideDialogue(string[] parameters)
    {
        dialogueCanvas.gameObject.SetActive(false);
    }

    public void ShowDialogue(string[] parameters)
    {
        dialogueCanvas.gameObject.SetActive(true);
    }
}

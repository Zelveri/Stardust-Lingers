using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class BoxViewChange : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public Canvas dialogueCanvas;
    AnimationEvent animationEvent;
    // Start is called before the first frame update
    void Awake()
    {
        dialogueRunner.AddCommandHandler("hide_dialogue", HideDialogue);
        dialogueRunner.AddCommandHandler("show_dialogue", ShowDialogue);
        animationEvent = dialogueCanvas.GetComponent<AnimationEvent>();
    }

    public void HideDialogue(string[] parameters, System.Action onComplete)
    {
        //dialogueCanvas.gameObject.SetActive(false);
        animationEvent.FadeClear();
        onComplete();
    }

    public void ShowDialogue(string[] parameters, System.Action onComplete)
    {
        //dialogueCanvas.gameObject.SetActive(true);
        animationEvent.FadeOpaque();
        onComplete();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    public BackgroundChangeHandler backgroundHandler;
    public DialogueAnimator dialogueAnimator;
    public TransitionHandler transitionHandler;
    DataController dataController;

    public Canvas dialogueCanvas;

    string curNametag = "";

    bool canContinue = false;
    private void Awake()
    {
        dialogueRunner = GameManager.dialogueRunner;
        dataController = GameManager.dataController;

        // Generic commands
        // other scripts can expand this list, but must implement the handlers on their own
        // command transition <Type>, takes 1 parameter
        dialogueRunner.AddCommandHandler("transition", Transition);
        // command nametag <Name> <status={"","hidden"}>, takes 1 parameter and 1 optional parameter
        dialogueRunner.AddCommandHandler("nametag", NameTag);
        // command backdrop <Filename>, takes 1 parameter
        dialogueRunner.AddCommandHandler("backdrop", Backdrop);
        // command hide/show_dialogue, no parameters
        dialogueRunner.AddCommandHandler("hide_dialogue", HideDialogue);
        dialogueRunner.AddCommandHandler("show_dialogue", ShowDialogue);
    }

    private void OnDestroy()
    {
        // remove all command handlers, so that it doesn't clash with other scenes
        dialogueRunner.RemoveCommandHandler("transition");
        dialogueRunner.RemoveCommandHandler("nametag");
        dialogueRunner.RemoveCommandHandler("backdrop");
        dialogueRunner.RemoveCommandHandler("hide_dialogue");
        dialogueRunner.RemoveCommandHandler("show_dialogue");
    }

    void Backdrop(string[] pars)
    {
        //StartCoroutine(DoChangeFast(parameters[0]));
        transitionHandler.SetNextBackdrop(pars[0]);
    }

    void Transition(string[] pars, System.Action onComplete)
    {
        transitionHandler.Transition(pars, onComplete);
    }

    public void Continue()
    {
        canContinue = true;
    }

    void WaitContinue()
    {
        while (!canContinue) {  };
        canContinue = false;
    }

    void NameTag(string[] pars, System.Action onComplete)
    {
        if (pars[0] != curNametag)
        {
            curNametag = pars[0];
            // nametag change should happen invisible during transition?
            if (TransitionHandler.newNode)
            {
                TransitionHandler.OnDark.AddListener(new UnityAction(() => dialogueAnimator.ChangeNameTag(pars, null)));
                onComplete?.Invoke();
            }
            else
            {
                dialogueAnimator.ChangeNameTag(pars, onComplete);
            }
        }
        else onComplete?.Invoke();
    }

    public void HideDialogue(string[] parameters, System.Action onComplete)
    {
        //dialogueCanvas.gameObject.SetActive(false);
        StartCoroutine(dialogueAnimator.FadeClear(onComplete));
    }

    public void ShowDialogue(string[] parameters, System.Action onComplete)
    {
        //dialogueCanvas.gameObject.SetActive(true);
        StartCoroutine(dialogueAnimator.FadeClear(onComplete));
    }
}

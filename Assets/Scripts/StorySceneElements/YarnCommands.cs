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
    public SpriteCommandBehaviour characterContainer;

    public Canvas dialogueCanvas;

    private void Awake()
    {
        dialogueRunner = GameManager.dialogueRunner;

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
        // will cause commandas to register effects with the next transition
        dialogueRunner.AddCommandHandler("schedule_transition", SchedTransition);

        // command to make sprite animations
        dialogueRunner.AddCommandHandler("sprite", SpriteCommand);
    }

    private void OnDestroy()
    {
        // remove all command handlers, so that it doesn't clash with other scenes
        dialogueRunner.RemoveCommandHandler("transition");
        dialogueRunner.RemoveCommandHandler("nametag");
        dialogueRunner.RemoveCommandHandler("backdrop");
        dialogueRunner.RemoveCommandHandler("hide_dialogue");
        dialogueRunner.RemoveCommandHandler("show_dialogue");
        dialogueRunner.RemoveCommandHandler("sprite");
        dialogueRunner.RemoveCommandHandler("schedule_transition");
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

    void SchedTransition(string [] pars)
    {
        TransitionHandler.newNode = true;
    }

    void NameTag(string[] pars, System.Action onComplete)
    {
        dialogueAnimator.ChangeNameTag(pars, onComplete);
    }

    public void HideDialogue(string[] parameters, System.Action onComplete)
    {
        if (!dialogueAnimator)
        {
            Debug.LogError("Command hide_dialogue: no dialogue to hide!");
            return;
        }
        // if transition is scheduled, just tell transition handler to not show dialogue afterwards
        if (TransitionHandler.newNode)
        {
            TransitionHandler.overrideDialogueFadeIn = true;
            onComplete();
        }
        else StartCoroutine(dialogueAnimator.FadeClear(onComplete));
    }

    public void ShowDialogue(string[] parameters, System.Action onComplete)
    {
        if (!dialogueAnimator)
        {
            Debug.LogError("Command show_dialogue: no dialogue to show!");
            return;
        }
        //dialogueCanvas.gameObject.SetActive(true);
        StartCoroutine(dialogueAnimator.FadeOpaque(onComplete));
    }

    void SpriteCommand(string[] pars, System.Action onComplete)
    {
        if (characterContainer)
            characterContainer.SpriteCommand(pars, onComplete);
        else
            Debug.LogError("Command sprite: no character container");
    }
}

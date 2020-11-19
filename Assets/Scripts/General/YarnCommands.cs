using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    public BackgroundChangeHandler backgroundHandler;
    public DialogueAnimator dialogueAnimator;
    public TransitionHandler transitionHandler;
    DataController dataController;

    public Animator slideAnimator;
    public Animator crossfadeAnimator;

    public Canvas dialogueCanvas;


    enum TransitionType
    {
        None=0,
        Fade,
        Slide,
        Cross_Fade,
        Circleslide
    }
    TransitionType transitionType = TransitionType.Fade;
    bool canContinue = false;
    private void Awake()
    {
        dialogueRunner = GameManager.dialogueRunner;
        dataController = GameManager.dataController;
        // All the commands
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
        dialogueRunner.RemoveCommandHandler("transition");
        dialogueRunner.RemoveCommandHandler("nametag");
        dialogueRunner.RemoveCommandHandler("backdrop");
        dialogueRunner.RemoveCommandHandler("hide_dialogue");
        dialogueRunner.RemoveCommandHandler("show_dialogue");
    }

    void Backdrop(string[] pars, System.Action onComplete)
    {
        //StartCoroutine(DoChangeFast(parameters[0]));
        switch (transitionType)
        {
            case TransitionType.None:
                StartCoroutine(backgroundHandler.DoChangeFast(pars[0], onComplete));
                break;
            case TransitionType.Fade:
                StartCoroutine(backgroundHandler.DoChange(pars[0], onComplete));
                break;
            case TransitionType.Slide:
                StartCoroutine(SlideTransition(pars[0], onComplete));
                break;
            default:
                break;
        }
        transitionType = TransitionType.Fade;
    }

    void Transition(string[] pars, System.Action onComplete)
    {
        // Fade_in and Fade_out are independent of background, therefore treated differently
        switch (pars[0])
        {
            case "Fade_In":
                StartCoroutine(transitionHandler.FadeIn(crossfadeAnimator, onComplete));
                break;
            case "Fade_Out":
                StartCoroutine(transitionHandler.FadeOut(crossfadeAnimator, onComplete));
                break;
            case "Slide":
            default:
                try
                {
                    transitionType = (TransitionType)Enum.Parse(typeof(TransitionType), pars[0]);
                    onComplete();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Transition type not found: " + pars[0] + "!\n" + ex.ToString());
                }
                break;
        }
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
        dialogueAnimator.ChangeNameTag(pars, onComplete);
    }

    IEnumerator  SlideTransition(string param, Action onComplete)
    {
        yield return StartCoroutine(dialogueAnimator.FadeClear(null));
        dialogueAnimator.ClearText();
        yield return StartCoroutine(transitionHandler.Slide(slideAnimator, param, null));
        yield return StartCoroutine(dialogueAnimator.FadeOpaque(onComplete));
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

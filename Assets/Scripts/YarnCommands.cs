using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public BackgroundChangeHandler backgroundHandler;
    public NameTagChangeHandler nameTagHandler;
    public TransitionHandler transitionHandler;
    public DataController dataController;

    public Animator slideAnimator;
    public Animator crossfadeAnimator;

    public Canvas dialogueCanvas;

    AnimationEvent animationEvent;

    enum TransitionType
    {
        None=0,
        Fade,
        Slide,
        Crossfade,
        Circleslide
    }
    TransitionType transitionType = TransitionType.Fade;
    private void Awake()
    {
        animationEvent = dialogueCanvas.GetComponent<AnimationEvent>();
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
                StartCoroutine(transitionHandler.Slide(slideAnimator, pars[0], onComplete));
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

    void NameTag(string[] pars, System.Action onComplete)
    {
        nameTagHandler.ChangeNameTag(pars, onComplete);
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

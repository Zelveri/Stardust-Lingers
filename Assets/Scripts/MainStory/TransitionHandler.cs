using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class TransitionHandler : MonoBehaviour
{
    public Animator slideAnimator;
    public Animator crossfadeAnimator;
    public DialogueAnimator dialogueAnimator;
    public float transitionSpeed = 1f;
    public BackgroundChangeHandler backgroundHandler;
    public static UnityEvent OnDark;

    string nextBackdrop = "";
    enum TransitionType
    {
        None = 0,
        Fade,
        Slide,
        Cross_Fade,
        Circleslide
    }
    TransitionType transitionType = TransitionType.Fade;

    void Awake()
    {
        //backgroundHandler = FindObjectOfType<BackgroundChangeHandler>();
        OnDark = new UnityEvent();
    }

    public void SetNextBackdrop(string backdrop)
    {
        nextBackdrop = backdrop;
    }

    public void Transition(string[] pars, System.Action onComplete)
    {
        // Fade_in and Fade_out are independent of background, therefore treated differently
        switch (pars[0])
        {
            case "Fade_In":
                StartCoroutine(FadeIn(crossfadeAnimator, onComplete));
                break;
            case "Fade_Out":
                StartCoroutine(FadeOut(crossfadeAnimator, onComplete));
                break;
            case "Slide":
            default:
                try
                {
                    transitionType = (TransitionType)Enum.Parse(typeof(TransitionType), pars[0]);
                    switch (transitionType)
                    {
                        case TransitionType.None:
                            StartCoroutine(backgroundHandler.DoChangeFast(nextBackdrop, onComplete));
                            break;
                        case TransitionType.Fade:
                            StartCoroutine(backgroundHandler.DoChange(nextBackdrop, onComplete));
                            break;
                        case TransitionType.Slide:
                            OnDark.AddListener(new UnityAction(() => backgroundHandler.ChangeBackdropFast(nextBackdrop)));
                            StartCoroutine(Transition(slideAnimator, nextBackdrop, onComplete));
                            break;
                        case TransitionType.Cross_Fade:
                            OnDark.AddListener(new UnityAction(() => backgroundHandler.ChangeBackdropFast(nextBackdrop)));
                            StartCoroutine(Transition(crossfadeAnimator, nextBackdrop, onComplete));
                            break;
                        default:
                            break;
                    }
                    transitionType = TransitionType.Fade;
                    onComplete();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Transition type not found: " + pars[0] + "!\n" + ex.ToString());
                }
                break;
        }
    }

    IEnumerator Transition(Animator animator, string param, System.Action onComplete)
    {
        yield return StartCoroutine(dialogueAnimator.FadeClear(null));
        dialogueAnimator.ClearText();
        // yield return StartCoroutine(transitionHandler.DoAnimation(animator, param, null)); 
        yield return StartCoroutine(FadeOut(animator, null));
        OnDark.Invoke();
        yield return StartCoroutine(FadeIn(animator, null));
        yield return StartCoroutine(dialogueAnimator.FadeOpaque(onComplete));
        OnDark.RemoveAllListeners();
    }

    public IEnumerator FadeOut(Animator animator, System.Action onComplete)
    {
        var animstate = animator.gameObject.GetComponent<AnimationState>();
        animator.SetFloat("Duration", transitionSpeed);
        animator.SetTrigger("Fade_Black");
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }

    public IEnumerator FadeIn(Animator animator, System.Action onComplete)
    {
        var animstate = animator.gameObject.GetComponent<AnimationState>();
        animator.SetTrigger("Fade_Clear");
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }

    public IEnumerator DoAnimation(Animator animator, string backgrnd, System.Action onComplete)
    {
        var animstate = animator.gameObject.GetComponent<AnimationState>();
        animator.SetTrigger("Fade_Black");
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        backgroundHandler.ChangeBackdropFast(backgrnd, null);
        animator.SetTrigger("Fade_Clear");
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }

}

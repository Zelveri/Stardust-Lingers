using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Yarn.Unity;

public class TransitionHandler : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public float transitionSpeed = 1f;
    public BackgroundChangeHandler backgroundHandler;

    void Awake()
    {
        //backgroundHandler = FindObjectOfType<BackgroundChangeHandler>();
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

    public IEnumerator Slide(Animator animator, string backgrnd, System.Action onComplete)
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

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class handles the &lt;&lt; transition &gt;&gt; command
/// </summary>
public class TransitionHandler : MonoBehaviour
{
    // the animators that execute the actual animation
    public Animator slideAnimator;
    public Animator crossfadeAnimator;
    public DialogueAnimator dialogueAnimator;
    public float transitionSpeed = 1f;
    // class that handles the background image changes
    public BackgroundChangeHandler backgroundHandler;
    // event, that gets triggered when the screen is dark during a Crossfade or Slide transition
    // useful for doing multiple things then, background change, remove sprites etc.
    // can be subscribed to with TransitionHandler.OnDark.AddListener(method to call)
    public static UnityEvent OnDark;
    // flag that gets set on start of a node, is unset after doing transition
    // to help other commands determine if they have to register an event with this or not
    public static bool newNode = false;

    string nextBackdrop = "";

    void Awake()
    {
        OnDark = new UnityEvent();
    }

    /// <summary>
    /// Set the backdrop the next transition should change to
    /// </summary>
    /// <param name="backdrop">filename of the new backdrop</param>
    public void SetNextBackdrop(string backdrop)
    {
        nextBackdrop = backdrop;
        // register backdrop change with lambda, enables passing fixed arguments with delegate functions
        OnDark.AddListener(new UnityAction(() => backgroundHandler.ChangeBackdropFast(nextBackdrop)));
    }

    /// <summary>
    ///  Invoke all registered listeners with OnDark, then unregisters them
    /// </summary>
    void DoOnDark()
    {
        newNode = false;
        OnDark.Invoke();
        OnDark.RemoveAllListeners();
    }

    /// <summary>
    /// Evaluate the parameters of the called transition command
    /// </summary>
    /// <param name="pars">command parameters</param>
    /// <param name="onComplete">blocking Action</param>
    public void Transition(string[] pars, System.Action onComplete)
    {
        if(pars == null || pars.Length == 0)
        {
            Debug.LogError("transition: no parameters given!");
        }
        switch (pars[0])
        {
            // direct background fade without black screen
            case "None":
                StartCoroutine(backgroundHandler.DoChangeFast(nextBackdrop, onComplete));
                break;
            case "Fade":
                StartCoroutine(backgroundHandler.DoChange(nextBackdrop, onComplete));
                break;
            // fade with black screen
            case "Fade_In":
                StartCoroutine(FadeIn(crossfadeAnimator, onComplete));
                break;
            case "Fade_Out":
                StartCoroutine(FadeOut(crossfadeAnimator, onComplete));
                break;
            case "Slide":
                StartCoroutine(Transition(slideAnimator, onComplete));
                break;
            case "Cross_Fade":
                StartCoroutine(Transition(crossfadeAnimator, onComplete));
                break;

            default:
                    Debug.LogError("Transition type not found: " + pars[0]);
                break;
        }
    }

    /// <summary>
    /// Execute the actual transition
    /// </summary>
    /// <param name="animator">Animator or override controller. Needs to support "Fade_Clear" and "Fade_Black" trigger</param>
    /// <param name="onComplete">blocking Action</param>
    /// <returns></returns>
    IEnumerator Transition(Animator animator, System.Action onComplete)
    {
        // yield return statement makes code wait till Coroutine is finished
        // hide dialogue
        yield return StartCoroutine(dialogueAnimator.FadeClear(null));
        // clear text so it wont show when dlg is faded back in
        dialogueAnimator.ClearText();
        // start screen transition to black with given animator
        yield return StartCoroutine(FadeOut(animator, null));
        // do the hidden changes
        DoOnDark();
        // fade back in
        yield return StartCoroutine(FadeIn(animator, null));
        // show dialogue again
        yield return StartCoroutine(dialogueAnimator.FadeOpaque(onComplete));
        
    }

    /// <summary>
    /// Start fade out animation on given animator
    /// </summary>
    /// <param name="animator">Animator or override controller. Needs to support "Fade_Clear" and "Fade_Black" trigger</param>
    /// <param name="onComplete">blocking action</param>
    /// <returns></returns>
    public IEnumerator FadeOut(Animator animator, System.Action onComplete)
    {
        // animationstate component receives signal when animation is finished
        var animstate = animator.gameObject.GetComponent<AnimationState>();
        // set animation speed multiplier, Duration is not accurate name
        animator.SetFloat("Duration", transitionSpeed);
        animator.SetTrigger("Fade_Black");
        animstate.isRunning = true;
        // wait for finished signal
        while (animstate.isRunning)
        {
            yield return null;
        }
        // if action is not null, invoke it
        onComplete?.Invoke();
    }

    /// <summary>
    /// Start fade in animation on given animator
    /// </summary>
    /// <param name="animator">Animator or override controller. Needs to support "Fade_Clear" and "Fade_Black" trigger</param>
    /// <param name="onComplete">blocking action</param>
    /// <returns></returns>
    public IEnumerator FadeIn(Animator animator, System.Action onComplete)
    {
        var animstate = animator.gameObject.GetComponent<AnimationState>();
        animator.SetTrigger("Fade_Clear");
        DoOnDark();
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }

    // obsolete, simply the other two function bound together with background change
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

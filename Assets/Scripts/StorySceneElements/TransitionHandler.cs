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

    // flag to signal, that the game was just loaded, so the next animation should always be a fade in
    // is set in DataController.DoStateLoad
    public static bool overrideTransitionFade = false;

    string nextBackdrop = "";

    void Awake()
    {
        OnDark = new UnityEvent();
        GameManager.RegisterTransitionHandler(this);
    }

    private void OnDestroy()
    {
        GameManager.UnregisterTransitionHandler();
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
    ///  If no transition is scheduled, cancel the actions
    /// </summary>
    void CancelOnDark()
    {
        newNode = false;
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
        // if override given, always do Fade_In transition
        if (overrideTransitionFade)
        {
            pars[0] = "Fade_In";
            // disable override
            overrideTransitionFade = false;
        }
        switch (pars[0])
        {
            // direct background fade without black screen
            case "None":
                CancelOnDark();
                StartCoroutine(backgroundHandler.DoChangeFast(nextBackdrop, onComplete));
                break;
            case "Fade":
                StartCoroutine(backgroundHandler.DoChange(nextBackdrop, onComplete));
                break;
            // fade with black screen
            case "Fade_In":
                StartCoroutine(Fade("in", crossfadeAnimator, onComplete));
                break;
            case "Fade_Out":
                StartCoroutine(Fade("out", crossfadeAnimator, onComplete));
                break;
            case "Cross_Fade":
                StartCoroutine(Transition(crossfadeAnimator, onComplete));
                break;
            case "Slide":
                StartCoroutine(Transition(slideAnimator, onComplete));
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
        yield return StartCoroutine(FadeOut(animator));
        // do the hidden changes
        DoOnDark();
        // fade back in
        yield return StartCoroutine(FadeIn(animator));
        // show dialogue again
        yield return StartCoroutine(dialogueAnimator.FadeOpaque(null));
        onComplete?.Invoke();
    }

    /// <summary>
    /// Do the fade
    /// </summary>
    /// <param name="direction">in or out</param>
    /// <param name="animator">Animator or override controller. Needs to support "Fade_Clear" and "Fade_Black" trigger</param>
    /// <param name="onComplete">blocking Action</param>
    /// <returns></returns>
    IEnumerator Fade(string direction, Animator animator, System.Action onComplete)
    {
        if (direction == "out")
        {// hide dialogue and then fade to black
            if(dialogueAnimator) yield return StartCoroutine(dialogueAnimator.FadeClear(null));
            yield return StartCoroutine(FadeOut(animator));
        }
        // always do the queued statements
        DoOnDark();
        if (direction == "in")
        {// fade in and then fade dialogue in
            // only do dialogueAnimator fcns if it exists, phone scene does not have a dialogueAnimator!!!
            if (dialogueAnimator)
            {
                dialogueAnimator.HideDialogue();
                dialogueAnimator.ClearText();
            }
            yield return StartCoroutine(FadeIn(animator));
            if (dialogueAnimator) yield return StartCoroutine(dialogueAnimator.FadeOpaque(null));
        }
        onComplete?.Invoke();
    }

    /// <summary>
    /// Start fade out animation on given animator
    /// </summary>
    /// <param name="animator">Animator or override controller. Needs to support "Fade_Clear" and "Fade_Black" trigger</param>
    /// <returns></returns>
    IEnumerator FadeOut(Animator animator)
    {
        // animationstate component receives signal when animation is finished
        var animstate = animator.gameObject.GetComponent<AnimationState>();
        // set animation speed multiplier, Duration is not accurate name
        animator.SetFloat("Duration", transitionSpeed);
        animator.SetTrigger("Fade_Black");
        animstate.IsRunning = true;
        // wait for finished signal
        while (animstate.IsRunning)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Start fade in animation on given animator
    /// </summary>
    /// <param name="animator">Animator or override controller. Needs to support "Fade_Clear" and "Fade_Black" trigger</param>
    /// <returns></returns>
    IEnumerator FadeIn(Animator animator)
    {
        var animstate = animator.gameObject.GetComponent<AnimationState>();
        animator.SetFloat("Duration", transitionSpeed);
        animator.SetTrigger("Fade_Clear");
        animstate.IsRunning = true;
        while (animstate.IsRunning)
        {
            yield return null;
        }
    }

    /// <summary>
    /// to use when using transitions apart from the story
    /// </summary>
    public IEnumerator SceneFadeOut()
    {
        // animationstate component receives signal when animation is finished
        var animstate = crossfadeAnimator.gameObject.GetComponent<AnimationState>();
        // set animation speed multiplier, Duration is not accurate name
        crossfadeAnimator.SetFloat("Duration", 2f);
        crossfadeAnimator.SetTrigger("Fade_Black");
        animstate.IsRunning = true;
        // wait for finished signal
        while (animstate.IsRunning)
        {
            yield return null;
        }
    }

    /// <summary>
    /// to use when using transitions apart from the story
    /// </summary>
    public IEnumerator SceneFadeIn()
    {
        // animationstate component receives signal when animation is finished
        var animstate = crossfadeAnimator.gameObject.GetComponent<AnimationState>();
        // set animation speed multiplier, Duration is not accurate name
        crossfadeAnimator.SetFloat("Duration", 2f);
        crossfadeAnimator.SetTrigger("Fade_Clear");
        animstate.IsRunning = true;
        // wait for finished signal
        while (animstate.IsRunning)
        {
            yield return null;
        }
    }
}

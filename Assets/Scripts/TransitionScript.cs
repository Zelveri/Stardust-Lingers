using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Yarn.Unity;

public class TransitionScript : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    CanvasGroup myCanvas;
    Animator transition;
    public float transitionSpeed = 1f;
    private string newBackground = "";
    BackgroundChange backgroundHandler;
    // Start is called before the first frame update
    void Awake()
    {
        dialogueRunner.AddCommandHandler("transition", ScreenTransition);
        backgroundHandler = FindObjectOfType<BackgroundChange>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    StartCoroutine(Slide("Cabin_Day"));
        //}
    }

    public void ScreenTransition(string[] parameters, System.Action onComplete)
    {
        string transitionType = parameters[0];
        switch (transitionType)
        {
            case "Fade_In":
                StartCoroutine(FadeIn(myCanvas, transitionSpeed));
                break;
            case "Fade_Out":
                myCanvas.gameObject.SetActive(true);
                StartCoroutine(FadeOut(myCanvas, transitionSpeed));
                break;
            case "Slide":
                myCanvas = gameObject.transform.Find("ScreenWipeRect").gameObject.GetComponent<CanvasGroup>();
                transition = gameObject.transform.Find("ScreenWipeRect").gameObject.GetComponent<Animator>();
                newBackground = parameters[1];
                transition.SetFloat("Duration", transitionSpeed);
                StartCoroutine(Slide(newBackground, onComplete));
                break;
            case "SlideCirc":
                myCanvas = gameObject.transform.Find("ScreenWipe").gameObject.GetComponent<CanvasGroup>();
                transition = gameObject.transform.Find("ScreenWipe").gameObject.GetComponent<Animator>();
                newBackground = parameters[1];
                transition.SetFloat("Duration", transitionSpeed);
                StartCoroutine(Slide(newBackground, onComplete));
                break;
        }

    }

    IEnumerator FadeOut(CanvasGroup canvas, float duration)
    {
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        // set the canvas to the start alpha – this ensures that the canvas is ‘reset’ if you fade it multiple times
        // myCanvas.alpha = startAlpha;
        var startAlpha = canvas.alpha;
        var endAlpha = 1f;
        // loop repeatedly until the previously calculated end time
        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            var a = startAlpha + percentage;
            canvas.alpha = (a < endAlpha) ? a : endAlpha; // calculate the new alpha

            yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
        }
        canvas.alpha = endAlpha; // force the alpha to the end alpha before finishing – this is here to mitigate any rounding errors, e.g. leaving the alpha at 0.01 instead of 0
        yield return null;
    }

    IEnumerator FadeIn(CanvasGroup canvas, float duration)
    {
        // keep track of when the fading started, when it should finish, and how long it has been running&lt;/p&gt; &lt;p&gt;&a
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;

        // set the canvas to the start alpha – this ensures that the canvas is ‘reset’ if you fade it multiple times
        // myCanvas.alpha = startAlpha;
        var startAlpha = canvas.alpha;
        var endAlpha = 0f;
        // loop repeatedly until the previously calculated end time
        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            var a = startAlpha - percentage;
            canvas.alpha = (a > endAlpha) ? a : endAlpha; // calculate the new alpha

            yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
        }
        canvas.alpha = endAlpha; // force the alpha to the end alpha before finishing – this is here to mitigate any rounding errors, e.g. leaving the alpha at 0.01 instead of 0
        canvas.gameObject.SetActive(false);
        yield return null;
    }

    IEnumerator Slide(string backgrnd, System.Action onComplete)
    {
        var animstate = transition.gameObject.GetComponent<AnimationState>();
        transition.SetTrigger("Fade_Black");
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        StartCoroutine(backgroundHandler.DoChangeFast(backgrnd));
        transition.SetTrigger("Fade_Clear");
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        onComplete();
    }

}

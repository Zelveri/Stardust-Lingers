using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Yarn.Unity;

public class TransitionScript : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public CanvasGroup myCanvas;
    public Animator transition;
    public float transitionTime = 1f;
    private string newBackground = "";

    bool isFading = false;
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
                StartCoroutine(FadeIn(myCanvas, transitionTime));
                break;
            case "Fade_Out":
                myCanvas.gameObject.SetActive(true);
                StartCoroutine(FadeOut(myCanvas, transitionTime));
                break;
            case "Slide":
                newBackground = parameters[1];
                transition.SetFloat("Duration", transitionTime);
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
        isFading = true;
        transition.SetTrigger("Fade_Black");
        while (isFading)
        {
            yield return null;
        }
        StartCoroutine(backgroundHandler.DoChange(backgrnd));
        isFading = true;
        transition.SetTrigger("Fade_Clear");
        while (isFading)
        {
            yield return null;
        }
        onComplete();
    }

    void AnimationComplete()
    {
        isFading = false;
    }


}

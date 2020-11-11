using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class Fader : MonoBehaviour
{
    public CanvasGroup myCanvas;
    public DialogueRunner dialogueRunner;
    public float fadeDuration = 0.8f;


    private void Awake()
    {
        //dialogueRunner.AddCommandHandler("transition", ScreenTransition);
    }

    private void Update()
    {
        
    }

    public void ScreenTransition(string[] parameters)
    {
        string transitionType = parameters[0];
        switch(transitionType)
        {
            case "Fade_In":
                StartCoroutine(FadeCanvasClear(myCanvas, fadeDuration));
                break;
            case "Fade_Out":
                myCanvas.gameObject.SetActive(true);
                StartCoroutine(FadeCanvasBlack(myCanvas, fadeDuration));
                break;
            case "Slide":
                break;
        }
        
    }

    public static IEnumerator FadeCanvasBlack(CanvasGroup canvas, float duration)
    {
        // keep track of when the fading started, when it should finish, and how long it has been running&lt;/p&gt; &lt;p&gt;&a
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

    public static IEnumerator FadeCanvasClear(CanvasGroup canvas, float duration)
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
}

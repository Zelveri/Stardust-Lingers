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
                //StartCoroutine(FadeIn(myCanvas, transitionSpeed));
                myCanvas = gameObject.transform.Find("Crossfade").gameObject.GetComponent<CanvasGroup>();
                transition = gameObject.transform.Find("Crossfade").gameObject.GetComponent<Animator>();
                transition.SetFloat("Duration", transitionSpeed);
                StartCoroutine(FadeIn(onComplete));
                break;
            case "Fade_Out":
                myCanvas = gameObject.transform.Find("Crossfade").gameObject.GetComponent<CanvasGroup>();
                transition = gameObject.transform.Find("Crossfade").gameObject.GetComponent<Animator>();
                transition.SetFloat("Duration", transitionSpeed);
                StartCoroutine(FadeOut(onComplete));
                break;
            case "Slide":
                if (parameters.Length > 1)
                {
                    myCanvas = gameObject.transform.Find("ScreenWipeRect").gameObject.GetComponent<CanvasGroup>();
                    transition = gameObject.transform.Find("ScreenWipeRect").gameObject.GetComponent<Animator>();
                    newBackground = parameters[1];
                    transition.SetFloat("Duration", transitionSpeed);
                    StartCoroutine(Slide(newBackground, onComplete));
                }
                else onComplete();
                break;
            case "SlideCirc":
                if (parameters.Length > 1)
                {
                    myCanvas = gameObject.transform.Find("ScreenWipe").gameObject.GetComponent<CanvasGroup>();
                    transition = gameObject.transform.Find("ScreenWipe").gameObject.GetComponent<Animator>();
                    newBackground = parameters[1];
                    transition.SetFloat("Duration", transitionSpeed);
                    StartCoroutine(Slide(newBackground, onComplete));
                } else onComplete();
                break;
        }
    }

    IEnumerator FadeOut(System.Action onComplete)
    {
        var animstate = transition.gameObject.GetComponent<AnimationState>();
        transition.SetTrigger("Fade_Black");
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        onComplete();
    }

    IEnumerator FadeIn(System.Action onComplete)
    {
        var animstate = transition.gameObject.GetComponent<AnimationState>();
        transition.SetTrigger("Fade_Clear");
        animstate.isRunning = true;
        while (animstate.isRunning)
        {
            yield return null;
        }
        onComplete();
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

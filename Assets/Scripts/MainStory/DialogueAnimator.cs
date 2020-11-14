using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class DialogueAnimator : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public DataController dialogueController;
    public CanvasGroup dialogueCanvas;
    public Animator fadeAnimator;
    public TMPro.TextMeshProUGUI nameTag;
    public TMPro.TextMeshProUGUI storyText;
    public Image textBackground;
    public float speed = 10f;

    Dictionary<string, string> nameToTextureDict;


    bool _isfading = false;
    bool _ishidden = false;

    public bool isHidden
    {
        get { return _ishidden; }
        set { _ishidden = value; }
    }

    public bool isRunning
    {
        get { return _isfading; }
    }

    void Awake()
    {
        nameToTextureDict = new Dictionary<string, string>();
        nameToTextureDict.Add("Mira", "Box_Red");
        nameToTextureDict.Add("Melly", "Box_Blue");
    }

    public void ChangeNameTag(string[] pars, System.Action onComplete)
    {
        var name = pars[0];
        string boxName = PlayerPrefs.GetString("theme_color") + "_" + nameToTextureDict[name];
        var doEffect = !isHidden;
        // if additional hidden argument is given conceal name
        if ((pars.Length > 1) && (pars[1] == "hidden"))
        {
            name = "???";
        }
        // add name to log
        dialogueController.UpdateNametag(name);
        StartCoroutine(DoChange(name, boxName, doEffect, onComplete));
    }

    IEnumerator DoChange(string newName, string boxName, bool doEffect, System.Action onComplete)
    {
        if (doEffect)
        {
            yield return StartCoroutine(FadeClear(null));
            //while (animationEvent.isRunning)
            //{
            //    yield return null;
            //}
        }
        nameTag.text = newName;
        textBackground.sprite = Resources.Load<Sprite>("Artwork/UI/Text Box/" + boxName);
        if (doEffect)
        {
            yield return StartCoroutine(FadeOpaque(null));
            //while (animationEvent.isRunning)
            //{
            //    yield return null;
            //}
        }
        onComplete();
    }


    public IEnumerator FadeClear(System.Action onComplete)
    {
        var currentState = fadeAnimator.GetCurrentAnimatorStateInfo(0);
        fadeAnimator.SetFloat("Speed", speed);
        if (!currentState.IsName("Dialogue_Fade_Clear"))
        {
            _isfading = true;
            _ishidden = true;
            fadeAnimator.SetTrigger("Fade_Clear");
            while (_isfading)
            {
                yield return null;
            }
            onComplete?.Invoke();
        }
    }

    public IEnumerator FadeOpaque(System.Action onComplete)
    {
        var currentState = fadeAnimator.GetCurrentAnimatorStateInfo(0);
        fadeAnimator.SetFloat("Speed", speed);
        if (!currentState.IsName("Dialogue_Fade_Opaque"))
        {
            _isfading = true;
            _ishidden = false;
            fadeAnimator.SetTrigger("Fade_Opaque");
            while (_isfading)
            {
                yield return null;
            }
            onComplete?.Invoke();
        }
    }

    public void AnimationStart()
    {
        _isfading = true;
    }

    public void AnimationEnd()
    {
        _isfading = false;
    }
}

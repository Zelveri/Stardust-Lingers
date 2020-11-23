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
        nameToTextureDict.Add("Lune", "Box_Purple");
        nameToTextureDict.Add("Trevis", "Box_Blue");
        GameManager.OnPrefsChanged.AddListener(ReloadSettings);
    }

    public void ChangeNameTag(string[] pars, System.Action onComplete)
    {
        var name = pars[0];
        string theme_color = PlayerPrefs.GetString("theme_color");
        string boxName = theme_color + "_" + nameToTextureDict[name];
        var doEffect = !isHidden;
        // if additional hidden argument is given conceal name
        if ((pars.Length > 1))
        {
            if (pars[1] == "hidden") name = "???";
            else name = pars[1];
        }
        // add name to log
        GameManager.dataController.UpdateNametag(pars[0], name);
        StartCoroutine(DoChange(name, boxName, theme_color, doEffect, onComplete));
    }

    IEnumerator DoChange(string newName, string boxName, string theme_color, bool doEffect, System.Action onComplete)
    {
        Color col;
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
        // change text color
        if(theme_color == "Light")
        {
            col = new Color(0, 0, 0);
        }
        else
        {
            col = new Color(1, 1, 1);
        }
        storyText.color = col;
        if (doEffect)
        {
            yield return StartCoroutine(FadeOpaque(null));
            //while (animationEvent.isRunning)
            //{
            //    yield return null;
            //}
        }
        onComplete?.Invoke();
    }

    public void ReloadSettings()
    {
        string theme_color = PlayerPrefs.GetString("theme_color");
        string boxName = theme_color + "_" + nameToTextureDict[GameManager.dataController.CurNametag];
        StartCoroutine(DoChange(GameManager.dataController.CurNametag, boxName, theme_color, false, null));
    }

    public void ClearText()
    {
        storyText.text = "";
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

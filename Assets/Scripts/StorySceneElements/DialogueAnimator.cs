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
    bool _ishidden = true;

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
        // only execute if nametag actually needs changing
        if (pars[0] != DataController.GetNametag())
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
            // nametag change should happen invisible during transition
            if (TransitionHandler.newNode)
            {
                // queue nametag change without effect
                TransitionHandler.OnDark.AddListener(
                delegate()
                {
                    GameManager.dataController.UpdateNametag(pars[0], name);
                    StartCoroutine(DoChange(name, boxName, theme_color, false, null));
                });
                onComplete?.Invoke();
            }
            else
            {
                // do nametag change now 
                GameManager.dataController.UpdateNametag(pars[0], name);
                StartCoroutine(DoChange(name, boxName, theme_color, doEffect, onComplete));
            }
        }
        else onComplete?.Invoke();
    }

    /// <summary>
    /// do the nametag change with box color
    /// </summary>
    /// <param name="newName">new nametag</param>
    /// <param name="boxName">new box sprite name</param>
    /// <param name="theme_color">theme color light or dark</param>
    /// <param name="doEffect">do fade animation?</param>
    /// <param name="onComplete">blocking action</param>
    /// <returns></returns>
    IEnumerator DoChange(string newName, string boxName, string theme_color, bool doEffect, System.Action onComplete)
    {
        Color col;
        // if fade effect should be done execute and wait for effect
        if (doEffect)
        {
            yield return StartCoroutine(FadeClear(null));
        }
        // assign new nametag to text field
        nameTag.text = newName;
        // load new sprite
        textBackground.sprite = Resources.Load<Sprite>("Artwork/UI/Text Box/" + boxName);
        // change text theme color
        if(theme_color == "Light") col = new Color(0, 0, 0);
        else col = new Color(1, 1, 1);
        storyText.color = col;

        if (doEffect)
        {
            yield return StartCoroutine(FadeOpaque(null));
        }
        // command done
        onComplete?.Invoke();
    }

    // reload settings is called from GameManager OnPrefsChanged event
    // reloads theme color if changed in menu
    public void ReloadSettings()
    {
        string theme_color = PlayerPrefs.GetString("theme_color");
        string boxName = theme_color + "_" + nameToTextureDict[GameManager.dataController.CurNametag];
        StartCoroutine(DoChange(GameManager.dataController.CurNametag, boxName, theme_color, false, null));
    }

    /// <summary>
    /// clears the story text
    /// </summary>
    public void ClearText()
    {
        storyText.text = "";
    }

    /// <summary>
    /// hides the dialogue
    /// </summary>
    public void HideDialogue()
    {
        StartCoroutine(FadeClear(null,true));
    }

    public IEnumerator FadeClear(System.Action onComplete, bool instant=false)
    {
        var currentState = fadeAnimator.GetCurrentAnimatorStateInfo(0);
        fadeAnimator.SetFloat("Speed", instant ? 10000f : speed);
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

    public IEnumerator FadeOpaque(System.Action onComplete, bool instant=false)
    {
        var currentState = fadeAnimator.GetCurrentAnimatorStateInfo(0);
        fadeAnimator.SetFloat("Speed", instant ? 10000f : speed);
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

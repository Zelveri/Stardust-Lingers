using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class NameTagChangeHandler : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public DataController dialogueTracker;
    public TMPro.TextMeshProUGUI nameTag;
    public Image textBackground;
    public Animator transitionAnimator;
    public float speed = 10f;
    Dictionary<string, string> nameToTextureDict;
    AnimationEvent animationEvent;
    // Start is called before the first frame update
    void Awake()
    {
        //animator = canvas.gameObject.GetComponent<Animator>();
        animationEvent = transitionAnimator.gameObject.GetComponent<AnimationEvent>();
        //nameTag = canvas.gameObject.transform.Find("NameTag").gameObject.GetComponent<TextMeshProUGUI>();
        //spriteRenderer = canvas.gameObject.transform.Find("Box").gameObject.transform.Find("Image").gameObject.GetComponent<SpriteRenderer>();
        dialogueTracker = GameObject.Find("DataController").GetComponent<DataController>();
        transitionAnimator.SetFloat("Speed", speed);
        nameToTextureDict = new Dictionary<string, string>();
        nameToTextureDict.Add("Mira", "Box_Red");
        nameToTextureDict.Add("Melly", "Box_Blue");
    }

    public void ChangeNameTag(string[] pars, System.Action onComplete)
    {
        var name = pars[0];
        string boxName = PlayerPrefs.GetString("theme_color") + "_" + nameToTextureDict[name];
        var doEffect = !animationEvent.isHidden;
        // if additional hidden argument is given conceal name
        if ((pars.Length > 1) && (pars[1] == "hidden"))
        {
            name = "???";
        }
        // add name to log
        dialogueTracker.UpdateNametag(name);
        StartCoroutine(DoChange(name, boxName, doEffect, onComplete));
    }

    IEnumerator DoChange(string newName, string boxName, bool doEffect, System.Action onComplete)
    {
        if (doEffect)
        {
            yield return StartCoroutine(animationEvent.FadeClear(null));
            //while (animationEvent.isRunning)
            //{
            //    yield return null;
            //}
        }
        nameTag.text = newName;
        textBackground.sprite = Resources.Load<Sprite>("Artwork/UI/Text Box/" + boxName);
        if (doEffect)
        {
            yield return StartCoroutine(animationEvent.FadeOpaque(null));
            //while (animationEvent.isRunning)
            //{
            //    yield return null;
            //}
        }
        onComplete();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class NameTagChange : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public Canvas canvas;
    DialogueTracker dialogueTracker;
    TMPro.TextMeshProUGUI nameTag;
    SpriteRenderer spriteRenderer;
    Animator animator;
    CanvasGroup canvasGroup;
    public float speed = 10f;
    Dictionary<string, string> nameToTextureDict;
    AnimationEvent animationEvent;
    InMemoryVariableStorage variableStorage;
    // Start is called before the first frame update
    void Awake()
    {
        dialogueRunner.AddCommandHandler("nametag", ChangeNameTag);
        canvasGroup = canvas.gameObject.GetComponent<CanvasGroup>();
        animator = canvas.gameObject.GetComponent<Animator>();
        animationEvent = canvas.gameObject.GetComponent<AnimationEvent>();
        nameTag = canvas.gameObject.transform.Find("NameTag").gameObject.GetComponent<TextMeshProUGUI>();
        spriteRenderer = canvas.gameObject.transform.Find("Box").gameObject.transform.Find("Image").gameObject.GetComponent<SpriteRenderer>();
        variableStorage = GameObject.Find("Variable Storage").GetComponent<InMemoryVariableStorage>();
        dialogueTracker = GameObject.Find("DialogueTracker").GetComponent<DialogueTracker>();
        animator.SetFloat("Speed", speed);
        nameToTextureDict = new Dictionary<string, string>();
        nameToTextureDict.Add("Mira", "Box_Red");
        nameToTextureDict.Add("Melly", "Box_Blue");
    }

    public void ChangeNameTag(string[] parameters, System.Action onComplete)
    {
        string boxName = PlayerPrefs.GetString("theme_color") + "_" + nameToTextureDict[parameters[0]];
        var doEffect = !animationEvent.isHidden;
        // add name to log
        dialogueTracker.UpdateNametag(parameters[0]);
        StartCoroutine(DoChange(parameters[0], boxName, doEffect, onComplete));
    }

    IEnumerator DoChange(string newName, string boxName, bool doEffect, System.Action onComplete)
    {
        if (doEffect)
        {
            animationEvent.FadeClear();
            while (animationEvent.isRunning)
            {
                yield return null;
            }
        }
        nameTag.text = newName;
        spriteRenderer.sprite = Resources.Load<Sprite>("Artwork/UI/Text Box/" + boxName);
        if (doEffect)
        {
            animationEvent.FadeOpaque();
            while (animationEvent.isRunning)
            {
                yield return null;
            }
        }
        onComplete();
    }
}

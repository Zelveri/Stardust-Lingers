using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarn;
using Yarn.Unity;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DialogueRunner))]
//[RequireComponent(typeof(VariableStorage))]
public class BackgroundChange : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    SpriteRenderer blendHelper;
    Animator animator;
    public DialogueRunner dialogueRunner;
    public VariableStorageBehaviour variableStorage;
    bool animationActive = false;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("backdrop", ChangeBackdrop);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        blendHelper = gameObject.transform.Find("SmoothTransitionHelper").gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangeBackdrop(string[] parameters, System.Action onComplete)
    {
        StartCoroutine(DoChange(parameters[0], onComplete));
    }
    
    public IEnumerator DoChange(string backdrop, System.Action onComplete)
    {
        // string timeName = variableStorage.GetValue("time").AsString;
        string spritePath = "Artwork/Backgrounds/" + backdrop;// + "_" + timeName;
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        // load new background under current one
        blendHelper.sprite = sprite;
        // start animation that makes current background transparent, revealing new one
        animator.SetTrigger("Start");
        animationActive = true;
        while (animationActive)
        {
            yield return null;
        }
        // set new background to def renderer
        spriteRenderer.sprite = sprite;
        // make def Renderer visible again
        spriteRenderer.color = new Color(1, 1, 1, 1);
        onComplete();
    }

    public IEnumerator DoChangeFast(string backdrop)
    {
        // string timeName = variableStorage.GetValue("time").AsString;
        string spritePath = "Artwork/Backgrounds/" + backdrop;// + "_" + timeName;
        spriteRenderer.sprite = Resources.Load<Sprite>(spritePath);
        spriteRenderer.color = Color.white;
        yield return null;
    }

    public void AnimationEnd()
    {
        animationActive = false;
    }
}

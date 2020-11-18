using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarn;
using Yarn.Unity;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DialogueRunner))]
//[RequireComponent(typeof(VariableStorage))]
public class BackgroundChangeHandler : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    SpriteRenderer blendHelper;
    public float animationDuration = 1f;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        blendHelper = gameObject.transform.Find("SmoothTransitionHelper").gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangeBackdrop(string backdrop, System.Action onComplete)
    //public void ChangeBackdrop(string[] parameters)
    {
        GameManager.dataController.UpdateBackdrop(backdrop);
        StartCoroutine(DoChange(backdrop, onComplete));
    }

    public void ChangeBackdropFast(string backdrop, System.Action onComplete)
    //public void ChangeBackdrop(string[] parameters)
    {
        GameManager.dataController.UpdateBackdrop(backdrop);
        StartCoroutine(DoChangeFast(backdrop, onComplete));
    }

    public IEnumerator DoChange(string backdrop, System.Action onComplete)
    {
        // string timeName = variableStorage.GetValue("time").AsString;
        string spritePath = "Artwork/Backgrounds/" + backdrop;// + "_" + timeName;
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        blendHelper.color = new Color(1f, 1f, 1f, 0f);
        // load new background under current one
        blendHelper.sprite = sprite;
        // start animation that makes current background transparent, revealing new one
        //animator.SetTrigger("Start");
        //animator.Play("Base Layer.BackgroundFade", -1, 0f);
        var startTime = Time.time;
        var endTime = Time.time + animationDuration;
        var elapsedTime = 0f;

        // set the canvas to the start alpha – this ensures that the canvas is ‘reset’ if you fade it multiple times
        var startAlpha = 0f;
        var endAlpha = 1f;
        blendHelper.color = new Color(1f, 1f, 1f, startAlpha);
        // loop repeatedly until the previously calculated end time
        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (animationDuration / elapsedTime); // calculate how far along the timeline we are
            var a = startAlpha + percentage;
            blendHelper.color = new Color(1f,1f,1f,(a < endAlpha) ? a : endAlpha); // calculate the new alpha

            yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
        }
        blendHelper.color = new Color(1f,1f,1f, endAlpha); // force the alpha to the end a
        // set new background to def renderer
        spriteRenderer.sprite = sprite;
        // make def Renderer visible again
        blendHelper.color = new Color(1f, 1f, 1f, startAlpha);
        onComplete();
    }

    public IEnumerator DoChangeFast(string backdrop, System.Action onComplete)
    {
        // string timeName = variableStorage.GetValue("time").AsString;
        string spritePath = "Artwork/Backgrounds/" + backdrop;// + "_" + timeName;
        spriteRenderer.sprite = Resources.Load<Sprite>(spritePath);
        spriteRenderer.color = Color.white;
        // null propagation operator ? prevents Invoke() call when onComplete is null
        onComplete?.Invoke();
        yield return null;
    }
}

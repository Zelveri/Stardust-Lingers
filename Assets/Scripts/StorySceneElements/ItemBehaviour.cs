using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour
{
    MyDialogueRunner dialogueRunner;
    public Image itemImage;
    public Animator itemAnimator;
    public AnimationState state;

    private void Awake()
    {
        // get persistent dialogueRunner object
        dialogueRunner = GameManager.dialogueRunner;
        // register command
        dialogueRunner.AddCommandHandler("item", Item);
    }

    private void OnDestroy()
    {
        // remove command handler to prevent overlap with other scripts and scenes
        dialogueRunner.RemoveCommandHandler("item");
    }

    public void Item(string[] parameters, System.Action onComplete)
    {
        // if parameter is "None" fade item out
        // otherwise interpret param as filename and fade item in
        if ("none None hide Hide".Contains(parameters[0]))
        {
            StartCoroutine(FadeClear(onComplete));
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>("Artwork/Items/" + parameters[0]);
            StartCoroutine(FadeOpaque(sprite, onComplete));
        }
    }

    IEnumerator FadeClear(System.Action onComplete)
    {
        state.AnimationStart();
        itemAnimator.SetTrigger("Fade_Clear");
        while (state.IsRunning)
        {
            yield return null;
        }
        itemImage.gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    IEnumerator FadeOpaque(Sprite sprite, System.Action onComplete)
    {
        itemImage.sprite = sprite;
        // set alpha to 0 before displaying image, prevent flashing 
        itemImage.color = new Color(1, 1, 1, 0);
        itemImage.gameObject.SetActive(true);
        state.AnimationStart();
        itemAnimator.SetTrigger("Fade_Opaque");
        while (state.IsRunning)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }
}

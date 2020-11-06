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

    public SpriteRenderer spriteRenderer;
    public DialogueRunner dialogueRunner;
    public VariableStorageBehaviour variableStorage;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("backdrop", ChangeBackdrop);
        //dialogueRunner.AddCommandHandler("transition", ScreenTransition);
        // dialogueRunner.AddCommandHandler("time", SetTime);
    }

    public void ChangeBackdrop(string[] parameters)
    {
        StartCoroutine(DoChange(parameters[0]));
    }
    
    public IEnumerator DoChange(string backdrop)
    {
        // string timeName = variableStorage.GetValue("time").AsString;
        string spritePath = "Artwork/Backgrounds/" + backdrop;// + "_" + timeName;
        spriteRenderer.sprite = Resources.Load<Sprite>(spritePath);
        yield return null;

        //switch (backdropName)
        //{
        //    case "Cabin_Dawn":
        //        spriteRenderer.sprite = backgrounds[0];
        //        break;
        //    case "Cabin_Day":
        //        spriteRenderer.sprite = backgrounds[1];
        //        break;
        //    case "Cabin_Night":
        //        spriteRenderer.sprite = backgrounds[2];
        //        break;
        //    default:
        //        spriteRenderer.sprite = null;
        //        break;
        //}
    }
}

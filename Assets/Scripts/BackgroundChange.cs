using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarn;
using Yarn.Unity;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(DialogueRunner))]
public class BackgroundChange : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public DialogueRunner dialogueRunner;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("backdrop", ChangeBackdrop);
    }

    public void ChangeBackdrop(string[] parameters)
    {
        string backdropName = parameters[0];
        string spritePath = Application.dataPath + "/Artwork/Backgrounds/" + backdropName + ".png";// + "_" + timeName;
        byte[] data = File.ReadAllBytes(spritePath);
        Texture2D texture = new Texture2D(1920, 1080, TextureFormat.ARGB32, false);
        texture.LoadImage(data);
        texture.name = Path.GetFileNameWithoutExtension(spritePath);
        Sprite icon = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        spriteRenderer.sprite = icon;

    }
}

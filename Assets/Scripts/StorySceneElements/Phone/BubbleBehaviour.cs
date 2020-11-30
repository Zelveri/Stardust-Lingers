using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BubbleBehaviour : MonoBehaviour
{
    public GameObject container;
    public MaskableGraphic top;
    public MaskableGraphic middle;
    public MaskableGraphic bottom;

    // is this bubble for text from mira?
    // set in the prefabs
    public bool isMeBubble;

    CanvasScaler canvasScaler;

    Graphic content;

    float heightScale = 1f;

    float oldHeight = 0;

    private void Awake()
    {
        content = container.GetComponent<Graphic>();
        var canvas = GameObject.Find("PhoneDialogue").GetComponent<Canvas>();
        canvasScaler = canvas.gameObject.GetComponent<CanvasScaler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText(" ");
    }

    // Update is called once per frame
    void Update()
    {
        // calc current height scale to react to resolution changes
        heightScale = Display.main.renderingHeight / canvasScaler.referenceResolution.y;
        AdjustBubbleSize();
    }

    public void UpdateText(string text)
    {
        if (content.GetType().Equals(typeof(TextMeshProUGUI)))
        {
            ((TextMeshProUGUI)content).text = text;
        }
        //AdjustBubbleSize();
    }

    public float GetHeight()
    {
        return top.rectTransform.rect.height + middle.rectTransform.rect.height + bottom.rectTransform.rect.height;
    }

    public float GetLastHeightDelta()
    {
        float delta = GetHeight() - oldHeight + 10 * heightScale;
        oldHeight =  GetHeight() + 10 * heightScale;
        return delta;
    }

    public void AdjustBubbleSize()
    {
        // adjust the height of the top bar to fit all the content
        var movVect = new Vector3(0, (content.rectTransform.rect.height - middle.rectTransform.rect.height) * heightScale, 0);
        top.rectTransform.Translate(movVect);

        // resize the middle section to fit the content
        middle.rectTransform.sizeDelta = new Vector2(middle.rectTransform.rect.width, content.rectTransform.rect.height);
    }

    public void MoveUp(float height)
    {
        // move the whole bubble up by the given amount
        var movVect = new Vector3(0, height * heightScale, 0);
        gameObject.transform.Translate(movVect);
    }

    public void ShowImage(string filename)
    {
        // destroy text field and content size fitter
        // dont need it for image
        DestroyImmediate(content.GetComponent<ContentSizeFitter>());
        DestroyImmediate(content.GetComponent<LayoutElement>());
        DestroyImmediate(content);
        content.material = content.defaultMaterial;

        // add image to the gamobobject
        content = container.AddComponent<Image>();
        // add aspect ratio fitter for ptoper image sizing
        var arf = container.AddComponent<AspectRatioFitter>();
       
        // load image and set apropriate width / height and aspect
        ((Image)content).preserveAspect = true;
        ((Image)content).sprite = Resources.Load<Sprite>("Artwork/Photos/"+filename);
        // set aspect ratio
        arf.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        arf.aspectRatio = ((Image)content).sprite.rect.width / ((Image)content).sprite.rect.height;
        ((Image)content).rectTransform.sizeDelta = new Vector2(400f, 100f);
    }

    // following functions are for saving bubbles but is not fully implemented yet

    public string GetContent()
    {
        if (content.GetType().Equals(typeof(TextMeshProUGUI)))
        {
            return ((TextMeshProUGUI)content).text;
        }
        else
        {
            return ((Image)content).sprite.name;
        }
    }

    public bool ContentIsImagePath()
    {
        if (content.GetType().Equals(typeof(TextMeshProUGUI)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetYPos()
    {
        return gameObject.transform.position[1];
    }

    public void SetYPos(float pos)
    {
        
    }

    public bool IsMeBubble()
    {
        return isMeBubble;
    }
}

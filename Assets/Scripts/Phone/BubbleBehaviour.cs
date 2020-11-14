using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BubbleBehaviour : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textField;
    public Image top;
    public Image middle;
    public Image bottom;

    string text = "";
    float oldHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText("");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(TestText());
        }
    }

    IEnumerator TestText()
    {
        while (true)
        {
            text += "a\n";
            UpdateText(text);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateText(string text)
    {
        textField.text = text;
        AdjustBubbleSize();
    }

    public float GetHeight()
    {
        return top.rectTransform.rect.height + middle.rectTransform.rect.height + bottom.rectTransform.rect.height;
    }

    public float GetLastHeightDelta()
    {
        float delta = top.rectTransform.rect.height + middle.rectTransform.rect.height + bottom.rectTransform.rect.height - oldHeight;
        oldHeight =  top.rectTransform.rect.height + middle.rectTransform.rect.height + bottom.rectTransform.rect.height;
        return delta;
    }

    void AdjustBubbleSize()
    {
        var movVect = new Vector3(0, textField.rectTransform.rect.height - middle.rectTransform.rect.height, 0);
        middle.rectTransform.sizeDelta = new Vector2(middle.rectTransform.rect.width, textField.rectTransform.rect.height);
    
        top.rectTransform.Translate(movVect);
    }

    public void MoveUp(float height)
    {
        var movVect = new Vector3(0, height,0);
        gameObject.transform.Translate(movVect);
        //top.rectTransform.Translate(movVect);
        //middle.rectTransform.Translate(movVect);
        //bottom.rectTransform.Translate(movVect);
    }
}

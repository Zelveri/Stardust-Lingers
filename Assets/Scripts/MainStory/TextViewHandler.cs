using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TextViewHandler : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textBox;

    private void Start()
    {
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateColor()
    {
        if (PlayerPrefs.GetString("theme_color") == "Light")
        {
            textBox.faceColor = new Color(0, 0, 0);
        }
        else
        {
            textBox.faceColor = new Color(1, 1, 1);
        }
    }
}

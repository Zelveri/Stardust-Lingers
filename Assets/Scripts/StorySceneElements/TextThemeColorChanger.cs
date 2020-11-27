﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TextThemeColorChanger : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textBox;

    private void Start()
    {
        UpdateColor();
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

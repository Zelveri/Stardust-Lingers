using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsHandler : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("theme_color"))
        {
            PlayerPrefs.SetString("theme_color", "Light");
        }
    }
}

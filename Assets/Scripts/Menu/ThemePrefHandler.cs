using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemePrefHandler : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("theme_color"))
        {
            SetLightTheme();
        }
        // set theme selector to current theme
        string setting = PlayerPrefs.GetString("theme_color");
        TMPro.TMP_Dropdown dropdown = gameObject.transform.Find("Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        int option = (setting == "Light") ? 0 : 1;
        dropdown.SetValueWithoutNotify(option);
        dropdown.RefreshShownValue();
    }

    public static void SetLightTheme()
    {
        PlayerPrefs.SetString("theme_color", "Light");
        PlayerPrefs.Save();
    }

    public static void SetDarkTheme()
    {
        PlayerPrefs.SetString("theme_color", "Dark");
        PlayerPrefs.Save();
    }

    public void OnSettingChanged(int pos)
    {
        if(pos == 0)
        {
            SetLightTheme();
        }
        else
        {
            SetDarkTheme();
        }
    }
}

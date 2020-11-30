using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSpeedBehaviour : MonoBehaviour
{
    public string prefName;
    public Slider speedSlider;

    private void Awake()
    {
        speedSlider.value = PlayerPrefs.HasKey(prefName) ? PlayerPrefs.GetFloat(prefName) : 0.025f;
        speedSlider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float value)
    {
        PlayerPrefs.SetFloat(prefName, value);
    }
}

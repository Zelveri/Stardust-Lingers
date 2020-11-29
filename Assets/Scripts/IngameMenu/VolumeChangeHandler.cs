using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// will update a PlayerPreference named by prefName on function call
/// and then call an update event
/// </summary>
public class VolumeChangeHandler : MonoBehaviour
{
    public string prefName;
    public Slider volumeSlider;

    private void Awake()
    {
        volumeSlider.value = PlayerPrefs.HasKey(prefName) ? PlayerPrefs.GetFloat(prefName) : 1f;
        volumeSlider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float value)
    {
        PlayerPrefs.SetFloat(prefName, value);
        GameManager.OnVolumeChanged.Invoke();
    }
}

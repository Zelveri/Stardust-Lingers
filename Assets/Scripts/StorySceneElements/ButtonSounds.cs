using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    public AudioClip soundOnHover;
    public AudioClip soundOnClick;

    private void Awake()
    {
        // register event on hover over button
        var trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener(OnHover);
        trigger.triggers.Add(entry);

        // and on click
        var btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnHover(BaseEventData data)
    {
        if (soundOnHover != null) GameManager.soundEffects.PlaySound(soundOnHover, loop: false);
    }

    void OnClick()
    {
        if (soundOnClick != null) GameManager.soundEffects.PlaySound(soundOnClick, loop: false);
    }

}

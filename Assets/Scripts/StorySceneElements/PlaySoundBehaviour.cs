using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// attatch to gameobject to enable sound playing on events
/// </summary>
public class PlaySoundBehaviour : MonoBehaviour
{
    public AudioClip[] sound;

    public void PlaySound(int index)
    {
        GameManager.soundEffects.PlaySound(sound[index].name, loop: false);
    }

    public void PlaySoundLooping(int index)
    {
        GameManager.soundEffects.PlaySound(sound[index].name, loop: true);
    }
    public void PlaySoundFade(int index)
    {
        GameManager.soundEffects.PlaySound(sound[index].name, fade: true);
    }

    public void StopSound(int index)
    {
        GameManager.soundEffects.StopSound(sound[index].name);
    }

    public void StopSoundFade(int index)
    {
        GameManager.soundEffects.StopSound(sound[index].name, fade: true);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MusicPlayerBehaviour : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    AudioSource player;

    private void Awake()
    {
        
        player = GetComponent<AudioSource>();
        // try to load the saved music volume
        if (!PlayerPrefs.HasKey("music_volume"))
        {
            PlayerPrefs.SetFloat("music_volume", 1);
        }
        else
        {
            player.volume = PlayerPrefs.GetFloat("music_volume");
        }

        GameManager.OnVolumeChanged.AddListener(VolumeChanged);
    }

    private void OnDestroy()
    {
        GameManager.OnVolumeChanged.RemoveListener(VolumeChanged);
    }

    public void VolumeChanged()
    {
        float newVolume = PlayerPrefs.GetFloat("music_volume");
        player.volume = newVolume;
    }

    public void PlayMusic(string[] parameters)
    {
        string file = "";
        // default fade time 3 seconds
        float duration = 3f;
        AudioClip clip;
        string musicPath = "";
        switch (parameters[0])
        {
            case "stop": // abrupt music stop
                player.Stop();
                break;

            case "fade_out": // music fade out with optional duration argument
                // check if duration given
                if(parameters.Length > 1)
                {
                    duration = float.Parse(parameters[1]);
                }
                StartCoroutine(FadeOut(player, duration));
                break;

            case "fade_in": // music fade in 
                // check if filename given
                if (parameters.Length > 1)
                {
                    file = parameters[1];
                } else Debug.LogError("Music Player - Fade In: No file given");
                //check if duration given
                if (parameters.Length > 2)
                {
                    duration = float.Parse(parameters[2]);
                }
                // load music file and trigger coroutine
                musicPath = "Music/" + file;
                clip = Resources.Load<AudioClip>(musicPath);
                StartCoroutine(FadeIn(player, duration, clip));
                break;

            case "play":
                // check if file given
                if (parameters.Length > 1)
                {
                    file = parameters[1];
                } else Debug.LogError("Music Player - Play: No file given");
                // load file and play
                musicPath = "Music/" + file;
                clip = Resources.Load<AudioClip>(musicPath);
                player.clip = clip;
                player.Play();
                break;
        }
    }

    // from https://forum.unity.com/threads/fade-out-audio-source.335031/
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime, AudioClip clip)
    {
        float targetVolume = audioSource.volume;
        audioSource.volume = 0;
        audioSource.clip = clip;
        audioSource.Play();
        while (audioSource.volume > 0)
        {
            audioSource.volume += targetVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}


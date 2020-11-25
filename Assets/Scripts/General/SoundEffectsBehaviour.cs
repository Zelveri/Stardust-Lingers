using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundEffectsBehaviour : MonoBehaviour
{
    public GameObject soundEffectPrefab;
    MyDialogueRunner dialogueRunner;
    // keeps track of all currently playing sounds
    Dictionary<string, AudioSource> sfxPlayers;

    private void Awake()
    {
        dialogueRunner = GameManager.dialogueRunner;
        // sound (play/stop) <name> [loop] [fade]
        dialogueRunner.AddCommandHandler("sound", Sound);
        if (!PlayerPrefs.HasKey("sfx_volume"))
        {
            PlayerPrefs.SetFloat("sfx_volume", 1);
        }
        sfxPlayers = new Dictionary<string, AudioSource>();
    }

    private void OnDestroy()
    {
        dialogueRunner?.RemoveCommandHandler("sound");
    }

    public void Sound(string[] pars)
    {
        bool loop = false;
        bool fade = false;
        if(pars.Length < 2)
        {
            Debug.LogError("sound command: too few arguments");
            return;
        }
        if (pars.Length > 2)
        {
            loop = pars.Any("loop".Contains);
            fade = pars.Any("fade".Contains);
        }
        switch (pars[0])
        {
            case "play":
                PlaySound(pars[1], loop, fade);
                break;

            case "stop":
                StopSound(pars[1]);
                break;
        }
    }

    public void PlaySound(string name, bool loop = true, bool fade = false)
    {
        // create new gameobject as sound player
        AudioSource src = Instantiate(soundEffectPrefab).GetComponent<AudioSource>();
        try
        {
            src.gameObject.SetActive(true);
            src.clip = Resources.Load<AudioClip>("Sounds/" + name);
            src.loop = loop;
            // volume as set in menu
            src.volume = PlayerPrefs.GetFloat("sfx_volume");

            // do a quick fade in of 2 sec if wanted
            if (fade)
            {
                StartCoroutine(FadeIn(src, 2f));
            }
            else
            {
                src.Play();
            }
            // add sound source to dict to remember who plays what sound
            if (loop)
            {
                sfxPlayers.Add(name, src);
            }
            else
            {
                // destroy object when sound finishes playing
                StartCoroutine(DestroyAfterPlay(src));
            }
        }
        catch(Exception ex)
        {
            Debug.LogWarning("Sound player: " + ex.Message);
        }
    }

    public void StopSound(string name, bool fade = false)
    {
        if (sfxPlayers.ContainsKey(name))
        {
            if (fade)
            {
                StartCoroutine(FadeOut(sfxPlayers[name], 2f));
            }
            else
            {
                sfxPlayers[name].Stop();
                RemoveSource(name);
            }
        }
    }

    public void StopAll()
    {
        foreach(var key in sfxPlayers.Keys)
        {
            sfxPlayers[name].Stop();
            RemoveSource(name);
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
        Destroy(audioSource.gameObject);
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float targetVolume = audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume > 0)
        {
            audioSource.volume += targetVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    IEnumerator DestroyAfterPlay(AudioSource src)
    {
        yield return new WaitUntil(() => !src.isPlaying);
        Destroy(src.gameObject);
    }

    void RemoveSource(string name)
    {
        Destroy(sfxPlayers[name].gameObject);
        sfxPlayers.Remove(name);
    }
}

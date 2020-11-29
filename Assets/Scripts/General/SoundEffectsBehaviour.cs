using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundEffectsBehaviour : MonoBehaviour
{
    public GameObject soundEffectPrefab;
    // keeps track of all currently playing sounds
    Dictionary<string, AudioSource> sfxPlayers;

    private void Awake()
    {
        //dialogueRunner = GameManager.dialogueRunner;
        
        //dialogueRunner.AddCommandHandler("sound", Sound);
        if (!PlayerPrefs.HasKey("sfx_volume"))
        {
            PlayerPrefs.SetFloat("sfx_volume", 1);
        }
        sfxPlayers = new Dictionary<string, AudioSource>();
        GameManager.OnVolumeChanged.AddListener(VolumeChanged);
    }

    private void OnDestroy()
    {
        GameManager.OnVolumeChanged.RemoveListener(VolumeChanged);
    }

    // update volume on every looping sound effect
    public void VolumeChanged()
    {
        float newVolume = PlayerPrefs.GetFloat("sfx_volume");
        foreach(var item in sfxPlayers)
        {
            item.Value.volume = newVolume;
        }
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
            // check if looping or fading wanted by applying lambda expression to all given params
            loop = pars.Any("loop".Contains);
            fade = pars.Any("fade".Contains);
        }
        switch (pars[0].ToLower())
        {
            case "play":
                PlaySound(pars[1], loop, fade);
                break;

            case "stop":
                StopSound(pars[1], fade);
                break;
        }
    }

    public void PlaySound(string name, bool loop = true, bool fade = false)
    {
        StartCoroutine(DoPlaySound(name, loop, fade));
    }


    IEnumerator DoPlaySound(string name, bool loop = true, bool fade = false)
    {
        // load audio file async
        var res_req = Resources.LoadAsync<AudioClip>("Sounds/" + name);
        yield return new WaitUntil(() => res_req.isDone);
        // call playback fcn when resource is loaded
        PlaySound((AudioClip)res_req.asset, loop, fade);
    }

    public void PlaySound(AudioClip clip, bool loop = true, bool fade = false)
    {
        // create new gameobject as sound player
        GameObject go = Instantiate(soundEffectPrefab);
        go.transform.parent = gameObject.transform;
        AudioSource src = go.GetComponent<AudioSource>();
        src.gameObject.SetActive(true);
        src.loop = loop;
        src.clip = clip;
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
            sfxPlayers.Add(clip.name, src);
            GameManager.dataController.UpdateSounds(sfxPlayers.Keys.ToArray<string>());
        }
        else
        {
            // destroy object when sound finishes playing
            StartCoroutine(DestroyAfterPlay(src));
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
            }
            RemoveSource(name);
        }
    }

    public void StopAll()
    {
        foreach(var key in sfxPlayers.Keys)
        {
            StartCoroutine(FadeOut(sfxPlayers[key], 1f, false));
            RemoveSource(key);
        }
    }

    //pause all !registered! sounds with a short fadeout
    // registered meaning they were invoked in a loop
    public void PauseAll()
    {
        foreach (var player in sfxPlayers.Values)
        {
            StartCoroutine(FadeOut(player, 1f, true));
        }
    }

    // unpause all !registered! sounds with fadein
    public void UnPauseAll()
    {
        foreach (var player in sfxPlayers.Values)
        {
            StartCoroutine(FadeIn(player, 1f));
        }
    }

    // from https://forum.unity.com/threads/fade-out-audio-source.335031/
    /// <summary>
    /// Fade the vloume to 0 for the given audio source
    /// </summary>
    /// <param name="audioSource">the sound effects player</param>
    /// <param name="FadeTime">how long to fade</param>
    /// <param name="onlyPause">fade to pause instad to stop?</param>
    /// <returns></returns>
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime, bool onlyPause=false)
    {
        float startVolume = audioSource.volume;
        // if source is already paused or stopped, do nothing
        if (audioSource.isPlaying)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.unscaledDeltaTime / FadeTime;

                yield return null;
            }

            if (onlyPause)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Stop();             
            }
            audioSource.volume = startVolume;
        }

    }

    /// <summary>
    /// fade volume to preset of given source
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="FadeTime"></param>
    /// <returns></returns>
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float targetVolume = audioSource.volume;
        audioSource.volume = 0;

        // this will also unpause paused sources
        audioSource.Play();
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.unscaledDeltaTime / FadeTime;

            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    // destroys singleshot sounds after they finish playing
    IEnumerator DestroyAfterPlay(AudioSource src)
    {
        yield return new WaitUntil(() => !src.isPlaying);
        Destroy(src.gameObject);
    }

    void RemoveSource(string name)
    {
        StartCoroutine(DoRemove(name));
    }

    // wait until the source stops playing to remove it
    // makes it possible to fade it out before destroying it
    IEnumerator DoRemove(string name)
    {
        yield return new WaitUntil(() => !sfxPlayers[name].isPlaying);
        Destroy(sfxPlayers[name].gameObject);
        sfxPlayers.Remove(name);
        GameManager.dataController.UpdateSounds(sfxPlayers.Keys.ToArray<string>());
    }
}

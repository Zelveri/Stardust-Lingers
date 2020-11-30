using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MusicPlayerBehaviour : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    AudioSource player;

    string curPlayingFile = "";

    static bool isFading = false;

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
        // register to the volume changed event, to make live volume preview possible
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
        switch (parameters[0].ToLower())
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
                    // dont play file if already playing
                    if (curPlayingFile == file)
                    {
                        return;
                    }
                    else
                    {
                        curPlayingFile = file;
                    }
                } 
                else Debug.LogError("Music Player - Fade In: No file given");
                //check if duration given
                if (parameters.Length > 2)
                {
                    duration = float.Parse(parameters[2]);
                }
                // load music file and trigger coroutine
                musicPath = "Music/" + file;
                clip = Resources.Load<AudioClip>(musicPath);
                player.clip = clip;
                GameManager.dataController.UpdateMusic(file);
                StartCoroutine(FadeIn(player, duration));
                break;

            case "play":
                // check if file given
                if (parameters.Length > 1)
                {
                    file = parameters[1];
                    // dont play file if already playing
                    if (curPlayingFile == file)
                    {
                        return;
                    }
                    else
                    {
                        curPlayingFile = file;
                    }
                }
                else Debug.LogError("Music Player - play: No file given");
                // load file and play
                musicPath = "Music/" + file;
                clip = Resources.Load<AudioClip>(musicPath);
                GameManager.dataController.UpdateMusic(file);
                if (player.isPlaying) player.Stop();
                player.clip = clip;
                player.Play();
                break;
        }
    }

    public void Play(string file)
    {
        if (file == null || file == "") return;
        var musicPath = "Music/" + file;
        var clip = Resources.Load<AudioClip>(musicPath);
        player.clip = clip;
        curPlayingFile = file;
        GameManager.dataController.UpdateMusic(file);
        StartCoroutine(FadeIn(player, 3f));
    }

    public void PlayMenuMusic() 
    {
        if (curPlayingFile == "Stardust_menu") return;
        var clip = Resources.Load<AudioClip>("Music/Stardust_menu");
        player.clip = clip;
        curPlayingFile = "Stardust_menu";
        GameManager.dataController.UpdateMusic("Stardust_menu");
        StartCoroutine(FadeIn(player, 2f));
    }

    public void Pause()
    {
        StartCoroutine(FadeOut(player, 1f, true));
    }

    public void UnPause()
    {
        StartCoroutine(FadeIn(player, 1f));
    }

    public void Stop()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut(player, 1f, false));
    }

    /// <summary>
    /// Waits until the musicplayer has finished fading out the music
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitFadeOut()
    {
        yield return new WaitWhile(() => isFading);
    }


    // from https://forum.unity.com/threads/fade-out-audio-source.335031/
    /// <summary>
    /// Fade the vloume to 0 for the given audio source
    /// </summary>
    /// <param name="audioSource">the sound effects player</param>
    /// <param name="FadeTime">how long to fade</param>
    /// <param name="onlyPause">fade to pause instad to stop?</param>
    /// <returns></returns>
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime, bool onlyPause = false)
    {
        isFading = true;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime / FadeTime;

            yield return null;
        }

        if (onlyPause)
        {
            audioSource.Pause();
            audioSource.volume = startVolume;
        }
        else
        {
            audioSource.Stop();
            audioSource.volume = startVolume;
        }
        isFading = false;
    }

    /// <summary>
    /// fade volume to preset of given source
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="FadeTime"></param>
    /// <returns></returns>
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        isFading = true;
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
        isFading = false;
    }
}


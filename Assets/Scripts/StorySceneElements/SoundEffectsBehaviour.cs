using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsBehaviour : MonoBehaviour
{
    public GameObject soundEffectPrefab;
    // keeps track of all currently playing sounds
    Dictionary<string, AudioSource> sfxPlayers;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("sfx_volume"))
        {
            PlayerPrefs.SetFloat("sfx_volume", 1);
        }
        sfxPlayers = new Dictionary<string, AudioSource>();
    }

    public void PlaySound(string name, bool loop = true)
    {
        // create new gameobject as sound player
        AudioSource src = GameObject.Instantiate(soundEffectPrefab).GetComponent<AudioSource>();
        sfxPlayers.Add(name,src);
        src.clip = Resources.Load<AudioClip>("Sounds/" + name);
        src.loop = loop;
        src.volume = PlayerPrefs.GetFloat("sfx_volume");
        src.Play();
    }

    public void StopSound(string name)
    {
        if (sfxPlayers.ContainsKey(name))
        {
            sfxPlayers[name].Stop();
            RemoveSource(name);
        }
    }

    public void FadeIn(string name, bool loop = true)
    {

    }

    public void FadeOut(string name)
    {

    }

    void RemoveSource(string name)
    {
        Destroy(sfxPlayers[name].gameObject);
        sfxPlayers.Remove(name);
    }
}

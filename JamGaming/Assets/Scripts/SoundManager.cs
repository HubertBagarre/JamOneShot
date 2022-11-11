using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<AudioClip> soundClips = new List<AudioClip>();
    private Dictionary<PlayerInfo, List<AudioSource>> sources = new Dictionary<PlayerInfo, List<AudioSource>>();

    public static SoundManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(int index)
    {
        musicSource.clip = musicClips[index];
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void CreateSources(PlayerInfo player)
    {
        var audioSources = new List<AudioSource>();
        foreach (var clip in soundClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            audioSources.Add(source);
        }
        sources.Add(player,audioSources);
    }

    public void ClearSources()
    {
        sources.Clear();
    }

    public void PlaySound(PlayerInfo player, int index)
    {
        if(!sources.ContainsKey(player)) return;
        sources[player][index].Play();
    }
}
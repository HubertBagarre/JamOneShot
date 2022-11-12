using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<AudioClip> soundClips = new List<AudioClip>();
    private List<AudioSource> globalSources = new ();
    private Dictionary<PlayerInfo, List<AudioSource>> sources = new Dictionary<PlayerInfo, List<AudioSource>>();

    private bool isInLoop = true;
    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMenuMusic();
        foreach (var clip in soundClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.loop = false;
            globalSources.Add(source);
        }
    }

    public void PlayMusic(int index)
    {
        musicSource.clip = musicClips[index];
        musicSource.Play();
    }

    private void Update()
    {
        if (!isInLoop)
        {
            if(musicSource.isPlaying) return;
            musicSource.loop = true;
            isInLoop = true;
            PlayMusic(1);
        }
    }

    public void StartGameMusic()
    {
        musicSource.loop = false;
        isInLoop = false;
        PlayMusic(0);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(2);
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

        sources.Add(player, audioSources);
    }

    public void ClearSources()
    {
        sources.Clear();
    }

    public void PlaySound(PlayerInfo player, int index)
    {
        if (!sources.ContainsKey(player)) return;
        if (index < 0 || index > sources[player].Count) return;
        sources[player][index].Play();
    }

    public void PlaySound(int index)
    {
        if (index < 0 || index > globalSources.Count) return;
        globalSources[index].Play();
    }
}
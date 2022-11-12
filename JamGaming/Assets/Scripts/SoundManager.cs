using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<Sound> soundClips = new ();
    private List<AudioSource> globalSources = new ();
    private Dictionary<PlayerInfo, List<AudioSource>> sourceDict = new Dictionary<PlayerInfo, List<AudioSource>>();

    private bool isInLoop = true;
    public static SoundManager instance = null;
    
    [Serializable]
    public class Sound
    {
        public AudioClip clip;
        [Range(0f,1f)] public float volume = 1f;
    }

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
        foreach (var sound in soundClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = false;
            source.playOnAwake = false;
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
        musicSource.volume = 0.2f;
        PlayMusic(2);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void CreateSources(PlayerInfo player)
    {
        var audioSources = new List<AudioSource>();
        foreach (var sound in soundClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = false;
            source.playOnAwake = false;
            audioSources.Add(source);
        }

        sourceDict.Add(player, audioSources);
    }

    public void ClearSources()
    {
        foreach (var player in sourceDict.Keys)
        {
            foreach (var source in sourceDict[player])
            {
                Destroy(source);
            }
            
        }
        sourceDict.Clear();
    }

    public void PlaySound(PlayerInfo player, int index)
    {
        if (!sourceDict.ContainsKey(player)) return;
        if (index < 0 || index >= sourceDict[player].Count) return;
        sourceDict[player][index].Play();
    }

    public void PlaySound(int index)
    {
        if (index < 0 || index > globalSources.Count) return;
        globalSources[index].Play();
    }
}
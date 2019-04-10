using NewEngine.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : CService
{
    // background music
    private AudioSource musicAudio;
    // effect sound
    private AudioSource[] soundAudios;

    private static AudioManager sInstance = null;
    public static AudioManager Instance
    {
        get
        {
            return sInstance;
        }
    }

    public void Start()
    {
        sInstance = this;
        if (gameObject.GetComponent<AudioSource>() == null)
            musicAudio = gameObject.AddComponent<AudioSource>();
    }

    public void OnDestroy()
    {
        if (sInstance == this)
        {
            sInstance = null;
        }
    }

    public string MusicName
    {
        get
        {
            if (musicAudio.clip == null)
            {
                return "";
            }
            else
            {
                return musicAudio.clip.name;
            }
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            musicAudio.clip = clip;
        }
        if (musicAudio.clip != null)
        {
            musicAudio.loop = true;
            musicAudio.Play();
        }
    }

    public void PauseMusic()
    {
        musicAudio.Pause();
    }

    public void StopMusic()
    {
        musicAudio.Stop();
    }

    public bool MusicMute
    {
        set
        {
            musicAudio.mute = value;
        }
        get
        {
            return musicAudio.mute;
        }
    }

   
}

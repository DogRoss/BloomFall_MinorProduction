using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [Header("Music")]
    public List<AudioSource> music = new List<AudioSource>();
    AudioSource currentMusic;
    AudioSource nextMusic;
    [Range(0, 100)]
    public float musicVolume = 50f;
    public bool cycleThroughMusic = false;

    [Header("Sound Effects")]
    public List<AudioSource> sfx = new List<AudioSource>();
    [Range(0, 100)]
    public float sfxVolume = 50f;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        currentMusic = music[0];
        nextMusic = music[1];

        currentMusic.Play();


    }

    // Update is called once per frame
    void Update()
    {
        CycleMusic();
    }

    [ContextMenu("PlaySound")]
    public void ContextSound()
    {
        //for(int i = 0; i < sfx.Count; i++)
        //{
        //    sfx[i].Play();
        //}

        sfx[0].Play();
    }

    public void ChangeSFXVolume(float volume)
    {
        sfxVolume = volume;
        UpdateVolume();
    }
    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateVolume();
    }
    public void UpdateVolume()
    {
        currentMusic.volume = musicVolume / 100;
    }

    public void CycleMusic()
    {
        //Debug.Log(currentMusic.time + " // playback position");
        //cycles through music
        if (cycleThroughMusic)
        {
            if (!currentMusic.isPlaying)
            {
                int index = FindIndexOfMusic(nextMusic);
                currentMusic = music[index];
                if (music[index + 1] != null)
                {
                    nextMusic = music[index + 1];
                }
                else
                {
                    nextMusic = music[0];
                }

                currentMusic.Play();
            }
        }
        else
        {
            if (!currentMusic.isPlaying)
            {
                currentMusic.Play();
            }
        }
    }
    public bool MusicExists(AudioSource musicAudio)
    {
        bool exists = false;
        for (int i = 0; i < music.Count; i++)
        {
            if (music[i] = musicAudio)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }
    //used to find index in list of specific sound, only should be used if the sound actually exists
    public int FindIndexOfMusic(AudioSource musicAudio)
    {
        int returnNum = 0;
        for (int i = 0; i < music.Count; i++)
        {
            if (music[i] = musicAudio)
            {
                returnNum = i;
                break;
            }
        }
        return returnNum;
    }
    public void TryFindIndexOfMusic(AudioSource musicAudio, out int indexReturn)
    {
        indexReturn = 0;
        for (int i = 0; i < music.Count; i++)
        {
            if (music[i] = musicAudio)
            {
                indexReturn = i;
                break;
            }
        }
    }

    //tools for finding sounds
    public bool SFXExists(AudioSource sound)
    {
        bool exists = false;
        for (int i = 0; i < sfx.Count; i++)
        {
            if (sfx[i] = sound)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }
    public int FindIndexOfSFX(AudioSource sound)
    {
        int returnNum = 0;
        for (int i = 0; i < sfx.Count; i++)
        {
            if (sfx[i] = sound)
            {
                returnNum = i;
                break;
            }
        }
        return returnNum;
    }
    public void TryFindIndexOfSFX(AudioSource sound, out int indexReturn)
    {
        indexReturn = 0;
        for (int i = 0; i < sfx.Count; i++)
        {
            if (sfx[i] = sound)
            {
                indexReturn = i;
                break;
            }
        }
    }

    //plays sfx
    public void PlaySound(int index)
    {
        sfx[index].volume = sfxVolume / 100f;
        sfx[index].Play();
    }
    public void PlaySound(AudioSource sound)
    {
        for (int i = 0; i < sfx.Count; i++)
        {
            if (sfx[i] == sound)
            {
                PlaySound(i);
                break;
            }
        }
    }

}

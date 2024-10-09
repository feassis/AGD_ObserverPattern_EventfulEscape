using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundView : MonoBehaviour
{
    [SerializeField] private AudioSource audioEffects;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private Sounds[] audioList;

    private void Start() => playbackgroundMusic(SoundType.BackgroundMusic, true);

    public void PlaySoundEffects(SoundType soundType, bool loopSound = false)
    {
        AudioClip clip = getSoundClip(soundType);
        if (clip != null)
        {
            audioEffects.loop = loopSound;
            audioEffects.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("No Audio Clip got selected");
        }
    }

    public void StopSoundEffects()
    {
        audioEffects.Stop();
    }

    private void playbackgroundMusic(SoundType soundType, bool loopSound = false)
    {
        AudioClip clip = getSoundClip(soundType);
        if (clip != null)
        {
            backgroundMusic.loop = loopSound;
            backgroundMusic.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("No Audio Clip got selected");
        }
    }


    private AudioClip getSoundClip(SoundType soundType)
    {
        Sounds st = Array.Find(audioList, item => item.soundType == soundType);
        if (st != null)
        {
            return st.audio;
        }
        return null;
    }
}

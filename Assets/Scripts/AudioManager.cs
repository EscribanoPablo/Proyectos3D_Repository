using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource audioMusic;
    [SerializeField]
    AudioSource audioSFX;

    [Header("AudioClips")]
    AudioClip menuMusic;
    AudioClip backgroundLevelMusic;

    public void playMusic(AudioClip musicClip)
    {
        audioMusic.clip = musicClip;
        audioMusic.Play();
    }

    public void playSFX(AudioClip sfxClip)
    {
        audioSFX.PlayOneShot(sfxClip);
    }
}

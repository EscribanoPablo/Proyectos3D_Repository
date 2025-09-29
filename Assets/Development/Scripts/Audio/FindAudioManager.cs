using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindAudioManager : MonoBehaviour
{
    [SerializeField]
    private Slider SFXVolumeSlider;
    [SerializeField]
    private Slider MusicVolumeSlider;

    private void Update()
    {
        SFXVolumeSlider.value = FindObjectOfType<AudioManager>().GetSXFVolume();
        MusicVolumeSlider.value = FindObjectOfType<AudioManager>().GetMusicVolume();
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        FindObjectOfType<AudioManager>().MusicVolumeLevel(newMusicVolume);
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        FindObjectOfType<AudioManager>().SFXVolumeLevel(newSFXVolume);
    }
}

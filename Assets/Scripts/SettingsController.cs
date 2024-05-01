using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown qualityDropdown;

    //[SerializeField]
    //private Slider sfxVolume;
    //[SerializeField]
    //private Slider musicVolume;
    //
    //[SerializeField]
    //private AudioMixer audioMixer;

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    public void ChangeVolume()
    {
        //audioMixer.SetFloat("musicVol", musicVolume.value);
        //audioMixer.SetFloat("sfxVol", sfxVolume.value);
    }
}

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

    private void Start()
    {
        qualityDropdown.value = GameController.GetGameController().qualityValue;
    }

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
        GameController.GetGameController().qualityValue = qualityDropdown.value;
    }
}

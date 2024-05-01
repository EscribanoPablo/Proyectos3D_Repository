using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown qualityDropdown;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }
}

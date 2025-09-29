using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSounds : MonoBehaviour
{
    public void ButtonPressedSound()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().pressingButtonSounds);
    }

    public void MovedToOtherButtonSound()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().movingInButtonsSound);
    }
}

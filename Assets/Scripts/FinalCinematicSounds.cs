using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCinematicSounds : MonoBehaviour
{
    private void FallSounds()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().FallingToGroundSound);
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().JumpSound);
    }

    private void JumpSound()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().JumpSound);
    }

    private void ClapsSounds()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().levelEndedSound);
    }

    private void GoToCredits()
    {
        GameObject.FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.Credits);
    }
}

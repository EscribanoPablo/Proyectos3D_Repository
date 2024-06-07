using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedSound : MonoBehaviour, IRestartLevelElement
{
    private bool eventTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!eventTriggered)
        {
            FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().levelEndedSound, transform.position);

            eventTriggered = true;
        }
    }

    public void Restart()
    {
        eventTriggered = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedSound : MonoBehaviour, IRestartLevelElement
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().levelEndedSound, transform.position);

        FindObjectOfType<BoxCollider>().enabled = false;
    }

    public void Restart()
    {
        FindObjectOfType<BoxCollider>().enabled = true;
    }
}

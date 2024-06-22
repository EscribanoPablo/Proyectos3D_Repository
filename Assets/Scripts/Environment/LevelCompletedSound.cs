using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedSound : MonoBehaviour, IRestartLevelElement
{
    private bool eventTriggered = false;

    private void Start()
    {
        GameController.GetGameController().AddRestartLevelElement(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!eventTriggered)
        {
            FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().levelEndedSound);
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds);

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "BetaLevel01")
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().FirstStageEndSound);
            else
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().SecondStageEndSound);

            eventTriggered = true;
        }
    }

    public void Restart()
    {
        eventTriggered = false;
    }
}

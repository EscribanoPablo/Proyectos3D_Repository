using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayTransition : MonoBehaviour
{
    Animator transitionAnimator;
    SceneToGo sceneToGo;

    private void Start()
    {
        transitionAnimator = GetComponent<Animator>();
        GoTransparent();
    }

    private void ChangeScene()
    {
        switch (sceneToGo)
        {
            case SceneToGo.MainMenu:
                GameController.GetGameController().EmptyRestartList();
                if (SceneManager.GetActiveScene().name == "BetaLevel01" || SceneManager.GetActiveScene().name == "BetaLevel02")
                {
                    FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
                    FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
                    FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceMenuSong);
                }
                SceneManager.LoadScene("MainMenu");
                break;
            case SceneToGo.Settings:
                SceneManager.LoadScene("SettingsMenu");
                break;
            case SceneToGo.TutorialLevel:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("TutorialLevel");
                break;
            case SceneToGo.Level01:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("BetaLevel01");

                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceMenuSong);
                FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
                break;
            case SceneToGo.Level02:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("BetaLevel02");
                break;
            case SceneToGo.Credits:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("CreditsScene");

                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
                FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceMenuSong);
                break;
            default:
                break;
        }

    }

    public void GoBlack(bool fromMenu, SceneToGo nextScene)
    {
        transitionAnimator.SetBool("MenuTransition", fromMenu);
        transitionAnimator.SetTrigger("GoBlack");
        sceneToGo = nextScene;
        FindObjectOfType<EventSystem>().enabled = false;

        foreach (StudioEventEmitter eventEmitter in FindObjectsOfType<StudioEventEmitter>())
        {
            eventEmitter.Stop(); eventEmitter.PlayEvent = EmitterGameEvent.None;
        }
    }

    public void GoTransparent()
    {
        transitionAnimator.SetTrigger("GoTransparent");
    }
}

public enum SceneToGo
{
    MainMenu,
    Settings,
    TutorialLevel,
    Level01,
    Level02,
    Credits,
}

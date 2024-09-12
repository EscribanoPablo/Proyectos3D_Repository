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
                if (SceneManager.GetActiveScene().name == "BetaLevel01_Cat" || SceneManager.GetActiveScene().name == "BetaLevel02_Cat")
                {
                    FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
                    FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
                    FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceMenuSong);
                }
                SceneManager.LoadScene("MainMenu_Cat");
                break;
            case SceneToGo.Settings:
                SceneManager.LoadScene("SettingsMenu_Cat");
                break;
            case SceneToGo.TutorialLevel:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("TutorialLevel_Cat");
                FindObjectOfType<AudioManager>().PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceTutorialWelcomeSound);
                break;
            case SceneToGo.Level01:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("BetaLevel01_Cat");

                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceMenuSong);
                FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
                FindObjectOfType<AudioManager>().PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceFirstStageWelcome);
                break;
            case SceneToGo.Level02:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("BetaLevel02_Cat");

                FindObjectOfType<AudioManager>().PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceSecondStageWelcome);
                break;
            case SceneToGo.FinalCinematic:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("FinalScene_Cat");

                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
                FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceFinalCinematicSong);
                break;
            case SceneToGo.Credits:
                GameController.GetGameController().EmptyRestartList();
                SceneManager.LoadScene("CreditsScene_Cat");

                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceFinalCinematicSong);
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
    FinalCinematic,
    Credits,
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTransition : MonoBehaviour
{
    /*Animation animationManager;
    [SerializeField] AnimationClip blackAnim;
    [SerializeField] AnimationClip transparentAnim;
    [SerializeField] AnimationClip inSceneTransitionAnim;
    [SerializeField] AnimationClip outSceneTransitionAnim1;
    [SerializeField] AnimationClip outSceneTransitionAnim2;*/

    Animator transitionAnimator;

    private void Start()
    {
        //animationManager = GetComponent<Animation>();
        //EnterSceneAnimation();

        transitionAnimator = GetComponent<Animator>();
        GoTransparent();
    }


    private void OnEnable()
    {
        //gameObject.GetComponent<Animator>().SetBool("ApplyTransition", true);
    }

    private void ChangeScene()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            GameController.GetGameController().EmptyRestartList();
            SceneManager.LoadScene("BetaLevel01");

            FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceMenuSong);
            FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
        }
        else if (SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            GameController.GetGameController().EmptyRestartList();
            SceneManager.LoadScene("MainMenu");
        }
        else if (SceneManager.GetActiveScene().name == "BetaLevel01")
        {
            GameController.GetGameController().EmptyRestartList();
            SceneManager.LoadScene("BetaLevel02");
        }
        else if (SceneManager.GetActiveScene().name == "BetaLevel02")
        {
            GameController.GetGameController().EmptyRestartList();
            SceneManager.LoadScene("CreditsScene");

            FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
            FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
            FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceMenuSong);
        }
        else if(SceneManager.GetActiveScene().name == "CreditsScene")
        {
            GameController.GetGameController().EmptyRestartList();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GoBlack(bool fromMenu)
    {
        transitionAnimator.SetBool("MenuTransition", fromMenu);
        transitionAnimator.SetTrigger("GoBlack");
    }

    public void GoTransparent()
    {
        transitionAnimator.SetTrigger("GoTransparent");
    }

    /*private void SetBlack()
    {
        animationManager.Play(blackAnim.name);
    }

    private void SetTransparent()
    {
        animationManager.Play(transparentAnim.name);
    }

    public void EnterSceneAnimation()
    {
        animationManager.Play(inSceneTransitionAnim.name);
    }

    public void QuitSceneAnimation1()
    {
        animationManager.Play(outSceneTransitionAnim1.name);
    }
    public void QuitSceneAnimation2()
    {
        animationManager.Play(outSceneTransitionAnim2.name);
    }*/
}

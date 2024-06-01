using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTransition : MonoBehaviour
{
    Animation animationManager;
    [SerializeField] AnimationClip blackAnim;
    [SerializeField] AnimationClip transparentAnim;
    [SerializeField] AnimationClip inSceneTransitionAnim;
    [SerializeField] AnimationClip outSceneTransitionAnim;

    private void Start()
    {
        animationManager = GetComponent<Animation>();
        EnterSceneAnimation();
    }


    private void OnEnable()
    {
        //gameObject.GetComponent<Animator>().SetBool("ApplyTransition", true);
    }

    private void ChangeScene()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("BetaLevel01");
        }
        else if (SceneManager.GetActiveScene().name == "BetaLevel01")
        {
            SceneManager.LoadScene("BetaLevel02");
        }
    }

    private void SetBlack()
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

    public void QuitSceneAnimation()
    {
        animationManager.Play(outSceneTransitionAnim.name);
    }
}

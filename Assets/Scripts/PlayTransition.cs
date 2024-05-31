using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }


    private void OnEnable()
    {
        //gameObject.GetComponent<Animator>().SetBool("ApplyTransition", true);
    }

    private void SetBlack()
    {
        animationManager.Play(blackAnim.name);
    }

    private void SetTransparent()
    {
        animationManager.Play(transparentAnim.name);
    }

    private void EnterSceneAnimation()
    {
        animationManager.Play(inSceneTransitionAnim.name);
    }

    private void QuitSceneAnimation()
    {
        animationManager.Play(outSceneTransitionAnim.name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndWarningAnimation : MonoBehaviour
{
    [SerializeField]
    private Animation controllerAnimator;
    [SerializeField]
    private AnimationClip warningAnimation;

    private void Start()
    {
        if (GameController.GetGameController().firstTimeInMenu)
        {
            controllerAnimator.Play(warningAnimation.name);
            GameController.GetGameController().firstTimeInMenu = false;
            FindObjectOfType<EventSystem>().enabled = false;
        }
        else
            this.gameObject.SetActive(false);
    }

    private void EnableUsingMenu()
    {
        FindObjectOfType<EventSystem>().enabled = true;
    }
}

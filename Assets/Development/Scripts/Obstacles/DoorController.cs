using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    Animation doorAnimations;
    [SerializeField]
    AnimationClip doorOpenAnimation;

    public void OpenDoor()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().buttonSpinSound, transform.position);
        doorAnimations.Play(doorOpenAnimation.name);
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().doorOpenSound, transform.position);
    }
}

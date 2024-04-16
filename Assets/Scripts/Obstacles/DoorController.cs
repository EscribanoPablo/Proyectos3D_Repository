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
        doorAnimations.Play(doorOpenAnimation.name);
    }
}

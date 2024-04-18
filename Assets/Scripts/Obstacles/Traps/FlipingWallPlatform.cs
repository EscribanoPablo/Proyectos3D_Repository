using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipingWallPlatform : Traps
{
    [SerializeField]
    Animation flipWallPlatAnimations;
    [SerializeField]
    AnimationClip flipingAnimation;
    [SerializeField]
    AnimationClip flipingAnimationInvested;

    bool investAnim = false;
    [SerializeField]
    float timeToFlip = 3.0f;
    float timeCounter = 0;

    void Update()
    {
        if (timeCounter >= timeToFlip)
        {
            FlipPlatform();
            timeCounter = 0.0f;
        }
        else
            timeCounter += 1.0f * Time.deltaTime;
    }

    void FlipPlatform()
    {
        if (!investAnim)
        {
            flipWallPlatAnimations.CrossFadeQueued(flipingAnimation.name, 0.1f);
            investAnim = true;
        }
        else
        {
            flipWallPlatAnimations.CrossFadeQueued(flipingAnimationInvested.name, 0.1f);
            investAnim = false;
        }
    }
}

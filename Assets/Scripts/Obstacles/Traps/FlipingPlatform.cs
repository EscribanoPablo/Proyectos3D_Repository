using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipingPlatform : Traps
{
    [SerializeField]
    Animation flipPlatAnimations;
    [SerializeField]
    AnimationClip flipingAnimation;
    [SerializeField]
    AnimationClip flipingAnimationInvested;

    PlayerMovement player;
    bool investAnim = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        //if (player.GetIfPlayerJumped())
        //    FlipPlatform();
    }

    void FlipPlatform()
    {
        if (!investAnim)
        {
            flipPlatAnimations.CrossFadeQueued(flipingAnimation.name, 0.1f);
            investAnim = true;
        }
        else
        {
            flipPlatAnimations.CrossFadeQueued(flipingAnimationInvested.name, 0.1f);
            investAnim = false;
        }
    }
}

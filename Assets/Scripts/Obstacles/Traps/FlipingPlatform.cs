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

    bool investAnim = false;

    void Update()
    {
        //mirar si el jugador ha saltado, o que el jugador llame a la funcion
    }

    void FlipPlatform()
    {
        if (!investAnim)
        {
            flipPlatAnimations.Play(flipingAnimation.name);
            investAnim = true;
        }
        else
        {
            flipPlatAnimations.Play(flipingAnimationInvested.name);
            investAnim = false;
        }
    }
}

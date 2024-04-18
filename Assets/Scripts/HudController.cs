using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField]
    Animation collectiblesAnimation;
    [SerializeField]
    AnimationClip collectiblesAnimate;

    [SerializeField]
    List<GameObject> collectibles;

    int collectiblesTaken = 0;

    public void CollectibleTaken()
    {
        collectiblesAnimation.Play(collectiblesAnimate.name);
        collectibles[collectiblesTaken].SetActive(true);
        collectiblesTaken++;
    }
}

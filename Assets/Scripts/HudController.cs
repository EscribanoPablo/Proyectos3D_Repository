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
    [SerializeField]
    List<GameObject> lifes;

    int collectiblesTaken = 0;

    public void CollectibleTaken()
    {
        collectiblesAnimation.Play(collectiblesAnimate.name);
        collectibles[collectiblesTaken].SetActive(true);
        collectiblesTaken++;
    }

    public void LifeLost(int lifesNum)
    {
        lifes[lifesNum].SetActive(false);
    }

    public void RestartLifes()
    {
        foreach(GameObject life in lifes)
        {
            life.SetActive(true);
        }
    }

    

}

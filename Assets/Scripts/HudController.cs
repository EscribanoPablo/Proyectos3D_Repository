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
    List<GameObject> collectiblesGO;

    [SerializeField]
    List<GameObject> lifes;

    public List<bool> GetCollectiblesTaken() 
    {
        List<bool> collectiblesStatus = new List<bool>();

        for (int index = 0; index < collectibles.Count; index++)
        {
            if (collectiblesGO[index].activeSelf)
                collectiblesStatus.Add(false);
            else
                collectiblesStatus.Add(true);
        }

        return collectiblesStatus; 
    }
    int collectiblesTaken = 0;

    private void Start()
    {
        for (int index = 0; index < collectibles.Count; index++)
        {
            if (FindObjectOfType<LevelsController>().CollectiblesCount()[index]) 
            { 
                collectibles[index].SetActive(true);
                collectiblesGO[index].SetActive(false);

                collectiblesTaken++;
            }
        }
    }

    public void CollectibleTaken(int collectibleIndex)
    {
        collectiblesAnimation.Play(collectiblesAnimate.name);
        collectibles[collectibleIndex].SetActive(true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNextArea : MonoBehaviour
{
    private BoxCollider pathCollider;

    [SerializeField] private Animation pathAnimations;
    [SerializeField] private AnimationClip openPathAnimation;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void EnableNewArea()
    {
        pathAnimations.Play(openPathAnimation.name);
        GetComponent<BoxCollider>().enabled = false;
    }
}

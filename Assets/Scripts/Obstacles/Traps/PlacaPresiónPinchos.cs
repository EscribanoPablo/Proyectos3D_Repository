using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaPresiónPinchos : Traps
{
    [SerializeField] private float damage;
    [SerializeField] private float timeBetweenChangeState;
    private float timer;

    [SerializeField] private Animation spikesAnimations;
    [SerializeField] private AnimationClip spikesUp;
    [SerializeField] private AnimationClip spikesDown;
    private bool spikesHidden;

    // Start is called before the first frame update
    void Start()
    {
        spikesHidden = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenChangeState)
        {
            timer = 0;
            ChangeState();
        }
    }

    private void DoDamage()
    {
        //player gets damaged
    }

    private void ChangeState()
    {
        if (spikesHidden)
        {
            spikesAnimations.CrossFade(spikesUp.name);
        }
        else
        {
            spikesAnimations.CrossFade(spikesDown.name);
        }

        spikesHidden = !spikesHidden;

        // (de)activate hit collider?
    }
}

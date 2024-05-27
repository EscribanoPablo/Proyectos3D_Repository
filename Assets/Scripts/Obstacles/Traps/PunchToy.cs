using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchToy : Traps
{
    [SerializeField] private float timeOnPunch;
    [SerializeField] private float timeToRest;
    private float timer;

    [SerializeField] private Animation punchAnimations;
    [SerializeField] private AnimationClip punchHit;
    [SerializeField] private AnimationClip punchReturn;
    private bool punchIdle;


    // Start is called before the first frame update
    void Start()
    {
        punchIdle = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (punchIdle)
        {
            if (timer >= timeToRest)
            {
                timer = 0;
                ChangeState();
            }
        }
        else
        {
            if (timer >= timeOnPunch)
            {
                timer = 0;
                ChangeState();
            }
        }

    }

    private void ChangeState()
    {
        if (punchIdle)
        {
            punchAnimations.CrossFade(punchHit.name);
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().punchTrapSound, transform.position);
        }
        else
        {
            punchAnimations.CrossFade(punchReturn.name);
        }

        punchIdle = !punchIdle;

    }

}

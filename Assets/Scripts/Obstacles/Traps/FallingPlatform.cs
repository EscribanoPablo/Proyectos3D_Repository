using System;
using System.Collections;
using UnityEngine;

public class FallingPlatform : Traps
{
    [SerializeField] private float timeToVanish;
    [SerializeField] private float timeToReappear;
    private float timerVanished;

    [SerializeField] private MeshRenderer platformRenderer;
    [SerializeField] private Collider platformCollider;

    [SerializeField] private Animation animations;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip vibrateAnimation;
    [SerializeField] private AnimationClip fallAnimation;

    private bool playerTouched = false;
    private bool disappearing = false;

    private float minDotToOnPlatform = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (playerTouched)
        {
            timerVanished += Time.deltaTime;
            if (timerVanished >= timeToVanish)
            {
                ObjectDisappearAnimation();
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == PLAYER_TAG && !disappearing)
        {
            if (PlayerOnPlatform(collision))
            {
                playerTouched = true;
                animations.PlayQueued(vibrateAnimation.name);
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().vibrateFallPlatformSound, transform.position);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG && !disappearing)
        {
            if (PlayerOnPlatform(collision))
            {
                playerTouched = true;
                animations.Play(vibrateAnimation.name);
            }
        }
    }

    private bool PlayerOnPlatform(Collision player)
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.Normalize();
        return Vector3.Dot(Vector3.up, directionToPlayer) > minDotToOnPlatform;
    }



    private void ObjectDisappearAnimation()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().fallPlatformSound, transform.position);
        animations.Play(fallAnimation.name);
        playerTouched = false;
        disappearing = true;
    }

    public void ObjectDisappear()
    {
        timerVanished = 0;
        platformRenderer.enabled = false;
        platformCollider.enabled = false;
        disappearing = false;

        StartCoroutine(ObjectReappear());
    }



    IEnumerator ObjectReappear()
    {
        yield return new WaitForSeconds(timeToReappear);
        platformRenderer.enabled = true;
        platformCollider.enabled = true;
        animations.Play(idleAnimation.name);
    }
}

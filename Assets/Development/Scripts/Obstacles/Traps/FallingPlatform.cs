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

    [Header("Anti-Crush")]
    [SerializeField] FallingPlatformSafety safety; // referencia al script del padre

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
        if(collision.gameObject.tag == PLAYER_TAG && !disappearing && !playerTouched)
        {
            if (PlayerOnPlatform(collision))
            {
                playerTouched = true;
                animations.Play(vibrateAnimation.name);
                FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().vibrateFallPlatformSound, transform.position);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == PLAYER_TAG && !disappearing && !playerTouched)
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
        // Por si quedó en trigger al final de la animación
        if (platformCollider) platformCollider.isTrigger = true;

        platformRenderer.enabled = false;
        platformCollider.enabled = false;
        disappearing = false;

        StartCoroutine(ObjectReappear());
    }



    IEnumerator ObjectReappear()
    {
        yield return new WaitForSeconds(timeToReappear);

        // Restablecer estado para el siguiente ciclo
        if (platformCollider)
        {
            platformCollider.enabled = true;
            platformCollider.isTrigger = false;
        }
        if (safety) safety.SetActive(false);

        platformRenderer.enabled = true;
        animations.Play(idleAnimation.name);
    }


    // === LLAMADOS POR ANIMATION EVENTS ===
    // Se activa cuando empieza a caer
    //public void OnFallStart()
    //{
    //    if (safety) safety.SetActive(true);
    //    // Aseguramos que el collider principal sigue siendo sólido
    //    if (platformCollider) platformCollider.isTrigger = false;
    //}

    //// Se llama justo antes de que desaparezca (último tramo)
    //public void OnFallEnd()
    //{
    //    if (platformCollider) platformCollider.isTrigger = true; // ya no puede aplastar
    //    if (safety) safety.SetActive(false);
    //}
}

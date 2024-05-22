using System;
using System.Collections;
using UnityEngine;

public class FallingPlatform : Traps
{
    [SerializeField] private float timeToVanish;
    [SerializeField] private float timeToReappear;
    private float timerVanished;

    private Renderer renderer;
    private Collider collider;

    private bool playerTouched = false;

    private float minDotToOnPlatform = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTouched)
        {
            timerVanished += Time.deltaTime;
            if (timerVanished >= timeToVanish)
            {
                ObjectDisappear();
                StartCoroutine(ObjectReappear());
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == PLAYER_TAG)
        {
            if (PlayerOnPlatform(collision))
            {
                playerTouched = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
        {
            if (PlayerOnPlatform(collision))
            {
                playerTouched = true;
            }
        }
    }

    private bool PlayerOnPlatform(Collision player)
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.Normalize();
        return Vector3.Dot(Vector3.up, directionToPlayer) > minDotToOnPlatform;
    }



    private void ObjectDisappear()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().fallPlatformSound, transform.position);
        timerVanished = 0;
        playerTouched = false;
        renderer.enabled = false;
        collider.enabled = false;
    }

    IEnumerator ObjectReappear()
    {
        yield return new WaitForSeconds(timeToReappear);
        renderer.enabled = true;
        collider.enabled = true;
    }
}

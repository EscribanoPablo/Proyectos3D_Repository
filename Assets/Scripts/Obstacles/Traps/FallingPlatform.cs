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
            playerTouched = true;
        }
    }
    private void ObjectDisappear()
    {
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

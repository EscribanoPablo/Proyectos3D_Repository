using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpikes : Traps
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG)
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(GetComponentInParent<Transform>().position);
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().rotatorySpikesHitSound, 0.5f, transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG)
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(GetComponentInParent<Transform>().position);
        }
    }
}

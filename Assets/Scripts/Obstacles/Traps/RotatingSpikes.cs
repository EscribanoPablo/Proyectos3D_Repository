using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSpikes : Traps
{

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == PLAYER_TAG)
    //    {
    //        collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(GetComponentInParent<Transform>().position - new Vector3(0, 1.0f, 0));
    //    }
    //}

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
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().rotatorySpikesHitSound, 0.5f, transform.position);
        }
    }
}

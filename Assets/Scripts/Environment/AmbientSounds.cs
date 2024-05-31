using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds[Random.Range(0, FindObjectOfType<AudioManager>().ambientClapsSounds.Count)]);
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientLaughtsSounds[Random.Range(0, FindObjectOfType<AudioManager>().ambientLaughtsSounds.Count)]);

        FindObjectOfType<BoxCollider>().enabled = false;
    }
}

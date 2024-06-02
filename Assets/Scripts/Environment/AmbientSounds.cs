using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds);
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientLaughtsSounds);

        FindObjectOfType<BoxCollider>().enabled = false;
    }
}

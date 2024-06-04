using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(Random.Range(0, 4) == 0)
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds);

        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientLaughtsSounds);

        FindObjectOfType<BoxCollider>().enabled = false;
    }
}

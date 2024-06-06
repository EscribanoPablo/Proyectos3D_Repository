using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour, IRestartLevelElement
{
    private void OnTriggerEnter(Collider other)
    {
        if(Random.Range(0, 3) == 0)
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientClapsSounds);

        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ambientLaughtsSounds);

        FindObjectOfType<BoxCollider>().enabled = false;
    }

    public void Restart()
    {
        FindObjectOfType<BoxCollider>().enabled = true;
    }
}

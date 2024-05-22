using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool checkpointGrabbed = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" && !checkpointGrabbed)
        {
            other.GetComponent<PlayerController>().SetRespawnPos(gameObject.transform);
            checkpointGrabbed = true;
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().ActivateCheckPointSound, transform.position);
        }
    }
}
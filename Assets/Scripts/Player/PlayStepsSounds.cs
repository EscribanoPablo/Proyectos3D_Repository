using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStepsSounds : MonoBehaviour
{
    public void PlaySteps()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().RecieveDamageSound[Random.Range(0, FindObjectOfType<AudioManager>().RecieveDamageSound.Count)], transform.position);
    }
}

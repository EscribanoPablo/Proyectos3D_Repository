using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStepsSounds : MonoBehaviour
{
    private void PlayStepsSound()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().runSounds, transform.position);
    }
}

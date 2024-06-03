using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStepsSounds : MonoBehaviour
{
    private void OnEnable()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().runSounds, transform.position);
    }
}

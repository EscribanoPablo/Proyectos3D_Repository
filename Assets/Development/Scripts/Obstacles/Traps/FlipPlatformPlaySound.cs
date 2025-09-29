using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPlatformPlaySound : MonoBehaviour
{
    private void OnEnable()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().flipPlatformSound, transform.position);
    }
}

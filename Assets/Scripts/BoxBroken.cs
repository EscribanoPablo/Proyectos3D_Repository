using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBroken : MonoBehaviour
{
    private void OnEnable()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().breakingBoxSound, transform.position);
    }
}

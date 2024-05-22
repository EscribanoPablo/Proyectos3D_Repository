using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTransition : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<Animator>().SetBool("ApplyTransition", true);
    }
}

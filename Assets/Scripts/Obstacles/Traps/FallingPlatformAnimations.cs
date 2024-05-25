using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformAnimationEvents : MonoBehaviour
{
    private FallingPlatform fallingPlatform;
    // Start is called before the first frame update
    void Start()
    {
        fallingPlatform = transform.Find("platformCollider").GetComponent<FallingPlatform>();
    }

    private void ObjectDisappear()
    {
        fallingPlatform.ObjectDisappear();
    }
}

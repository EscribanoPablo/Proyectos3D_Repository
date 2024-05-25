using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformAnimations : MonoBehaviour
{
    private FallingPlatform fallingPlatform;
    // Start is called before the first frame update
    void Start()
    {
        fallingPlatform = transform.Find("platformCollider").GetComponent<FallingPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ObjectDisappear()
    {
        fallingPlatform.ObjectDisappear();
    }
}

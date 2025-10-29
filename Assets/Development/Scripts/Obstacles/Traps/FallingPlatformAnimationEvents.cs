using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformAnimations : MonoBehaviour
{
    private FallingPlatform fallingPlatform;
    [SerializeField] FallingPlatformSafety safety;   // componente del raíz
    [SerializeField] Collider platformCollider;      // el collider SÓLIDO (NO el trigger)

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

    public void OnFallStart()
    {
        if (safety) safety.SetActive(true);
        if (platformCollider) platformCollider.isTrigger = false;
    }

    public void OnFallEnd()
    {
        if (platformCollider) platformCollider.isTrigger = true;
        if (safety) safety.SetActive(false);
    }

}

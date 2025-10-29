using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformSkirt : MonoBehaviour
{
    FallingPlatformSafety safety;
    void Awake() => safety = GetComponentInParent<FallingPlatformSafety>();

    void OnTriggerStay(Collider other)
    {
        if (safety != null) safety.HandlePlayerInDanger(other);
    }
}

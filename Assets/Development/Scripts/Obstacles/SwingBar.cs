using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingBar : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody barRigidbody;   // Rigidbody del GameObject padre
    [SerializeField] private Collider grabCollider;    // Este collider (trigger) que define la zona de agarre
    [SerializeField] private Transform pivot;
    public Rigidbody BarRigidbody => barRigidbody;
    public Transform Pivot => pivot;

    private void Reset()
    {
        grabCollider = GetComponent<Collider>();
        if (grabCollider != null)
            grabCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerSwing swing = other.GetComponent<PlayerSwing>();
        if (swing == null)
            return;

        Vector3 grabPoint = grabCollider.ClosestPoint(other.transform.position);
        swing.AttachToBar(this, grabPoint);
    }
}

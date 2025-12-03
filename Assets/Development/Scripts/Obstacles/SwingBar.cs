using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingBar : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody barRigidbody;   // Rigidbody del GameObject padre
    [SerializeField] private Collider grabCollider;    // Este collider (trigger) que define la zona de agarre
    [SerializeField] private Transform pivot;
    private void Reset()
    {
        grabCollider = GetComponent<Collider>();
        if (grabCollider != null)
            grabCollider.isTrigger = true;
    }
    public Rigidbody BarRigidbody => barRigidbody;
    public Collider GrabCollider => grabCollider;
    public Transform Pivot => pivot;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var swing = other.GetComponent<PlayerSwing>();
        if (swing == null)
            return;

        // Punto más cercano de esta zona de agarre al jugador
        Vector3 grabPoint = grabCollider.ClosestPoint(other.transform.position);
        swing.AttachToBar(this, grabPoint);
    }
}

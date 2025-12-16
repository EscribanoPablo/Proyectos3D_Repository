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

    private void Awake()
    {
        //IMPORTANTE: buscar el Hinge en el PADRE
        HingeJoint hinge = GetComponentInParent<HingeJoint>();
        if (!hinge)
        {
            Debug.LogError("SwingBar: No se encontró HingeJoint en el padre");
            return;
        }

        hinge.useSpring = true;

        JointSpring spring = hinge.spring;
        spring.spring = 80f;        // fuerza de retorno
        spring.damper = 8f;         // amortiguación
        spring.targetPosition = 0f; // neutro EXACTO
        hinge.spring = spring;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerSwing swing = other.GetComponent<PlayerSwing>();
        if (!swing)
            return;

        Vector3 grabPoint = grabCollider.ClosestPoint(other.transform.position);
        swing.AttachToBar(this, grabPoint);
    }
}

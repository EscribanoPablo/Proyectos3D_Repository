using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float bounceVelocity = 12f;
    [SerializeField] private string playerTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag(playerTag))
            return;

        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb == null)
            return;

        // Limpia solo la componente vertical para que el rebote sea consistente
        Vector3 v = rb.velocity;
        v.y = 0f;
        rb.velocity = v;

        rb.AddForce(Vector3.up * bounceVelocity, ForceMode.VelocityChange);
    }
}

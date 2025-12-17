using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float baseBounceVelocity = 10f;   // X
    [SerializeField] private float bounceMultiplier = 1.25f;  // incremento por rebote
    [SerializeField] private int maxConsecutiveBounces = 3;

    [Header("Detection")]
    [SerializeField] private string playerTag = "Player";

    private int consecutiveBounces = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag(playerTag))
            return;

        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb == null)
            return;

        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        consecutiveBounces++;
        consecutiveBounces = Mathf.Clamp(consecutiveBounces, 1, maxConsecutiveBounces);

        float bounceVelocity =
            baseBounceVelocity * Mathf.Pow(bounceMultiplier, consecutiveBounces - 1);

        // Limpiamos la velocidad vertical previa para que el rebote sea consistente
        Vector3 velocity = rb.velocity;
        velocity.y = 0f;
        rb.velocity = velocity;

        rb.AddForce(Vector3.up * bounceVelocity, ForceMode.VelocityChange);
        player.RegisterTrampoline(this);
        player.OnTrampolineBounce();
    }

    public void ResetBounces()
    {
        consecutiveBounces = 0;
    }
}

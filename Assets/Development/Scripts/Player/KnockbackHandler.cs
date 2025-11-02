using UnityEngine;

public class KnockbackHandler : MonoBehaviour
{
    [SerializeField] private float baseKnockbackImpulse = 0.0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyKnockback(Vector3 pointOfImpact, float extraImpulse = 0f)
    {
        PlayerMovement pm = GetComponent<PlayerMovement>();
        Vector3 knockbackDirection = transform.position - pointOfImpact;
        float impulse = baseKnockbackImpulse + extraImpulse;
        float baseImpulseAdded = impulse;
        if (pm.GetIfGrounded() && !pm.GetIsJumping() && !pm.GetIfDashing())
        {
            impulse *= 4; 
        }

        if (impulse >= baseImpulseAdded *4 && !pm.GetIfGrounded())
        {
             impulse = baseImpulseAdded;
        }

        knockbackDirection.y = 0.5f;
        knockbackDirection.Normalize();

        Vector3 impulseVector = knockbackDirection * impulse;

        // impulso físico inmediato
        rb.AddForce(impulseVector, ForceMode.Impulse);
        impulse = baseKnockbackImpulse;
        // notificar a PlayerMovement para preservar y mezclar momentum
        if (pm != null)
        {
            // Δv ≈ impulse / m
            Vector3 deltaV = impulseVector / Mathf.Max(0.0001f, rb.mass);
            deltaV.y = 0f;
            pm.AddExternalVelocity(deltaV);
        }
    }
}


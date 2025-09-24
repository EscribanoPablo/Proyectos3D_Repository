using UnityEngine;

public class KnockbackHandler : MonoBehaviour
{
    [SerializeField] private float baseKnockbackImpulse = 7.5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyKnockback(Vector3 pointOfImpact, float extraImpulse = 0f)
    {
        Vector3 knockbackDirection = transform.position - pointOfImpact;
        float impulse = baseKnockbackImpulse + extraImpulse;

        knockbackDirection.y = 0.5f;
        knockbackDirection.Normalize();

        rb.AddForce(knockbackDirection * impulse, ForceMode.Impulse);
    }
}


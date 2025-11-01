using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformSafety : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Collider solidPlatformCollider; // tu platformCollider (NO trigger)
    [SerializeField] LayerMask groundMask = ~0;      // capa(s) de suelo

    [Header("Empuje anti-aplastamiento")]
    [SerializeField] float sidePush = 12f;
    [SerializeField] float upNudge = 2.5f;
    [SerializeField] float minDownSpeedToCrush = 0.6f;

    Vector3 lastPos;
    float downSpeed; // >0 si baja
    Rigidbody rb;
    bool active;     // solo durante la caída

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        lastPos = transform.position;
    }

    void OnEnable() { lastPos = transform.position; }

    void FixedUpdate()
    {
        float dy = transform.position.y - lastPos.y;
        downSpeed = -dy / Time.fixedDeltaTime; // positivo si baja
        lastPos = transform.position;
    }

    public void SetActive(bool value) => active = value;

    // Llamado por el trigger hijo
    public void HandlePlayerInDanger(Collider playerCol)
    {
        if (!active || downSpeed < minDownSpeedToCrush) return;
        if (!playerCol.CompareTag("Player")) return;

        var prb = playerCol.attachedRigidbody;
        if (prb == null) return;

        // Empuje lateral + pequeño up hacia el borde más cercano
        Vector3 toPlayer = prb.worldCenterOfMass - rb.worldCenterOfMass;
        toPlayer.y = 0f;
        Vector3 sideDir = toPlayer.sqrMagnitude > 1e-4f ? toPlayer.normalized : transform.right;
        prb.AddForce(sideDir * sidePush + Vector3.up * upNudge, ForceMode.VelocityChange);

        // Si está sándwich contra el suelo, ignora colisión 1–2 frames
        if (IsTouchingGround(playerCol))
            StartCoroutine(TempIgnoreCollision(playerCol, 2));
    }

    bool IsTouchingGround(Collider playerCol)
    {
        var c = playerCol.bounds.center;
        float dist = playerCol.bounds.extents.y + 0.08f;
        return Physics.Raycast(c, Vector3.down, dist, groundMask, QueryTriggerInteraction.Ignore);
    }

    IEnumerator TempIgnoreCollision(Collider playerCol, int fixedFrames)
    {
        if (solidPlatformCollider == null) yield break;

        Physics.IgnoreCollision(solidPlatformCollider, playerCol, true);
        var prb = playerCol.attachedRigidbody;
        if (prb) prb.velocity = new Vector3(prb.velocity.x, Mathf.Max(prb.velocity.y, 0f), prb.velocity.z);

        for (int i = 0; i < fixedFrames; i++) yield return new WaitForFixedUpdate();

        Physics.IgnoreCollision(solidPlatformCollider, playerCol, false);
    }
}

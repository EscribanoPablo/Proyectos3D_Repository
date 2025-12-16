using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerSwing : MonoBehaviour
{
    [SerializeField] float swingForce = 40f;
    [SerializeField] float maxSwingSpeed = 20f;
    [SerializeField] float baseLaunchBoost = 6f;
    [SerializeField] AnimationCurve extraBoostBySpeed;
    [SerializeField] float impulseScale = 1f;
    [SerializeField] float attachAngularScale = 1f;
    [SerializeField] float barReturnDamping = 3f;

    Rigidbody rb;
    PlayerInput input;

    HingeJoint hinge;
    SwingBar currentBar;
    bool isSwinging;

    RigidbodyConstraints originalConstraints;

    public bool IsSwinging => isSwinging;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
    }

    public void AttachToBar(SwingBar bar, Vector3 grabPointWorld)
    {
        if (isSwinging || bar == null) return;

        currentBar = bar;
        Vector3 incomingVel = rb.velocity;

        // ===== GUARDAR Y BLOQUEAR ROTACIONES =====
        originalConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // ===== CALCULAR PIVOTE =====
        Vector3 pivotPos = bar.Pivot
            ? bar.Pivot.position
            : bar.BarRigidbody.transform.position;

        // ===== ORIENTAR JUGADOR MIRANDO AL BALANCÍN =====
        Vector3 toPivot = pivotPos - transform.position;
        toPivot.y = 0f;

        if (toPivot.sqrMagnitude > 1e-6f)
        {
            rb.MoveRotation(Quaternion.LookRotation(toPivot.normalized, Vector3.up));
        }

        // ===== CREAR HINGE =====
        hinge = gameObject.AddComponent<HingeJoint>();
        hinge.connectedBody = bar.BarRigidbody;
        hinge.autoConfigureConnectedAnchor = false;

        hinge.anchor = transform.InverseTransformPoint(grabPointWorld);
        hinge.connectedAnchor =
            bar.BarRigidbody.transform.InverseTransformPoint(grabPointWorld);

        Vector3 axisWorld = bar.BarRigidbody.transform.right;
        hinge.axis = transform.InverseTransformDirection(axisWorld);

        hinge.useLimits = false;
        hinge.enableCollision = false;

        isSwinging = true;

        // ===== TRANSFERENCIA DE INERCIA =====
        Vector3 r = grabPointWorld - pivotPos;
        if (r.sqrMagnitude > 1e-6f)
        {
            Vector3 tangentialVel =
                incomingVel - Vector3.Project(incomingVel, r.normalized);

            Vector3 impulse = tangentialVel * rb.mass * impulseScale;
            bar.BarRigidbody.AddForceAtPosition(
                impulse,
                grabPointWorld,
                ForceMode.Impulse
            );

            Vector3 omega =
                Vector3.Cross(r, tangentialVel) / r.sqrMagnitude;

            float omegaAxis = Vector3.Dot(omega, axisWorld);
            bar.BarRigidbody.angularVelocity +=
                axisWorld * (omegaAxis * attachAngularScale);
        }

        rb.velocity = bar.BarRigidbody.GetPointVelocity(transform.position);
    }

    public void DetachFromBar(bool withLaunch)
    {
        if (!isSwinging) return;

        Vector3 launchVelocity = rb.velocity;

        if (withLaunch)
        {
            float speed = launchVelocity.magnitude;
            float extra = baseLaunchBoost;

            if (extraBoostBySpeed != null && extraBoostBySpeed.keys.Length > 0)
                extra += extraBoostBySpeed.Evaluate(speed);

            launchVelocity = launchVelocity.normalized * (speed + extra);
        }

        // ===== LIMPIAR HINGE =====
        Destroy(hinge);
        hinge = null;
        currentBar = null;
        isSwinging = false;

        // ===== RESTAURAR ROTACIONES =====
        rb.constraints = originalConstraints;

        // Forzar upright (elimina inclinaciones residuales)
        Vector3 euler = rb.rotation.eulerAngles;
        rb.rotation = Quaternion.Euler(0f, euler.y, 0f);

        rb.velocity = launchVelocity;
    }

    public void HandleSwingUpdate()
    {
        if (!isSwinging || hinge == null) return;

        Vector2 inputMove = input.actions["Movement"].ReadValue<Vector2>();
        float horizontal = inputMove.x;

        if (Mathf.Abs(horizontal) > 0.01f)
        {
            Vector3 pivotPos =
                currentBar.BarRigidbody.transform.TransformPoint(hinge.connectedAnchor);

            Vector3 radius = transform.position - pivotPos;

            if (radius.sqrMagnitude > 1e-6f)
            {
                Vector3 axisWorld = currentBar.BarRigidbody.transform.right;
                Vector3 tangent = Vector3.Cross(axisWorld, radius).normalized;

                rb.AddForce(
                    tangent * swingForce * horizontal,
                    ForceMode.Acceleration
                );
            }
        }

        // límite de velocidad
        Vector3 v = rb.velocity;
        Vector3 planar = new Vector3(v.x, 0f, v.z);
        if (planar.magnitude > maxSwingSpeed)
        {
            planar = planar.normalized * maxSwingSpeed;
            rb.velocity = new Vector3(planar.x, v.y, planar.z);
        }

        if (input.actions["Jump"].WasPressedThisFrame())
        {
            DetachFromBar(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerSwing : MonoBehaviour
{
    [Header("Ajustes de balancín")]
    [SerializeField] private float swingForce = 40f;      // fuerza al empujar
    [SerializeField] private float maxSwingSpeed = 20f;   // límite de velocidad
    [SerializeField] private float baseLaunchBoost = 6f;  // extra de fuerza al soltar
    [SerializeField] private AnimationCurve extraBoostBySpeed;
    // opcional: velocidad -> boost extra

    private Rigidbody rb;
    private PlayerInput input;
    private PlayerMovement movement;

    private HingeJoint hinge;
    private SwingBar currentBar;
    private bool isSwinging;
    private bool jumpConsumedThisFrame;   // para evitar doble lectura

    public bool IsSwinging => isSwinging;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        movement = GetComponent<PlayerMovement>();
    }

    public void AttachToBar(SwingBar bar, Vector3 grabPointWorld)
    {
        if (isSwinging || bar == null)
            return;

        currentBar = bar;

        // 1) Guardar la velocidad de entrada del jugador ANTES del joint
        Vector3 incomingVelocity = rb.velocity;

        // 2) Crear el joint
        hinge = gameObject.AddComponent<HingeJoint>();
        hinge.connectedBody = bar.BarRigidbody;
        hinge.autoConfigureConnectedAnchor = false;

        hinge.anchor = transform.InverseTransformPoint(grabPointWorld);
        hinge.connectedAnchor = bar.BarRigidbody.transform.InverseTransformPoint(grabPointWorld);

        // Asegúrate de que este eje coincide con el eje real de giro del balancín
        hinge.axis = Vector3.right;

        hinge.useLimits = false;
        hinge.enableCollision = false;

        isSwinging = true;

        // 3) DAR EL GOLPE INICIAL AL BALANCÍN
        //    Impulso proporcional a tu velocidad de entrada.
        float impulseScale = 1.0f; // prueba 0.5, 1, 1.5, 2
        Vector3 impulse = incomingVelocity * rb.mass * impulseScale;

        bar.BarRigidbody.AddForceAtPosition(
            impulse,
            grabPointWorld,              // punto donde te agarras
            ForceMode.Impulse
        );

        // 4) Ajustar la velocidad del jugador al punto de la barra
        //    para que al enganchar no "patine" respecto al balancín.
        Vector3 barPointVel = bar.BarRigidbody.GetPointVelocity(transform.position);
        rb.velocity = barPointVel;
    }

    public void DetachFromBar(bool withLaunch)
    {
        if (!isSwinging)
            return;

        Vector3 launchVelocity = rb.velocity;

        if (withLaunch)
        {
            float speed = launchVelocity.magnitude;

            // dirección de lanzamiento: tangente al movimiento actual
            Vector3 dir = (speed > 0.1f) ? launchVelocity.normalized : transform.forward;

            float extraBoost = baseLaunchBoost;
            if (extraBoostBySpeed != null && extraBoostBySpeed.keys.Length > 0)
                extraBoost += extraBoostBySpeed.Evaluate(speed);

            launchVelocity = dir * (speed + extraBoost);
        }

        Destroy(hinge);
        hinge = null;
        currentBar = null;
        isSwinging = false;

        rb.velocity = launchVelocity;
    }

    /// <summary>
    /// Llamar desde Update del PlayerMovement mientras IsSwinging == true.
    /// </summary>
    public void HandleSwingUpdate()
    {
        if (!isSwinging || input == null)
            return;

        jumpConsumedThisFrame = false;

        // Input horizontal: usamos el eje X del movimiento
        Vector2 moveInput = input.actions["Movement"].ReadValue<Vector2>();
        float horizontal = -moveInput.x;

        // fuerza tangencial según el stick
        if (Mathf.Abs(horizontal) > 0.01f && currentBar != null)
        {
            Vector3 pivotPos = currentBar.BarRigidbody.transform.TransformPoint(hinge.connectedAnchor);
            Vector3 radius = transform.position - pivotPos;

            // plano de oscilación (aprox.): vertical (gravedad + radius)
            Vector3 planeNormal = Vector3.Cross(radius, Vector3.up);
            if (planeNormal.sqrMagnitude < 1e-4f)
                planeNormal = Vector3.Cross(radius, transform.right);

            // tangente en el arco
            Vector3 tangent = Vector3.Cross(planeNormal.normalized, radius.normalized);

            rb.AddForce(tangent * swingForce * horizontal, ForceMode.Acceleration);
        }

        // limitar velocidad
        Vector3 v = rb.velocity;
        Vector3 horizontalVel = new Vector3(v.x, 0f, v.z);
        if (horizontalVel.magnitude > maxSwingSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSwingSpeed;
            rb.velocity = new Vector3(horizontalVel.x, v.y, horizontalVel.z);
        }

        // salto para soltarse
        if (input.actions["Jump"].WasPressedThisFrame())
        {
            jumpConsumedThisFrame = true;
            DetachFromBar(withLaunch: true);
        }
    }
}

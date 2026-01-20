using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Ability
{
    public AbilityState abilityData;
    public bool canUse;
    public bool alreadyUsed;

    public void SetCanUseAbility(bool value)
    {
        canUse = value;
    }
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Mechanics/Abilities")]
    [SerializeField] private List<Ability> abilitiesList = new List<Ability>();

    public Rigidbody rigidBody { get; set; }
    [Header("References")]
    [SerializeField] private CapsuleCollider baseCollider;
    [SerializeField] private CapsuleCollider crouchingCollider;
    [SerializeField] new Camera camera;
    [SerializeField] private CinemachineVirtualCamera cameraCinemachine;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform roofChecker;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsSafeGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private GameObject cannonGO;
    [SerializeField] private CanonShoot canonShoot;

    [SerializeField] private Transform spawnBulletDoubleJumpPosition;
    [SerializeField] private Transform spawnBulletDashPosition;
    [SerializeField] private GameObject groundPoundExplosion;

    [SerializeField] private GameObject canonParticles;
    [SerializeField] private GameObject wallJumpParticles;
    [SerializeField] private GameObject jumpParticles;
    [SerializeField] private GameObject dustParticles;

    [Header("Inputs")]
    PlayerInput playerInput;
    public bool playerControllerEnabled { get; set; }

    [Header("Movement Variables")]
    [SerializeField] private float baseSpeedMovement = 40;
    [SerializeField] private float runSpeedMovement = 70;
    [SerializeField] private float crouchSpeedMovement = 20;
    private float speedMovement;
    private float cameraFOVBase;
    [SerializeField] private float cameraFOVWhenRunning;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float baseRotationSpeed = 7.5f;
    private float rotationSpeed;
    private bool isMoving;
    private bool movementBlocked = false;

    [SerializeField] private float transitionDurationStart = 0.5f;
    [SerializeField] private float transitionDurationStop = 0.5f;
    private float transitionTimer = 0f;
    private float speedAnimation = 0;

    [Header("Jumps Variables")]
    [SerializeField] private int multipleJumps = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float detectionGroundRadius = 0.5f;
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float crouchingJumpForce;
    private int currentJumps;
    private bool longJumped = false;
    [SerializeField] float gravity;
    public bool GetIsJumping() { return isJumping; }
    private bool isJumping = false;

    [Header("Wall Variables")]
    [SerializeField] private float wallDetectionDistance = 0.02f;
    [SerializeField] private float wallDetectionOffset = 0.5f;
    [SerializeField] private float timeToWallFall = 3f;
    private bool facingWall = false;
    private bool onWall = false;
    private bool canWall = true;
    private float wallTimer;
    private Vector3 lastWallNormal = Vector3.zero;
    private RaycastHit lastWallHit;
    private bool faceAwayAfterWallJump = true; // opcional: girar tras el salto
    [SerializeField] private float wallRegrabCooldown = 0.18f; // 0.15–0.25s va bien
    [SerializeField] private float minAngleBetweenWalls = 35f; // grados para evitar misma pared
    private Vector3 lastWallJumpNormal = Vector3.zero;
    [SerializeField] private float wallSlideSpeed = 1.5f;     // velocidad máx. de caída estando en pared
    [SerializeField] private float wallUpStopDamp = 12f;      // amortiguación para eliminar velocidad ascendente en pared
    private bool requireNotGroundedToLatch = true; // no entrar en pared si estás en suelo
    [SerializeField] private float minDistanceToGroundToSetOnWall = 0.6f; // altura mínima al suelo para enganchar pared
    
    [SerializeField] private float postJumpNoLatchTime = 0.12f; // 0.1–0.15 s
    private float blockWallLatchUntil = 0f;
    [Range(0,1)] [SerializeField] private float minFacingDotToSetOnWall = 0.5f;     // ≈60° respecto a la normal
    [SerializeField] private float minPlayerSpeedToSetOnWall = 0.2f;  // m/s hacia adelante
    [SerializeField] private float wallJumpMoveFreeze = 0.15f;
    private Coroutine movementUnlockRoutine = null;
    private float wallJumpCooldown = 0.15f; // 0.12–0.2 se siente bien
    private float nextWallJumpAllowedTime = 0f;
    [SerializeField] private float wallJumpInputDelay = 0.08f; // tiempo mínimo pegado antes de poder saltar
    private float wallJumpCanStartAt = 0f;
    [Header("Wall Stick / Slide")]
    [SerializeField] private float wallStickTime = 0.4f;   // tiempo clavado
    [SerializeField] private float wallDetachInputDelay = 0.5f; // tiempo antes de permitir despegarse moviendo

    private float wallStickUntil = 0f;
    private float wallDetachAllowedAt = 0f;
    //[SerializeField] private float wallAnticipationDistance = 0.6f;   // distancia para anticipar
    //[SerializeField] private float wallAnticipationAngle = 35f;       // grados máx entre forward y -normal
    //[SerializeField] private float wallAnticipationMinSpeed = 0.5f;   // mínima velocidad hacia la pared
    //[SerializeField] private float wallAnticipationSmoothing = 10f;   // rapidez del blend (mayor = más rápido)
    //private float wallApproachBlend = 0f;

    [Header("Crouching Variables")]
    private float timeCrouching = 0;
    public bool GetIfCrouching() { return isCrouching; }
    private bool isCrouching;

    [Header("Dash Variables")]
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashPower = 300;
    public bool GetIfDashing() { return isDashing; }
    private bool isDashing;
    private float multipleDashOnAir = 2;
    private float currentDashes = 0;
    [SerializeField] private float dashDrag = 0f;    // drag durante el dash
    private float previousDrag = 0.5f;              // para restaurar luego

    [Header("GroundPound Variables")]
    [SerializeField] private float groundPoundForce = 10f;
    private bool doingGroundPound = false;
    [SerializeField] private float distanceToGroundPound = 2f;


    [Header("Coyote Time Variables")]
    [Range(0.1f, 1.0f)] [SerializeField] float coyoteTimeDuration = 0.3f;
    private float coyoteTimeCounter = 0f;

    [Header("Grounded Variables")]
    private float groundedTime = 0f;
    public bool GetIfGrounded() { return isGrounded; }
    private bool isGrounded = false;
    private float groundedGraceTime = 0.1f; // tiempo mínimo en el suelo para resetear
    public bool inMovingPlatform = false;
    private Vector3 lastGroundedPos;
    [SerializeField] private float SafeGroundRadius = 1f;

    private AudioManager audioManager;
    [SerializeField] private Animator playerAnimator;

    // ----- NEW: external momentum (knockback smoothing) -----
    [Header("Knockback smoothing")]
    [SerializeField] private Vector3 externalVelocity = Vector3.zero; // planar impulse-preserved velocity (x,z)
    [SerializeField] private float externalVelocityDamping = 6f; // decay rate
    [SerializeField] private float externalVelocityBlend = 0.15f; // blending factor per FixedUpdate

    [Header("Balancín")]
    private PlayerSwing swingHandler;
    private Trampoline lastTrampoline;

    [Header("Trampoline")]
    [SerializeField] private bool trampolineEnablesDoubleJump = true;

    public Ability GetAbility(string name)
    {
        foreach (Ability ability in abilitiesList)
        {
            if (ability.abilityData.name == name)
                return ability;
        }
        return null;
    }

    public void ReducePlayerMovement(float movementDivisor, float rotationDivisor)
    {
        speedMovement = speedMovement / movementDivisor;
        rotationSpeed = rotationSpeed / rotationDivisor;
    }

    public void ResetPlayerMovement()
    {
        speedMovement = baseSpeedMovement;
        rotationSpeed = baseRotationSpeed;
    }

    public IEnumerator GradientPlayerMassAfterKnockbackImpact(float duration = 2f)
    {
        playerControllerEnabled = false;
        float startMass = 2f;
        float endMass = 1f;
        float elapsed = 0f;

        rigidBody.mass = startMass;

        while (elapsed < duration)
        {

            if (elapsed >= duration/2)
            {
                playerControllerEnabled = true;

            }
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rigidBody.mass = Mathf.Lerp(startMass, endMass, t);
            yield return null;
        }

        rigidBody.mass = endMass; // asegurar el valor final exacto
    }

    public void AddExternalVelocity(Vector3 v)
    {
        v.y = 0f;
        externalVelocity += v;
    }

    private void ApplyExternalVelocityTick()
    {
        // If there's no external velocity, don't do extra work (but still ensure SpeedControl when appropriate)
        if (externalVelocity.sqrMagnitude <= 1e-6f)
        {
            // Keep speed limit if not dashing and not onWall
            if (!isDashing && !onWall)
                SpeedControl();

            return;
        }

        // Current planar velocity (from physics + forces applied)
        Vector3 currentPlanar = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        // Target planar is current + external
        Vector3 targetPlanar = currentPlanar + externalVelocity;

        // Blend towards target to avoid snapping
        Vector3 newPlanar = Vector3.Lerp(currentPlanar, targetPlanar, Mathf.Clamp01(externalVelocityBlend));

        // Preserve vertical (gravity/jumps), compute current vertical and set combined velocity
        float currentY = rigidBody.velocity.y;
        rigidBody.velocity = new Vector3(newPlanar.x, currentY, newPlanar.z);

        // Decay external velocity (lerp towards zero)
        externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, externalVelocityDamping * Time.fixedDeltaTime);

        // apply speed limit if needed
        if (!isDashing && !onWall)
            SpeedControl();
    }

    void Start()
    {
        swingHandler = GetComponent<PlayerSwing>();
        audioManager = FindObjectOfType<AudioManager>();
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        currentJumps = 0;
        playerControllerEnabled = true;
        speedAnimation = 0;
        speedMovement = baseSpeedMovement;
        rotationSpeed = baseRotationSpeed;
        cameraFOVBase = cameraCinemachine.m_Lens.FieldOfView;

        if (GetAbility("Shoot").abilityData.isUnlocked)
            cannonGO.SetActive(true);
        else
            cannonGO.SetActive(false);
    }

    private void Update()
    {
        if (!playerControllerEnabled)
            return;

        // Si estamos en el balancín, delegamos y salimos
        if (swingHandler != null && swingHandler.IsSwinging)
        {
            swingHandler.HandleSwingUpdate();
            return;
        }

        isGrounded = IsGroundedChecker();

        HandleCoyoteTime();
        HandleWallInteractions();
        //UpdateWallAnticipation();
        HandleJumps();
        HandleDash();
        HandleGroundPound();
    }

    private void FixedUpdate()
    {
        if (playerControllerEnabled)
        {
            if (swingHandler != null && swingHandler.IsSwinging)
            {
                // aquí en principio no hace falta nada, la física del joint
                // ya trabaja con la gravedad normal; si algún día quieres
                // aplicar fuerzas extras por frame, puedes añadir un método
                // swingHandler.HandleSwingFixedUpdate();
                return;
            }

            Movement();
            UpdateDustParticles();
        }
    }


    private void Movement()
    {
        if (isDashing) return;

        Vector3 direction = Vector3.zero;

        // Input SOLO si no está bloqueado
        if (!movementBlocked)
        {
            direction = new Vector3(
                playerInput.actions["Movement"].ReadValue<Vector2>().x,
                0f,
                playerInput.actions["Movement"].ReadValue<Vector2>().y
            ).normalized;
        }

        float verticalSpeed = rigidBody.velocity.y;
        verticalSpeed += -gravity;

        if (onWall)
        {
            // WALL STICK: totalmente clavado
            if (Time.time < wallStickUntil)
            {
                verticalSpeed = 0f;
            }
            else
            {
                // WALL SLIDE
                if (verticalSpeed > 0f)
                    verticalSpeed = Mathf.Lerp(verticalSpeed, 0f, wallUpStopDamp * Time.deltaTime);

                if (verticalSpeed < -wallSlideSpeed)
                    verticalSpeed = -wallSlideSpeed;
            }
        }
        else
        {
            playerAnimator.SetBool("OnWall", false);
        }

        HandleCrouching();

        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (isCrouching)
                rigidBody.AddForce(moveDir * crouchSpeedMovement, ForceMode.Force);
            else
            {
                if ((playerInput.actions["Run"].IsPressed() && isGrounded && !canonShoot.GetIfAiming()) || (!isGrounded && longJumped))
                    rigidBody.AddForce(moveDir * runSpeedMovement, ForceMode.Force);
                else
                    rigidBody.AddForce(moveDir * speedMovement, ForceMode.Force);
            }

            transitionTimer = Mathf.Min(transitionTimer + Time.deltaTime, transitionDurationStart);
            speedAnimation = Mathf.Lerp(0f, 1f, transitionTimer / transitionDurationStart);
        }
        else
        {
            isMoving = false;

            transitionTimer = Mathf.Max(transitionTimer - Time.deltaTime / transitionDurationStop * transitionDurationStart, 0f);
            speedAnimation = Mathf.Lerp(0f, 1f, transitionTimer / transitionDurationStart);
        }

        // Cancelar empuje contra la pared si no estamos en OnWall
        if (!onWall && !isGrounded && facingWall)
        {
            Vector3 v = rigidBody.velocity;
            Vector3 n = lastWallNormal.normalized;

            float intoDot = Vector3.Dot(v, -n);
            if (intoDot > 0f)
            {
                v -= (-n) * intoDot;
                rigidBody.velocity = v;
            }
        }

        Vector3 planar = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);
        rigidBody.velocity = new Vector3(planar.x, verticalSpeed, planar.z);

        playerAnimator.SetBool("IsCrouching", isCrouching);
        SetSpeedAnimation(speedAnimation);
    }

    private void HandleCrouching()
    {
        if (playerInput.actions["Crouch"].IsPressed() && isGrounded)
        {
            isCrouching = true;
            timeCrouching += Time.deltaTime;

            crouchingCollider.enabled = true;
            baseCollider.enabled = false;
        }
        else if (!HasRoofAbove())
        {
            isCrouching = false;
            timeCrouching = 0;

            crouchingCollider.enabled = false;
            baseCollider.enabled = true;
        }
    }

    public void SetSpeedAnimation(float speed)
    {
        playerAnimator.SetFloat("Speed", speed);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        if (flatVel.magnitude > maxVelocity)
        {
            Vector3 limitedVel = flatVel.normalized * maxVelocity;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
        }
    }

    private void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeDuration;
            groundedTime += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            groundedTime = 0f;
        }
    }

    private void HandleWallInteractions()
    {
        if (doingGroundPound) return;

        if (isDashing) return;

        if (TryGetWallHit(out RaycastHit hit))
        {
            facingWall = true;
            lastWallHit = hit;
            lastWallNormal = hit.normal; // GUARDAMOS LA NORMAL
        }
        else
        {
            facingWall = false;
            // No reseteamos lastWallNormal aquí para poder usarla si el salto se hace de inmediato
        }

        if (!onWall)
        {
            if (isGrounded && groundedTime >= groundedGraceTime)
            {
                ResetJumps();
                rigidBody.drag = 5;
            }
            else
            {
                bool timeOK = Time.time >= blockWallLatchUntil;
                bool facingOK = Vector3.Dot(transform.forward, -lastWallNormal) >= minFacingDotToSetOnWall;
                bool forwardOK = Vector3.Dot(rigidBody.velocity, transform.forward) > minPlayerSpeedToSetOnWall;
                bool differentWallOK = IsDifferentWall(lastWallNormal);

                // MISMA pared desde la que hicimos el último wall jump
                bool sameWallAsLastJump = !differentWallOK;

                // Cooldown SOLO si es la misma pared
                bool timeWallOK = !sameWallAsLastJump || Time.time >= nextWallJumpAllowedTime;

                if (facingWall
                    && CanAttachToThisWall(lastWallNormal)
                    && (!requireNotGroundedToLatch || !isGrounded)
                    && HasMinGroundClearance()
                    && timeOK
                    && timeWallOK          //solo limita la misma pared
                    && facingOK
                    && forwardOK
                    && (canWall || differentWallOK))
                {
                    SetOnWall();
                }
                else
                {
                    rigidBody.drag = 0.5f;
                }
            }
        }
        else
        {
            // Si ya se permite input y el jugador mueve, nos soltamos
            if (Time.time >= wallDetachAllowedAt)
            {
                Vector2 moveInput = playerInput.actions["Movement"].ReadValue<Vector2>();

                if (moveInput.magnitude > 0.2f)
                {
                    // Convertimos el input a mundo (plano XZ, relativo a cámara)
                    Vector3 inputWorld =
                        Quaternion.Euler(0f, camera.transform.eulerAngles.y, 0f) *
                        new Vector3(moveInput.x, 0f, moveInput.y);

                    inputWorld.Normalize();

                    // Normal plana de la pared
                    Vector3 wallNormalPlanar = Vector3.ProjectOnPlane(lastWallNormal, Vector3.up).normalized;

                    // Dot: >0 = moviéndose HACIA la pared
                    float dotIntoWall = Vector3.Dot(inputWorld, -wallNormalPlanar);

                    // Solo nos soltamos si NO estamos empujando hacia la pared
                    if (dotIntoWall < 0.1f)
                    {
                        WallFall();
                        return;
                    }
                }
            }

            wallTimer += Time.deltaTime;

            if (wallTimer > timeToWallFall || !HasMinGroundClearance())
            {
                WallFall();
                wallTimer = 0;
            }
        }
    }

    private bool CanAttachToThisWall(Vector3 candidateNormal)
    {
        if (lastWallJumpNormal == Vector3.zero) return true;
        // Ángulo entre la pared del último salto y la nueva pared
        float angle = Vector3.Angle(candidateNormal, lastWallJumpNormal);
        return angle >= minAngleBetweenWalls;
    }

    private void SetOnWall()
    {
        onWall = true;
        canWall = false;
        ResetJumps();

        // PARÓN SECO
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        // Timers
        wallStickUntil = Time.time + wallStickTime;
        wallDetachAllowedAt = Time.time + wallDetachInputDelay;

        movementBlocked = true;          // <- BLOQUEA input/fuerzas de locomoción
        wallTimer = 0f;                  // asegúrate de arrancar el timer aquí


        playerAnimator.SetBool("OnWall", true);
        // A partir de este momento empezamos a contar el delay para poder hacer WallJump
        wallJumpCanStartAt = Time.time + wallJumpInputDelay;

        canonShoot.ShootAbility.SetCanUseAbility(false);
        // reset anticipación
        //wallApproachBlend = 0f;
        //playerAnimator.SetFloat("WallApproach", 0f);
        //playerAnimator.SetBool("NearWall", false);
    }

    private bool IsDifferentWall(Vector3 candidateNormal)
    {
        if (lastWallJumpNormal == Vector3.zero) return true;
        return Vector3.Angle(candidateNormal, lastWallJumpNormal) >= minAngleBetweenWalls;
    }

    private void WallFall()
    {
        onWall = false;
        movementBlocked = false;

        wallStickUntil = 0f;
        wallDetachAllowedAt = 0f;

        if (movementUnlockRoutine != null)
        {
            StopCoroutine(movementUnlockRoutine);
            movementUnlockRoutine = null;
        }

        playerAnimator.SetBool("OnWall", false);

        canonShoot.ShootAbility.SetCanUseAbility(true);

    }

    private bool HasMinGroundClearance()
    {
        // true si NO hay suelo dentro de la distancia indicada
        return DistanceToGroundChecker(minDistanceToGroundToSetOnWall);
    }

    private void UpdateWallAnticipation()
    {
        //float target = 0f;

        //// Anticipamos sólo si NO estamos ya en pared
        //if (!onWall)
        //{
        //    if (TryGetWallHit(out RaycastHit hit))
        //    {
        //        bool closeEnough = hit.distance <= wallAnticipationDistance;

        //        // ¿Mirando a la pared? (1 = de frente)
        //        float facingDot = Vector3.Dot(transform.forward, -hit.normal);
        //        bool facingOK = facingDot >= Mathf.Cos(wallAnticipationAngle * Mathf.Deg2Rad);

        //        // ¿Moviéndote hacia delante?
        //        float speedForward = Vector3.Dot(rigidBody.velocity, transform.forward);
        //        bool movingToWall = speedForward > wallAnticipationMinSpeed;

        //        bool clearanceOK = HasMinGroundClearance(); // ya la tienes implementada

        //        if (closeEnough && facingOK && movingToWall && clearanceOK)
        //            target = 1f;
        //    }
        //}

        //// Suavizado
        //wallApproachBlend = Mathf.MoveTowards(wallApproachBlend, target, wallAnticipationSmoothing * Time.deltaTime);

        //// Parámetros de animación
        //playerAnimator.SetFloat("WallApproach", wallApproachBlend);
        //playerAnimator.SetBool("NearWall", wallApproachBlend > 0.5f); // opcional
    }

    private void HandleJumps()
    {
        if (isDashing)
            return;


        if (!playerInput.actions["Jump"].WasPressedThisFrame())
        {
            isJumping = false;
            return;
        }

        if (playerInput.actions["Jump"].WasPressedThisFrame())
        {
            if (isCrouching)
            {
                if (!HasRoofAbove() && timeCrouching >= GetAbility("CrouchingJump").abilityData.cooldown && GetAbility("CrouchingJump").abilityData.isUnlocked)
                    StartCoroutine(DoCrouchingJump());
            }
            else if (currentJumps <= multipleJumps)
            {
                if (!(isGrounded || coyoteTimeCounter > 0f))
                    GetAbility("Jump").canUse = false;

                if (currentJumps == 0 && GetAbility("Jump").canUse)
                    StartCoroutine(DoNormalJump());
                else if (onWall)
                {
                    // Si todavía no ha pasado el delay desde que nos enganchamos,
                    // ignoramos este input de salto (no consumimos double jump tampoco).
                    if (Time.time >= wallJumpCanStartAt)
                        WallJump();
                    else
                        isJumping = false;
                }
                else if (GetAbility("DoubleJump").canUse && GetAbility("DoubleJump").abilityData.isUnlocked && !GetAbility("Jump").canUse)
                    StartCoroutine(DoDoubleJump());
                else
                    isJumping = false;
            }
            else
                isJumping = false;
        }
    }

    private bool TryGetWallHit(out RaycastHit bestHit)
    {
        bestHit = default;
        bool any = false;
        float minDist = Mathf.Infinity;

        Vector3[] origins = new Vector3[]
        {
        transform.position,
        transform.position + (Vector3.up * wallDetectionOffset),
        transform.position - (Vector3.up * wallDetectionOffset)
        };

        for (int i = 0; i < origins.Length; i++)
        {
            if (Physics.Raycast(origins[i], transform.forward, out RaycastHit hit, wallDetectionDistance, whatIsWall))
            {
                if (hit.distance < minDist)
                {
                    minDist = hit.distance;
                    bestHit = hit;
                }
                any = true;
            }
        }
        return any;
    }

    private Vector3 GetPlanarWallNormal()
    {
        if (lastWallNormal == Vector3.zero)
            return -transform.forward; // reserva

        Vector3 planar = Vector3.ProjectOnPlane(lastWallNormal, Vector3.up);
        if (planar.sqrMagnitude < 1e-4f)
            return -transform.forward; // si la normal apunta demasiado arriba/abajo

        return planar.normalized;
    }

    private void ResetJumps()
    {
        currentJumps = 0;
        currentDashes = 0;
    }

    private void Jump(float jumpForce)
    {
        currentJumps++;
        StopVerticalVelocity();
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    IEnumerator DoNormalJump()
    {
        isJumping = true;

        ResetJumps(); // tendria que haber algo que reseteara los dashes por si solo, y no así
        GetAbility("Jump").canUse = false;
        GetAbility("Dash").canUse = false;
        Jump(jumpForce);

        audioManager.SetPlaySfx(audioManager.JumpSound, transform.position);

        ParticleSystem particlesJump = jumpParticles.GetComponent<ParticleSystem>();
        particlesJump.Emit(5);

        if (playerInput.actions["Run"].IsPressed())
        {
            longJumped = true;
            StartCoroutine(CheckIfLongJump());
        }
        coyoteTimeCounter = 0f;

        playerAnimator.SetTrigger("Jumped");

        yield return new WaitForSeconds(0.4f);

        if (!GetAbility("DoubleJump").alreadyUsed)
            GetAbility("DoubleJump").canUse = true;
        GetAbility("Dash").canUse = true;

        blockWallLatchUntil = Time.time + postJumpNoLatchTime;
    }

    IEnumerator CheckIfLongJump()
    {
        yield return new WaitForSeconds(0.2f);
        while (longJumped)
        {
            if (isGrounded)
                longJumped = false;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void WallJump()
    {
        isJumping = true;

        audioManager.SetPlaySfx(audioManager.WallJumpSound, transform.position);

        onWall = false;

        // Dirección lateral: SIEMPRE la normal de la pared (plana)
        Vector3 away = GetPlanarWallNormal();

        ActivateWallJumpParticles();

        // Limpiamos vertical y aplicamos impulso compuesto
        StopVerticalVelocity();

        lastWallJumpNormal = lastWallNormal;            // recordamos desde qué pared saltamos
        StartCoroutine(WallRegrabCooldownRoutine());    // cooldown breve de re-agarre

        Vector3 lateral = away * wallJumpSideForce;
        Vector3 vertical = Vector3.up * wallJumpUpForce;

        rigidBody.useGravity = true;
        rigidBody.AddForce(lateral + vertical, ForceMode.Impulse);


        // reorientar al personaje mirando en sentido del salto
        if (faceAwayAfterWallJump)
        {
            Quaternion targetRot = Quaternion.LookRotation(away, Vector3.up);
            transform.rotation = targetRot;
        }

        playerAnimator.SetTrigger("WallJumped");


        //bloquea el salto durante un pequeño tiempo
        nextWallJumpAllowedTime = Time.time + wallJumpCooldown;

        // Si había un unlock anterior, lo cancelamos y lanzamos uno nuevo
        if (movementUnlockRoutine != null)
            StopCoroutine(movementUnlockRoutine);
        movementUnlockRoutine = StartCoroutine(UnlockMovementAfter(wallJumpMoveFreeze));

    }

    private IEnumerator UnlockMovementAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        movementBlocked = false;
        movementUnlockRoutine = null; // limpiar handle
    }

    private IEnumerator WallRegrabCooldownRoutine()
    {
        canWall = false;                  // evita re-pegarte instantáneamente
        yield return new WaitForSeconds(wallRegrabCooldown);
        canWall = true;                   // permite agarrar la siguiente pared
    }

    IEnumerator DoDoubleJump()
    {
        GetAbility("DoubleJump").alreadyUsed = true;

        movementBlocked = false;
        doingGroundPound = false;

        isJumping = true;
        GetAbility("DoubleJump").canUse = false;
        GetAbility("Dash").canUse = false;

        audioManager.SetPlaySfx(audioManager.DoubleJumpSound, 0.5f, transform.position);
        Jump(doubleJumpForce);

        canonShoot.ShootBullet(spawnBulletDoubleJumpPosition.position, false, true);
        canonShoot.SpawnCanonParticles();
        canonShoot.currentTimeShoot = 0.5f;


        playerAnimator.SetTrigger("DoubleJumped");

        yield return new WaitForSeconds(0.4f);

        GetAbility("Dash").canUse = true;
    }

    IEnumerator DoCrouchingJump()
    {
        movementBlocked = true;

        yield return new WaitForSeconds(0.3f);

        isCrouching = false;
        GetAbility("DoubleJump").canUse = false;
        ResetJumps();
        GetAbility("Jump").canUse = false;

        Jump(crouchingJumpForce);

        isJumping = true;
        playerAnimator.SetTrigger("CrouchJumped");

        yield return new WaitForSeconds(1f);
        movementBlocked = false;
        GetAbility("DoubleJump").canUse = true;
    }

    private void HandleDash()
    {
        if (isCrouching)
            return;

        if (!onWall)
        {
            if (isDashing)
                return;
            else
                SpeedControl();
        }

        if (playerInput.actions["Dash"].WasPressedThisFrame())
        {
            if (GetAbility("Dash").canUse && currentDashes < multipleDashOnAir && GetAbility("Dash").abilityData.isUnlocked && !GetAbility("Dash").alreadyUsed)
            {
                movementBlocked = false;
                doingGroundPound = false;

                StartCoroutine(DoDash());
            }
        }
    }

    private IEnumerator DoDash()
    {
        GetAbility("Dash").canUse = false;
        GetAbility("Dash").alreadyUsed = true;
        isDashing = true;
        currentDashes++;

        GetAbility("Jump").canUse = false;
        GetAbility("DoubleJump").canUse = false;
        canonShoot.currentTimeShoot = 0.3f;

        audioManager.SetPlaySfx(audioManager.DashSound, 0.5f, transform.position);
        playerAnimator.SetTrigger("Dashed");

        StartCoroutine(AddLitleForceUp());

        rigidBody.useGravity = false;

        // >>> NUEVO: guardamos y sobreescribimos el drag
        previousDrag = rigidBody.drag;
        rigidBody.drag = dashDrag;

        // >>> MISMO dashPower siempre (sin multiplicar en suelo)
        float _dashPower = dashPower;
        Vector3 dashDirection = _dashPower * transform.forward;

        StopVerticalVelocity();
        rigidBody.AddForce(dashDirection, ForceMode.Impulse);

        canonShoot.SpawnCanonParticles();

        yield return new WaitForSeconds(0.2f);

        canonShoot.ShootBullet(spawnBulletDashPosition.position, false, false);

        yield return new WaitForSeconds(dashDuration);

        // Fin del dash: restauramos físico
        if (!GetAbility("DoubleJump").alreadyUsed)
            GetAbility("DoubleJump").canUse = true;

        rigidBody.useGravity = true;
        isDashing = false;

        // >>> NUEVO: restaurar drag original
        rigidBody.drag = previousDrag;

        yield return new WaitForSeconds(GetAbility("Dash").abilityData.cooldown);

        GetAbility("Dash").alreadyUsed = false;
        GetAbility("Dash").canUse = true;
    }

    private void HandleGroundPound()
    {
        if (isGrounded)
        {
            if (doingGroundPound)
                StartCoroutine(ActivateGroundPoundExplosion());
        }
        else
        {
            if (playerInput.actions["Crouch"].WasPressedThisFrame() && DistanceToGroundChecker(distanceToGroundPound) && GetAbility("GroundPound").abilityData.isUnlocked)
                StartCoroutine(DoGroundPound());
        }
    }

    IEnumerator DoGroundPound()
    {
        doingGroundPound = true;
        playerControllerEnabled = false;
        GetAbility("DoubleJump").canUse = false;
        GetAbility("Dash").canUse = false;

        rigidBody.velocity = Vector3.zero;

        playerAnimator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.2f);

        playerControllerEnabled = true;
        movementBlocked = true;
        rigidBody.AddForce(Vector3.down * groundPoundForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.3f);

        if (!GetAbility("DoubleJump").alreadyUsed)
            GetAbility("DoubleJump").canUse = true;
        GetAbility("Dash").canUse = true;
    }

    IEnumerator ActivateGroundPoundExplosion()
    {
        doingGroundPound = false;

        movementBlocked = false;
        playerControllerEnabled = false;

        playerAnimator.SetTrigger("GroundPoundHit");
        groundPoundExplosion.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        groundPoundExplosion.SetActive(false);
        playerControllerEnabled = true;
    }

    public void CannonGrabbed(GameObject cannonToGrab)
    {
        cannonToGrab.SetActive(false);
        cannonGO.SetActive(true);
        StartCoroutine(DoGrabbingCannon());
    }

    IEnumerator DoGrabbingCannon()
    {
        playerAnimator.SetTrigger("CannonGrabbed");
        playerControllerEnabled = false;

        yield return new WaitForSeconds(1.3f);

        GetAbility("Shoot").abilityData.isUnlocked = true;
        GetAbility("DoubleJump").abilityData.isUnlocked = true;
        GetAbility("Dash").abilityData.isUnlocked = true;

        playerControllerEnabled = true;
    }

    private bool IsGroundedChecker()
    {
        //float detectionRadius = 0.02f;

        //Collider[] colliders = Physics.OverlapSphere(groundChecker.position, detectionRadius, whatIsGround);

        Vector3 point1 = transform.position + baseCollider.center + Vector3.up * (-(baseCollider.height / 2) + baseCollider.radius);
        Vector3 point2 = transform.position + baseCollider.center + Vector3.up * ((baseCollider.height / 2) - baseCollider.radius);

        point1 += Vector3.down * detectionGroundRadius;
        point2 += Vector3.down * detectionGroundRadius;

        //if (colliders.Length > 0)
        if (Physics.CheckCapsule(point1, point2, baseCollider.radius * 0.9f, whatIsGround) || inMovingPlatform)
        {
            playerAnimator.SetBool("OnGround", true);

            if (!isGrounded)
            {
                GetAbility("Jump").canUse = true;
                GetAbility("DoubleJump").canUse = true; GetAbility("DoubleJump").alreadyUsed = false;
                canWall = true;

                lastWallJumpNormal = Vector3.zero;

                audioManager.SetPlaySfx(audioManager.FallingToGroundSound, transform.position);

                //if (lastTrampoline != null)
                //{
                //    lastTrampoline.ResetBounces();
                //    lastTrampoline = null;
                //}
                // reset anticipación al tocar suelo
                //wallApproachBlend = 0f;
                //playerAnimator.SetFloat("WallApproach", 0f);
                //playerAnimator.SetBool("NearWall", false);
            }

            if (IsSafeGround())
                lastGroundedPos = transform.position;

            return true;
        }
        playerAnimator.SetBool("OnGround", false);

        return false;
    }

    public void RegisterTrampoline(Trampoline trampoline)
    {
        lastTrampoline = trampoline;
    }

    public void OnTrampolineBounce()
    {
        if (!trampolineEnablesDoubleJump)
            return;

        Ability doubleJump = GetAbility("DoubleJump");
        if (doubleJump == null || !doubleJump.abilityData.isUnlocked)
            return;

        doubleJump.canUse = true;
        doubleJump.alreadyUsed = false;
    }

    private bool DistanceToGroundChecker(float distanceToGround)
    {
        Ray ray = new Ray(groundChecker.position, Vector3.down);

        if (Physics.Raycast(ray, distanceToGround, whatIsGround))
        {
            return false;
        }

        return true;
    }

    private bool HasRoofAbove()
    {
        float detectionRadius = 1f;

        Collider[] colliders = Physics.OverlapSphere(roofChecker.position, detectionRadius, whatIsGround);
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    private bool IsSafeGround()
    {
        int checks = 8;
        for (int i = 0; i < checks; i++)
        {
            float angle = i * Mathf.PI * 2f / checks;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Vector3 origin = transform.position + dir * SafeGroundRadius + Vector3.up * 0.5f;

            if (!Physics.Raycast(origin, Vector3.down, 2f, whatIsSafeGround))
                return false;
        }
        return true;
    }

    public void ReturnFromDeathZone()
    {
        StartCoroutine(SetPositionFromDeathZone());
    }

    IEnumerator SetPositionFromDeathZone()
    {
        transform.position = lastGroundedPos;
        playerControllerEnabled = false;

        playerAnimator.SetTrigger("Respawn");
        SetSpeedAnimation(0);

        rigidBody.velocity = Vector3.zero;

        yield return new WaitForSeconds(1);
        playerControllerEnabled = true;
    }

    private void StopVerticalVelocity()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
    }

    IEnumerator AddLitleForceUp()
    {
        yield return new WaitForSeconds(0.1f);
        rigidBody.AddForce(Vector3.up * 3.5f, ForceMode.Impulse);
    }

    private void UpdateDustParticles()
    {
        if (isMoving && isGrounded)
            PlayParticles(dustParticles);
        else
            StopParticles(dustParticles);
    }

    private void SpawnCanonParticles(GameObject particles, Vector3 position)
    {
        GameObject _particles = Instantiate(particles, position, particles.transform.rotation);

        ParticleSystem instantiateParticles = _particles.GetComponent<ParticleSystem>();
        instantiateParticles.Play();

        Destroy(_particles, 3f);
    }

    private void PlayParticles(GameObject particles)
    {
        ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = true;
    }

    private void StopParticles(GameObject particles)
    {
        ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = false;
    }

    private void ActivateWallJumpParticles()
    {
        wallJumpParticles.SetActive(true);

        SpawnCanonParticles(wallJumpParticles, wallJumpParticles.transform.position);
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * wallDetectionDistance));
        Gizmos.DrawLine(transform.position + (Vector3.up * wallDetectionOffset), transform.position + (transform.forward * wallDetectionDistance) + (Vector3.up * wallDetectionOffset));
        Gizmos.DrawLine(transform.position - (Vector3.up * wallDetectionOffset), transform.position + (transform.forward * wallDetectionDistance) - (Vector3.up * wallDetectionOffset));

        // Normal de la última pared
        if (lastWallNormal != Vector3.zero)
        {
            Gizmos.color = Color.cyan;
            Vector3 p = (lastWallHit.point != Vector3.zero) ? lastWallHit.point : transform.position;
            Gizmos.DrawLine(p, p + lastWallNormal * 0.5f);
        }
    }
}

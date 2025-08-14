using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSelectorMovement : MonoBehaviour
{
    public Rigidbody rigidBody { get; set; }
    [Header("References")]
    [SerializeField] Camera camera;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask whatIsGround;

    [SerializeField] GameObject dustParticles;

    [Header("Inputs")]
    PlayerInput playerInput;

    [Header("Movement Variables")]
    [SerializeField] float baseSpeedMovement = 40;
    private float speedMovement;
    [SerializeField] float baseRotationSpeed = 7.5f;
    private float rotationSpeed;

    bool isMoving;
    [SerializeField] float transitionDurationStart = 0.5f;
    [SerializeField] float transitionDurationStop = 0.5f;
    private float transitionTimer = 0f;
    float speedAnimation = 0;
    public bool playerControllerEnabled { get; set; }

    private AudioManager audioManager;
    [SerializeField]
    private Animator playerAnimator;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerControllerEnabled = true;
        speedAnimation = 0;
        speedMovement = baseSpeedMovement;
        rotationSpeed = baseRotationSpeed;
    }

    private void Update()
    {
        IsGrounded();
    }

    private void FixedUpdate()
    {
        if (playerControllerEnabled)
        {
            Movement();
            UpdateDustParticles();
        }
    }

    private void Movement()
    {
        Vector3 direction = new Vector3(playerInput.actions["Movement"].ReadValue<Vector2>().x, 0f, playerInput.actions["Movement"].ReadValue<Vector2>().y).normalized;

        float verticalSpeed = rigidBody.velocity.y;

        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir = ProjectOnSlope(moveDir);

            if (HasGroundInFront(moveDir))
            {
                rigidBody.AddForce(moveDir.normalized * speedMovement, ForceMode.Force);
            }

            transitionTimer += Time.deltaTime;
            if (transitionTimer > transitionDurationStart) transitionTimer = transitionDurationStart;
            speedAnimation = Mathf.Lerp(0f, 1f, transitionTimer / transitionDurationStart);
        }
        else
        {
            isMoving = false;

            transitionTimer -= Time.deltaTime / transitionDurationStop * transitionDurationStart;
            if (transitionTimer < 0f) transitionTimer = 0f;

            speedAnimation = Mathf.Lerp(0f, 1f, transitionTimer / transitionDurationStart);
        }
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, verticalSpeed, rigidBody.velocity.z);

        playerAnimator.SetFloat("Speed", speedAnimation);

    }

    private Vector3 ProjectOnSlope(Vector3 moveDir)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 3.5f, whatIsGround))
        {
            return Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;
        }
        return moveDir;
    }

    private bool IsGrounded()
    {
        float detectionRadius = 1.5f;

        Collider[] colliders = Physics.OverlapSphere(groundChecker.position, detectionRadius, whatIsGround);
        if (colliders.Length > 0)
        {
            playerAnimator.SetBool("OnGround", true);
            return true;
        }
        playerAnimator.SetBool("OnGround", false);
        return false;
    }

    private bool HasGroundInFront(Vector3 moveDir)
    {
        Ray ray = new Ray(groundChecker.position + moveDir.normalized, Vector3.down);
        return Physics.Raycast(ray, 0.75f, whatIsGround);
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

    private void UpdateDustParticles()
    {
        if (isMoving)
        {
            PlayParticles(dustParticles);
        }
        else
        {
            StopParticles(dustParticles);
        }
    }
}

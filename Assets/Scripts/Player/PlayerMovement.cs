using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody rigidBody { get; set; }
    [Header("References")]
    [SerializeField] Camera camera;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] GameObject armCanon;
    [SerializeField] GameObject armCanonJump;
    [SerializeField] GameObject canonJump;
    [SerializeField] GameObject canonIdle;
    [SerializeField] GameObject canonParticles;
    [SerializeField] GameObject wallJumpParticles;
    [SerializeField] GameObject jumpParticles;



    [SerializeField] Transform spawnJumpCanonParticlesPos;
    [SerializeField] CanonShoot canonShoot;
    [SerializeField] GameObject dustParticles;

    [Header("Inputs")]
    //[SerializeField] KeyCode m_JumpKey;
    //[SerializeField] KeyCode dashKey;
    PlayerInput playerInput;

    [Header("Movement Variables")]
    [SerializeField] float speedMovement;
    [SerializeField] float maxVelocity;
    [SerializeField] float rotationTime = 0.1f;
    float turnSmoothVelocity;
    bool isMoving;
    [SerializeField] float transitionDurationStart = 0.5f; 
    [SerializeField] float transitionDurationStop = 0.5f; 
    private float transitionTimer = 0f;
    float speedAnimation = 0; 
    public bool playerControllerEnabled { get; set;}

    [Header("Jump Variables")]
    [SerializeField] int multipleJumps = 2;
    [SerializeField] float jumpForce;
    [SerializeField] float doubleJumpForce;
    int currentJumps;
    bool isOnAir = false;
    public bool canJump { get; set; } 
    public bool DoubleJump => doubleJump;
    bool doubleJump;
    [SerializeField] float gravity;
    float verticalSpeed;
    bool isJumping = false;

    [Header("Dash Variables")]
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashPower = 300;
    bool canDash;
    public bool IsDashing => isDashing;
    bool isDashing;
    float coolDown = 1;

    [Header("WallJump Variables")]
    [SerializeField] float wallJumpUpForce;
    [SerializeField] float wallJumpSideForce;
    [SerializeField] float wallDetectionDistance = 0.02f;
    [SerializeField] float wallDetectionOffset = 0.5f;
    [SerializeField] float timeToWallFall = 3f;
    private bool onWall = false;
    private bool canWall = true;
    private float wallTimer;

    private AudioManager audioManager;
    [SerializeField]
    private Animator playerAnimator;


    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        currentJumps = 0;
        armCanonJump.SetActive(false);
        canonJump.SetActive(false);
        canDash = true;
        playerControllerEnabled = true;
        wallJumpParticles.SetActive(false);
        speedAnimation = 0;
        canJump = true; 

    }

    private void Update()
    {
        if (playerControllerEnabled)
        {
            Jumper();
            if (!onWall)
            {
                PlayerDash();

                if (IsGrounded())
                {
                    ResetJumps();
                    rigidBody.drag = 5;
                }
                else
                {
                    if (HeadOnWall() && canWall)
                    {
                        SetOnWall();
                    }
                    else if (HeadOnWall())
                    {
                        KillXZVelocity();
                    }
                    else
                    {
                        rigidBody.drag = 0.5f;
                    }

                }

                if (!isDashing)
                    SpeedControl();
            }
            else
            {
                wallTimer += Time.deltaTime;
                if ((wallTimer > timeToWallFall) || (!HeadOnWall()))
                {
                    WallFall();
                    wallTimer = 0;
                }
            }
        }

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
        if (isDashing) return;

        //float l_AD = Input.GetAxis("Horizontal");
        //float l_WS = Input.GetAxis("Vertical");
        //Vector3 l_Direction = new Vector3(l_AD, 0f, l_WS).normalized;
        Vector3 direction = new Vector3(playerInput.actions["Movement"].ReadValue<Vector2>().x, 0f, playerInput.actions["Movement"].ReadValue<Vector2>().y).normalized;


        float verticalSpeed = rigidBody.velocity.y;
        if (!onWall)
        {
            playerAnimator.SetBool("OnWall", false);
            verticalSpeed += /*Physics.gravity.y*/ -gravity;
        }

        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;
            //Look Where You Go
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //moveDirection = l_MoveDir;

            //Apply to rb
            //m_Rb.velocity = new Vector3(l_MoveDir.x * m_SpeedMovement * Time.deltaTime, verticalSpeed, l_MoveDir.z * m_SpeedMovement * Time.deltaTime);

            rigidBody.AddForce(moveDir.normalized * speedMovement, ForceMode.Force);

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

    public void SetSpeedAnimation(float speed)
    {
        playerAnimator.SetFloat("Speed", speed);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > maxVelocity)
        {
            Vector3 limitedVel = flatVel.normalized * maxVelocity;
            rigidBody.velocity = new Vector3(limitedVel.x, rigidBody.velocity.y, limitedVel.z);
        }
    }

    private void Jumper()
    {
        if (/*Input.GetKeyDown(m_JumpKey)*/playerInput.actions["Jump"].WasPressedThisFrame())
        {
            if (currentJumps < multipleJumps && !isDashing && canJump)
            {
                if (IsGrounded())
                {
                    audioManager.SetPlaySfx(audioManager.JumpSound, transform.position);
                    Jump(jumpForce);

                    ParticleSystem particlesJump = jumpParticles.GetComponent<ParticleSystem>();
                    particlesJump.Emit(5);

                    isJumping = true;
                    playerAnimator.SetTrigger("Jumped");
                }
                else if (onWall)
                {
                    audioManager.SetPlaySfx(audioManager.WallJumpSound, transform.position);
                    WallJump();
                    isJumping = true;
                    playerAnimator.SetTrigger("WallJumped");
                }
                else if (doubleJump)
                {
                    audioManager.SetPlaySfx(audioManager.DoubleJumpSound, 0.5f, transform.position);
                    Jump(doubleJumpForce);
                    canonShoot.ShootBullet();
                    canonShoot.SpawnCanonParticles();
                    canonShoot.currentTimeShoot = 0; 

                    doubleJump = false;
                    isJumping = true;
                    playerAnimator.SetTrigger("DoubleJumped");
                }
                else
                    isJumping = false;
            }
            else
            {
                isJumping = false;
            }
        }
        else
        {
            isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        float detectionRadius = 0.02f;

        Collider[] colliders = Physics.OverlapSphere(groundChecker.position, detectionRadius, whatIsGround);
        if (colliders.Length > 0)
        {
            doubleJump = true;
            canWall = true;
            playerAnimator.SetBool("OnGround", true);
            if (isOnAir)
                audioManager.SetPlaySfx(audioManager.FallingToGroundSound, transform.position);
            isOnAir = false;
            return true;
        }
        playerAnimator.SetBool("OnGround", false);
        isOnAir = true;
        return false;
    }

    private void ResetJumps()
    {
        currentJumps = 0;
    }
    private void Jump(float jumpForce)
    {
        StopVerticalVelocity();
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        currentJumps++;
    }

    private void StopVerticalVelocity()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
    }

    private void SpawnCanonParticles(GameObject particles, Vector3 position)
    {
        GameObject _particles = Instantiate(particles, position, particles.transform.rotation);

        ParticleSystem instantiateParticles = _particles.GetComponent<ParticleSystem>();
        instantiateParticles.Play();

        Destroy(_particles, 3);
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
        if (isMoving && IsGrounded())
        {
            PlayParticles(dustParticles);
        }
        else
        {
            StopParticles(dustParticles);
        }
    }

    private void PlayerDash()
    {
        if (isDashing) return;
        if (/*Input.GetKeyDown(dashKey)*/playerInput.actions["Dash"].WasPressedThisFrame() && canDash)
        {
            StartCoroutine(DoDash());
        }
    }

    private IEnumerator DoDash()
    {
        audioManager.SetPlaySfx(audioManager.DashSound, 0.5f, transform.position);
        playerAnimator.SetTrigger("Dashed");
        StartCoroutine(AddLitleForceUp());
        canDash = false;
        isDashing = true;

        rigidBody.useGravity = false;

        float _dashPower = IsGrounded() ? dashPower * 1.5f : dashPower;
        Vector3 dashDirection = _dashPower * transform.forward;
        StopVerticalVelocity();


        rigidBody.AddForce(dashDirection, ForceMode.Impulse);

        //SpawnCanonParticles(canonParticles, spawnJumpCanonParticlesPos.position);
        canonShoot.SpawnCanonParticles();

        yield return new WaitForSeconds(0.2f);

        canonShoot.ShootBullet();
        canonShoot.currentTimeShoot = 0;

        yield return new WaitForSeconds(dashDuration);

        //Stop inercia rigidBody;
        //m_Rb.velocity = new Vector3(0, 0, 0);

        rigidBody.useGravity = true;
        isDashing = false;


        yield return new WaitForSeconds(coolDown);
        canDash = true;
    }

    IEnumerator AddLitleForceUp()
    {
        yield return new WaitForSeconds(0.1f);
        rigidBody.AddForce(Vector3.up * 3.5f, ForceMode.Impulse);
    }

    private bool HeadOnWall()
    {
        return Physics.Raycast(transform.position, transform.forward, wallDetectionDistance, whatIsWall) ||
            Physics.Raycast(transform.position + (Vector3.up*wallDetectionOffset), transform.forward, wallDetectionDistance, whatIsWall) ||
            Physics.Raycast(transform.position - (Vector3.up * wallDetectionOffset), transform.forward, wallDetectionDistance, whatIsWall);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * wallDetectionDistance));
        Gizmos.DrawLine(transform.position + (Vector3.up * wallDetectionOffset), transform.position + (transform.forward * wallDetectionDistance) + (Vector3.up * wallDetectionOffset));
        Gizmos.DrawLine(transform.position - (Vector3.up * wallDetectionOffset), transform.position + (transform.forward * wallDetectionDistance) - (Vector3.up * wallDetectionOffset));
    }

    private void SetOnWall()
    {
        onWall = true;
        canWall = false;
        ResetJumps();
        rigidBody.velocity = Vector3.zero;
        rigidBody.useGravity = false;
        playerAnimator.SetBool("OnWall", true);
    }

    private void WallJump()
    {
        onWall = false;
        ActivateWallJumpParticles();
        Vector3 jumpDirection = -transform.forward;
        jumpDirection.Normalize();
        rigidBody.AddForce((jumpDirection * wallJumpSideForce) + (Vector3.up * wallJumpUpForce), ForceMode.Impulse);
        //transform.Rotate(Vector3.up * 180f);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z));

        rigidBody.useGravity = true;
    }

    private void ActivateWallJumpParticles()
    {
        wallJumpParticles.SetActive(true);

        //PlayParticles(wallJumpParticles);
        SpawnCanonParticles(wallJumpParticles, wallJumpParticles.transform.position);
        //StopParticles(wallJumpParticles);
    }

    private void WallFall()
    {
        onWall = false;
        rigidBody.useGravity = true;
    }

    private void KillXZVelocity()
    {
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y-0.5f, 0);
        Debug.Log(rigidBody.velocity);
    }
    public bool GetIsJumping()
    {
        return isJumping;
    }

}

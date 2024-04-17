using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    Rigidbody m_Rb;
    [SerializeField] Camera m_Camera;
    [SerializeField] Transform m_GroundChecker;
    [SerializeField] LayerMask m_WhatIsGround;
    [SerializeField] GameObject armCanon;
    [SerializeField] GameObject armCanonJump;
    [SerializeField] GameObject canonJump;
    [SerializeField] GameObject canonIdle;
    [SerializeField] GameObject canonParticles;
    [SerializeField] Transform spawnJumpCanonParticlesPos;

    [Header("Inputs")]
    [SerializeField] KeyCode m_JumpKey;
    [SerializeField] KeyCode m_ShootKey; 
    [SerializeField] KeyCode dashKey;


    [Header("Movement Variables")]
    [SerializeField] float m_SpeedMovement;
    [SerializeField] float m_RotationTime = 0.1f;
    float m_TurnSmoothVelocity;
    Vector3 moveDirection;

    [Header("Jump Variables")]
    [SerializeField] int multipleJumps = 2;
    [SerializeField] float m_JumpForce;
    [SerializeField] float doubleJumpForce;
    int currentJumps;
    bool doubleJump;
    bool canJump;
    [SerializeField] float gravity;
    float verticalSpeed;

    [Header("Dash Variables")]
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashPower = 300;
    bool canDash;
    bool isDashing;
    float coolDown = 1;



    private void Awake()
    {
        //Si no hay ningun player en la escena, el player va a ser este
        if (GameController.GetGameController().GetPlayer() == null)
        {
            //GameController.GetGameController().AddRestartGameElement(this);
            GameController.GetGameController().m_Player = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else // Y si lo hay, destruyemelo y te quedas con el otro player
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        currentJumps = 0;
        armCanonJump.SetActive(false);
        canonJump.SetActive(false);
        canDash = true;
    }

    private void Update()
    {
        Jumper();
        //Debug.Log("Current Jumps: " + currentJumps);
        PlayerDash();
        Debug.Log(moveDirection);

    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    private void Movement()
    {
        if (isDashing) return;

        float l_AD = Input.GetAxis("Horizontal");
        float l_WS = Input.GetAxis("Vertical");
        Vector3 l_Direction = new Vector3(l_AD, 0f, l_WS).normalized;

        float verticalSpeed = m_Rb.velocity.y;
        verticalSpeed += /*Physics.gravity.y*/ -gravity * Time.deltaTime;

        if (l_Direction.magnitude >= 0.1f)
        {
            //Look Where You Go
            float l_TargetAngle = Mathf.Atan2(l_Direction.x, l_Direction.z) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
            float l_Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, l_TargetAngle, ref m_TurnSmoothVelocity, m_RotationTime);
            transform.rotation = Quaternion.Euler(0f, l_Angle, 0f);

            Vector3 l_MoveDir = Quaternion.Euler(0f, l_TargetAngle, 0f) * Vector3.forward;
            moveDirection = l_MoveDir;

            //Apply to rb
            m_Rb.velocity = new Vector3(l_MoveDir.x * m_SpeedMovement * Time.deltaTime, verticalSpeed, l_MoveDir.z * m_SpeedMovement * Time.deltaTime);
        }
        else
        {
            //m_Rb.velocity = Vector3.zero;
        }
        //hacer que la verticalSpeed.y sea siempre a graviti
        m_Rb.velocity = new Vector3(m_Rb.velocity.x, verticalSpeed, m_Rb.velocity.z);
    }

    private void Jumper()
    {
        if (Input.GetKeyDown(m_JumpKey) )
        {
            if (IsGrounded() || currentJumps < multipleJumps && !isDashing)
            {
                Jump();
            }
        }
    }

    private bool IsGrounded()
    {
        float detectionRadius = 0.05f;
        bool l_IsGrounded = Physics.CheckSphere(m_GroundChecker.position, detectionRadius, m_WhatIsGround);

        if (l_IsGrounded) 
        { 
            currentJumps = 0;
        }

        return l_IsGrounded;
    }
    private void Jump()
    {
        //Debug.Log("Current Jumps: " + currentJumps);

        float jumpForce = currentJumps != 0  ? doubleJumpForce : m_JumpForce;
        StopVerticalVelocity();
        m_Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (currentJumps != 0)
        {
            armCanonJump.SetActive(true);
            canonJump.SetActive(true);
            armCanon.SetActive(false);
            canonIdle.SetActive(false);
            SpawnParticles(canonParticles, spawnJumpCanonParticlesPos.position);

            StartCoroutine(HoldCanonAgain(0.75f));
            //Debug.Log("double jump");
        }
        currentJumps++;

    }

    private void StopVerticalVelocity()
    {
        m_Rb.velocity = new Vector3(m_Rb.velocity.x, 0, m_Rb.velocity.z);
    }

    private void SpawnParticles(GameObject particles, Vector3 position)
    {
        GameObject _particles = Instantiate(particles, position, particles.transform.rotation);

        ParticleSystem instantiateParticles = _particles.GetComponent<ParticleSystem>();
        instantiateParticles.Play();

        Destroy(_particles, 3);
    }

    IEnumerator HoldCanonAgain(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        armCanon.SetActive(true);
        canonIdle.SetActive(true);
        armCanonJump.SetActive(false);
        canonJump.SetActive(false);
    }

    private void PlayerDash()
    {
        if (isDashing) return;
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            Debug.Log("dash");
            StartCoroutine(DoDash());
        }
    }

    private IEnumerator DoDash()
    {
        canDash = false;
        isDashing = true;

        m_Rb.useGravity = false;

        HoldCanonAgain(0f);

        Vector3 dashDirection = dashPower * transform.forward;
        m_Rb.velocity = dashDirection;

        SpawnParticles(canonParticles, spawnJumpCanonParticlesPos.position);

        yield return new WaitForSeconds(dashDuration);

        //Stop inercia rigidBody;
        //m_Rb.velocity = new Vector3(0, 0, 0);

        m_Rb.useGravity = true; 
        //_trailRenderer.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(coolDown);
        canDash = true;
    }

}

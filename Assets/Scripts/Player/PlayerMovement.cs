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

    [Header("Inputs")]
    [SerializeField] KeyCode m_JumpKey;
    [SerializeField] KeyCode m_ShootKey; 

    [Header("Movement Variables")]
    [SerializeField] float m_SpeedMovement;
    [SerializeField] float m_RotationTime = 0.1f;
    float m_TurnSmoothVelocity;

    [Header("Jump Variables")]
    [SerializeField] float m_JumpForce;
    [SerializeField] float doubleJumpForce;
    int currentJumps;
    bool doubleJump;
    bool canJump;


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
    }

    private void Update()
    {
        Jumper();
        Debug.Log("Current Jumps: " + currentJumps);

    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    private void Movement()
    {
        float l_AD = Input.GetAxis("Horizontal");
        float l_WS = Input.GetAxis("Vertical");
        Vector3 l_Direction = new Vector3(l_AD, 0f, l_WS).normalized;

        //float verticalSpeed = m_Rb.velocity.y;
        //verticalSpeed += Physics.gravity.y * Time.deltaTime;

        if (l_Direction.magnitude >= 0.1f)
        {
            //Look Where You Go
            float l_TargetAngle = Mathf.Atan2(l_Direction.x, l_Direction.z) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
            float l_Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, l_TargetAngle, ref m_TurnSmoothVelocity, m_RotationTime);
            transform.rotation = Quaternion.Euler(0f, l_Angle, 0f);

            Vector3 l_MoveDir = Quaternion.Euler(0f, l_TargetAngle, 0f) * Vector3.forward;

            //Apply to rb
            m_Rb.velocity = new Vector3(l_MoveDir.x * m_SpeedMovement * Time.deltaTime, m_Rb.velocity.y, l_MoveDir.z * m_SpeedMovement * Time.deltaTime);
        }
    }

    private void Jumper()
    {
        //Debug.Log("IsGrounded = " + IsGrounded());
        //Debug.Log(m_Rb.velocity.y);

        if (Input.GetKeyDown(m_JumpKey) )
        {
            if (IsGrounded() || currentJumps < 2 /* || m_Rb.velocity.y == 0*/)
            {
                Jump();
            }
        }
    }


    private bool IsGrounded()
    {
        float detectionRadius = 0.1f;
        bool l_IsGrounded = Physics.CheckSphere(m_GroundChecker.position, detectionRadius, m_WhatIsGround);
        //bool l_IsGrounded = Physics.Raycast(m_GroundChecker.position, Vector3.down, m_WhatIsGround);
        if (l_IsGrounded)
        {
            currentJumps = 0;
        }

        return l_IsGrounded;
        //float raycastDistance = 0.05f; 
        //RaycastHit hit;

        //if (Physics.Raycast(m_GroundChecker.position, Vector3.down, out hit, raycastDistance, m_WhatIsGround))
        //{
        //    currentJumps = 0;
        //    //canJump = true;
        //    //doubleJump = false;
        //    return true;
        //}

        //return false;
    }
    private void Jump()
    {
        //Debug.Log("Current Jumps: " + currentJumps);

        float jumpForce = currentJumps != 0  ? doubleJumpForce : m_JumpForce;

        StopVerticalVelocity();
        m_Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        currentJumps++;

    }

    private void StopVerticalVelocity()
    {
        m_Rb.velocity = new Vector3(m_Rb.velocity.x, 0, m_Rb.velocity.z);
    }


}

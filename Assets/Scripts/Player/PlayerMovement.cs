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
    }

    void FixedUpdate()
    {
        Movement();
        Jumper();
    }

    private void Movement()
    {
        float l_Horizontal = Input.GetAxis("Horizontal");
        float l_Vertical = Input.GetAxis("Vertical");
        Vector3 l_Direction = new Vector3(l_Horizontal, 0f, l_Vertical).normalized;

        if (l_Direction.magnitude >= 0.1f)
        {
            //Look Where You Go
            float l_TargetAngle = Mathf.Atan2(l_Direction.x, l_Direction.z) * Mathf.Rad2Deg + m_Camera.transform.eulerAngles.y;
            float l_Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, l_TargetAngle, ref m_TurnSmoothVelocity, m_RotationTime);
            transform.rotation = Quaternion.Euler(0f, l_Angle, 0f);

            Vector3 l_MoveDir = Quaternion.Euler(0f, l_TargetAngle, 0f) * Vector3.forward;

            float verticalSpeed = m_Rb.velocity.y;
            verticalSpeed += Physics.gravity.y * Time.deltaTime;

            //Apply to rb
            //m_Rb.velocity = l_MoveDir.normalized * m_SpeedMovement * Time.deltaTime;
            m_Rb.velocity = (l_MoveDir.normalized * m_SpeedMovement * Time.deltaTime) + (Vector3.up * verticalSpeed);
        }
    }

    private void Jumper()
    {
        Debug.Log("IsGrounded = " + IsGrounded());
        //Debug.Log(m_Rb.velocity.y);

        if (IsGrounded() /* m_Rb.velocity.y == 0*/)
        {
            if (Input.GetKeyDown(m_JumpKey))
            {
                Jump();
            }
        }
    }


    private bool IsGrounded()
    {
        float detectionRadius = 0.1f;
        bool l_IsGrounded = Physics.CheckSphere(m_GroundChecker.position, detectionRadius, m_WhatIsGround);

        return l_IsGrounded;
    }
    private void Jump()
    {
        Debug.Log("Jump");
        StopVerticalVelocity();
        m_Rb.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
    }

    private void StopVerticalVelocity()
    {
        m_Rb.velocity = new Vector3(m_Rb.velocity.x, 0, m_Rb.velocity.z);
    }

}

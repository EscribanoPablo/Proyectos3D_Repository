using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController_Test : MonoBehaviour
{
    [SerializeField] private Transform lookAt;
    [SerializeField] private float minDistance = 10.0f;
    [SerializeField] private float maxDistance = 20.0f;
    [SerializeField] private float rotationalYawSpeed = 180;
    [SerializeField] private float rotationalPitchSpeed = 90;
    [SerializeField] private float minPitch = -30;
    [SerializeField] private float maxPitch = 70;
    [SerializeField] private LayerMask avoidObstaclesLayerMask;
    [SerializeField] private float offsetAvoidObstacles = 0.1f;

    [SerializeField] private PlayerInput playerInputs;
    private Vector2 lookInput;
    private bool YaxisInverted = false;

    private void Awake()
    {
        playerInputs.actions["Camera"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInputs.actions["Camera"].canceled += ctx => lookInput = Vector2.zero;

        YaxisInverted = false;
    }

    private void OnEnable()
    {
        playerInputs.actions["Camera"].Enable();
    }

    private void OnDisable()
    {
        playerInputs.actions["Camera"].Disable();
    }

    /*private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }*/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            InvertYAxis();
    }

    private void LateUpdate()
    {
        transform.LookAt(lookAt.position);
        float lDistance = Vector3.Distance(transform.position, lookAt.position);
        Vector3 lEulerAngles = transform.rotation.eulerAngles;
        float lYaw = lEulerAngles.y * Mathf.Deg2Rad;
        float lPitch = lEulerAngles.x * Mathf.Deg2Rad;
        if (lPitch > Mathf.PI)
            lPitch -= 2.0f * Mathf.PI;

        float movementX = lookInput.x;
        float movementY = 0.0f;

        if (YaxisInverted)
            movementY = -lookInput.y;
        else
            movementY = lookInput.y;

        bool isGamepad = Gamepad.current != null && Gamepad.current.rightStick.ReadValue() != Vector2.zero;

        float sensitivityMult = 0.1f;

        if (isGamepad)
            sensitivityMult = 1.0f;
        else
            sensitivityMult = 0.1f;

        lYaw = lYaw + movementX * (rotationalYawSpeed * Mathf.Deg2Rad) * sensitivityMult * Time.deltaTime;
        lPitch = lPitch + movementY * (rotationalPitchSpeed * Mathf.Deg2Rad) * sensitivityMult * Time.deltaTime;
        lPitch = Mathf.Clamp(lPitch, minPitch * Mathf.Deg2Rad, maxPitch * Mathf.Deg2Rad);
        Vector3 lForward = new Vector3(Mathf.Sin(lYaw) * Mathf.Cos(-lPitch), Mathf.Sin(-lPitch), Mathf.Cos(lYaw) * Mathf.Cos(-lPitch));
        lDistance = Mathf.Clamp(lDistance, minDistance, maxDistance);
        Vector3 lDesiredPosition = lookAt.position - lForward * lDistance;

        Ray lRay = new Ray(lookAt.position, -lForward);
        RaycastHit lRaycastHit;
        if (Physics.Raycast(lRay, out lRaycastHit, lDistance, avoidObstaclesLayerMask.value))
            lDesiredPosition = lRaycastHit.point + lForward * offsetAvoidObstacles;

        transform.position = lDesiredPosition;
        transform.LookAt(lookAt.position);
    }

    public void RestartCamera()
    {
        transform.position = lookAt.position + new Vector3(maxDistance, 0.0f, 0.0f);
    }

    public void InvertYAxis() 
    {
        /*if (YaxisInverted)
            YaxisInverted = false;
        else
            YaxisInverted = true;*/
        YaxisInverted = !YaxisInverted;    
    }
}

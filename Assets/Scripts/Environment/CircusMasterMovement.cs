using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class CircusMasterMovement : MonoBehaviour
{
    Animation animationCircusMaster;
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip encenderAnimation;
    [SerializeField] AnimationClip olaAnimation;

    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float snapRotationFactor = 2;
    [SerializeField] List<GameObject> lights;
    [SerializeField] List<GameObject> introCameras;
    private int currentLight;
    private int currentCam;
    private float rotationDestination;

    private bool introducingScenario;
    private bool moving;

    private PlayerInput playerInput;
    private GameObject mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerInput.currentActionMap.Disable();
        //playerInput.SwitchCurrentActionMap("HudControls");
        animationCircusMaster = GetComponent<Animation>();
        animationCircusMaster.Play(idleAnimation.name);
        introducingScenario = true;
        currentLight = 0;
        currentCam = 0;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        foreach (GameObject cam in introCameras)
        {
            cam.SetActive(false);
        }
        introCameras[currentCam].SetActive(true);

        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (introducingScenario)
        {
            if (playerInput.actions["Accept"].WasPressedThisFrame())
            {
                SkipCutscene();
            }
            else
            {
                TurningLightsOn();
            }

        }
        else
        {
            // poner la ola cada 10 segundos o cuando llegue el player a un checkpoint
        }
    }

    private void SkipCutscene()
    {
        foreach (GameObject cam in introCameras)
        {
            cam.SetActive(false);
        }
        mainCamera.SetActive(true);

        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }

        introducingScenario = false;

        Vector3 direction = lights[lights.Count-1].transform.position - transform.position;
        direction.y = transform.position.y;
        float yRotationValue = Quaternion.LookRotation(direction).eulerAngles.y;
        transform.parent.transform.rotation = Quaternion.Euler(0, yRotationValue, 0);

        currentCam = introCameras.Count - 1;
        currentLight = lights.Count - 1;
    }

    private void TurningLightsOn()
    {
        if (rotationDestination == 0)
        {
            SetRotationDestination();
            moving = true;
        }

        if (moving)
        {
            float yRotationValue = Mathf.Lerp(transform.parent.transform.rotation.eulerAngles.y, rotationDestination, Time.deltaTime * rotateSpeed);
            //Debug.Log("Myangles: " + transform.rotation.eulerAngles.y + ", my new Y value: " + yRotationValue + ", my destination: " + rotationDestination);
            if (yRotationValue > 360)
            {
                rotationDestination -= 360;
            }
            else if(yRotationValue < 0)
            {
                rotationDestination += 360;
            }
            transform.parent.transform.rotation = Quaternion.Euler(0, yRotationValue, 0);
            //Debug.Log(transform.rotation.eulerAngles + "    , " + Quaternion.Euler(0, yRotationValue, 0));
            if (Snapping.Snap(rotationDestination, snapRotationFactor) == Snapping.Snap(transform.parent.transform.rotation.eulerAngles.y, snapRotationFactor))
            {
                moving = false;
                animationCircusMaster.CrossFade(encenderAnimation.name);
            }
        }
    }

    private void SetRotationDestination()
    {
        Vector3 direction = lights[currentLight].transform.position - transform.position;
        direction.y = transform.position.y;
        rotationDestination = Quaternion.LookRotation(direction).eulerAngles.y;
        if(rotationDestination - transform.parent.transform.rotation.eulerAngles.y > 180)
        {
            //transform.parent.transform.rotation = Quaternion.Euler(0, transform.parent.transform.rotation.eulerAngles.y + 360, 0);
            rotationDestination -= 360;
        }
        else if ( transform.parent.transform.rotation.eulerAngles.y - rotationDestination > 180)
        {
            rotationDestination += 360;
        }

    }

    private void EncenderLuz()
    {
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().lightTurningOnSound);
        lights[currentLight].SetActive(true);
    }

    private void SeguirAnimacion()
    {
        currentLight++;
        currentCam++;
        if (currentLight > lights.Count - 1)
        {
            introducingScenario = false;
            animationCircusMaster.CrossFadeQueued(olaAnimation.name);
        }
        else
        {
            moving = true;
            SetRotationDestination();
            if (currentLight == lights.Count - 1)
            {
                mainCamera.SetActive(true);
            }
            else
            {
                introCameras[currentCam].SetActive(true);
            }
            introCameras[currentCam - 1].SetActive(false);
        }
        animationCircusMaster.CrossFadeQueued(idleAnimation.name);
    }

}


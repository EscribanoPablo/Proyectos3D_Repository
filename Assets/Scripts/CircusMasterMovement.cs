using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircusMasterMovement : MonoBehaviour
{
    [SerializeField] Animation animationCircusMaster;
    [SerializeField] AnimationClip idleAnimation;
    [SerializeField] AnimationClip encenderAnimation;
    [SerializeField] AnimationClip olaAnimation;

    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float snapRotationFactor = 2;
    [SerializeField] List<GameObject> lights;
    private int currentLight;
    private float rotationDestination;

    private bool introducingScenario;
    private bool moving;
    private const float ROTATIONFORWARD_OFFSET = 180;


    // Start is called before the first frame update
    void Start()
    {
        animationCircusMaster.Play(idleAnimation.name);
        introducingScenario = true;
        currentLight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (introducingScenario)
        {
            TurningLightsOn();
        }
        else
        {
            // poner la ola cada 10 segundos o cuando llegue el player a un checkpoint
        }
    }

    private void TurningLightsOn()
    {
        if(rotationDestination == 0)
        {
            SetRotationDestination();
            moving = true;
        }

        if (moving)
        {
            float yRotationValue = Mathf.Lerp(transform.rotation.eulerAngles.y, rotationDestination, Time.deltaTime * rotateSpeed); ;
            Vector3 nextRotation = new Vector3(0,yRotationValue, 0);
            transform.rotation = Quaternion.Euler(nextRotation);
            if(Snapping.Snap(rotationDestination,snapRotationFactor) == Snapping.Snap(transform.rotation.eulerAngles.y,snapRotationFactor))
            {
                moving = false;
                animationCircusMaster.PlayQueued(encenderAnimation.name);
                Debug.Log("ARRIVED");
            }
        }

    }

    private void SetRotationDestination()
    {
        Vector3 direction = lights[currentLight].transform.position -  transform.position;
        direction.y = transform.position.y;
        rotationDestination = Quaternion.LookRotation(direction).eulerAngles.y - ROTATIONFORWARD_OFFSET;
        Debug.Log(rotationDestination);
    }

    private void EncenderLuz()
    {
        // encender luz
        currentLight++;
        if(currentLight > lights.Count - 1)
        {
            introducingScenario = false;
        }
        else
        {
            moving = true;
            SetRotationDestination();
            animationCircusMaster.CrossFadeQueued(idleAnimation.name);
        }
    }
}

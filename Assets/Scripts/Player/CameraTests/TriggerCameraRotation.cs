using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraRotation : MonoBehaviour, IRestartLevelElement
{
    [SerializeField] private RotateCamera cameraRotator;
    [SerializeField] private bool rotateOnX = false;
    [SerializeField] private float angleToRotateX = 30f;
    [SerializeField] private bool rotateOnY = true;
    [SerializeField] private float angleToRotateY = 30f;

    [SerializeField] private bool hasLinkedTrigger = false;
    [SerializeField] private bool isPrincipalTrigger = false;
    [SerializeField] private GameObject linkedTrigger;

    [SerializeField] private Checkpoint checkpointLinked;

    public virtual void Awake()
    {
        GameController.GetGameController().AddRestartLevelElement(this);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerDirection = other.transform.position - transform.position;
            playerDirection.Normalize();

            float dot = Vector3.Dot(playerDirection, transform.right);

            if (dot > 0)
            {
                cameraRotator.StartRotation(angleToRotate);
            }
            else
            {
                cameraRotator.StartRotation(-angleToRotate);
            }

        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !cameraRotator.GetIsRotating())
        {
            if (rotateOnX && rotateOnY)
            {
                cameraRotator.StartRotation(angleToRotateX, angleToRotateY);                
            }
            else if (rotateOnX)
            {
                cameraRotator.StartRotation(rotateOnX, angleToRotateX);
            }
            else
            {
                cameraRotator.StartRotation(rotateOnX, angleToRotateY);
            }

            if(hasLinkedTrigger) 
                linkedTrigger.SetActive(true);
            
            gameObject.SetActive(false);
        }
    }

    public void Restart()
    {
        if (isPrincipalTrigger) 
        {
            if (!checkpointLinked.GetIfCheckpointGrabbed())
            {
                gameObject.SetActive(true);

                if (hasLinkedTrigger)
                    linkedTrigger.SetActive(false);
            }
        }
    }
}

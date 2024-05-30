using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRestartLevelElement
{
    Vector3 startPosition;
    Quaternion startRotation;
    Rigidbody rigidBody;

    private void Awake()
    {
        GameController.GetGameController().SetCurrentPlayer(this);
        GameController.GetGameController().AddRestartLevelElement(this);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation; 
    }

    public void Restart()
    {
        StartCoroutine(ResetRigidbody());
    }

    IEnumerator ResetRigidbody()
    {
        yield return new WaitForSeconds(0.1f);
        rigidBody.velocity = Vector3.zero;
        transform.position = startPosition;
        transform.rotation = startRotation;
        ResetLight();
    }

    private void ResetLight()
    {
        GameObject.FindGameObjectWithTag("PlayerLight").GetComponent<FollowingPlayerAsLight>().ResetLight();
    }

    public void SetRespawnPos(Transform transform)
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        transform.position = startPosition;
        transform.rotation = startRotation;
    }
    
}

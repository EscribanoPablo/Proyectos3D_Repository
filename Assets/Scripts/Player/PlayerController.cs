using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRestartLevelElement
{
    Vector3 startPosition;
    Quaternion startRotation;
    Rigidbody rigidBody;
    [SerializeField] GameObject restartPlayerParticles;
    PlayerMovement playerMovement;

    [SerializeField] float timeToMoveAgain = 1; 

    private void Awake()
    {
        GameController.GetGameController().SetCurrentPlayer(this);
        GameController.GetGameController().AddRestartLevelElement(this);
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
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
        rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        transform.position = startPosition;
        transform.rotation = startRotation;
        ResetLight();

        yield return new WaitForSeconds(0.1f);
        playerMovement.playerControllerEnabled = false;
        rigidBody.velocity = Vector3.zero;
        playerMovement.SetSpeedAnimation(0);

        ParticleSystem particles = restartPlayerParticles.GetComponent<ParticleSystem>();

        yield return new WaitForSeconds(0.25f);
        particles.Emit(4);

        yield return new WaitForSeconds(timeToMoveAgain);
        playerMovement.playerControllerEnabled = true;

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

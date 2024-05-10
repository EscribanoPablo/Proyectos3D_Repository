using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRestartLevelElement
{
    Vector3 startPosition;
    Quaternion startRotation;

    private void Awake()
    {
        GameController.GetGameController().SetCurrentPlayer(this);
        GameController.GetGameController().AddRestartLevelElement(this);
    }

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation; 
    }

    public void Restart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        //GameController.GetGameController().RestartLevelElment(); 
    }

    public void SetRespawnPos(Transform transform)
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        transform.position = startPosition;
        transform.rotation = startRotation;
    }
    
}

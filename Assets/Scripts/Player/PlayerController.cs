using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRestartLevelElement
{
    Vector3 startPosition;
    Quaternion startRotation;

    private void Awake()
    {
        //Si no hay ningun player en la escena, el player va a ser este
        if (GameController.GetGameController().GetPlayer() == null)
        {
            //GameController.GetGameController().AddRestartGameElement(this);
            GameController.GetGameController().player = this;
            GameController.GetGameController().AddRestartLevelElement(this);
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else // Y si lo hay, destruyemelo y te quedas con el otro player
        {
            GameObject.Destroy(this.gameObject);
        }
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
    }
}

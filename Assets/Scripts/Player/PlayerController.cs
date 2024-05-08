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
        //if (GameController.GetGameController().GetPlayer() == null)
        //{
        //    //GameController.GetGameController().AddRestartGameElement(this);
        //    GameController.GetGameController().player = this;
        //    GameController.GetGameController().AddRestartLevelElement(this);
        //    GameObject.DontDestroyOnLoad(gameObject);
        //}
        //else // Y si lo hay, destruyeme el anterior player y te quedas con este player
        //{
        //    GameController.GetGameController().player.SetRespawnPos(transform);
        //    GameObject.Destroy(GameController.GetGameController().GetPlayer().gameObject);
        //}

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

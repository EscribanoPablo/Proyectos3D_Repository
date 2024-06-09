using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacles : MonoBehaviour, IRestartLevelElement
{
    protected const string PLAYER_TAG = "Player";
    protected const string BULLET_TAG = "Cannonball";
    public virtual void Awake()
    {
        GameController.GetGameController().AddRestartLevelElement(this);
    }

    public virtual void RestartLevel()
    {
    }

    public void Restart()
    {
        RestartLevel();
    }
}

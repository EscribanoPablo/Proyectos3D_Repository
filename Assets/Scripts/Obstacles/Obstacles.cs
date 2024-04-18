using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacles : MonoBehaviour
{
    protected const string PLAYER_TAG = "Player";
    protected const string BULLET_TAG = "Cannonball";
    public virtual void Awake()
    {

    }

    public virtual void RestartLevel()
    {

    }
}

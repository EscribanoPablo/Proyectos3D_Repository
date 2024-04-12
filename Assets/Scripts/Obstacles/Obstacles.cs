using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacles : MonoBehaviour
{
    public virtual void Awake()
    {

    }

    public abstract void RestartLevel();
}

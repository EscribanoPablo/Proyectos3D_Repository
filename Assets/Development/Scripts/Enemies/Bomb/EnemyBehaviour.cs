using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    //esta sera la clase padre de todos los enemigos, de momento solo tiene una funcion que deben eredar

    public abstract void ReceiveDamage();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Aqui se tendrian que añadir las cosas que se quieran detectar con el groundpound, como los enemigos
        if (other.gameObject.tag == "Enemy")
            other.GetComponent<EnemyBehaviour>().ReceiveDamage();
    }
}

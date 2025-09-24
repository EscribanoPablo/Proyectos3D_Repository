using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().AddExtraLife(1);
            this.gameObject.SetActive(false);
        }
    }
}

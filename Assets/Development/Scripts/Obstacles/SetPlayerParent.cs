using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerParent : MonoBehaviour
{
    [SerializeField] private GameObject parentGO;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(parentGO.transform);
            other.GetComponent<PlayerMovement>().inMovingPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            other.GetComponent<PlayerMovement>().inMovingPlatform = false;
        }
    }
}

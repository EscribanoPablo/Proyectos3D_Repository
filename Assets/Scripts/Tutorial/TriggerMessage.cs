using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMessage : MonoBehaviour
{
    public GameObject message;
    void Start()
    {
        if (message != null)
        {
            message.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && message != null)
        {
            message.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && message != null)
        {
            message.SetActive(false);
        }
    }
}

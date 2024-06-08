using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerMessage : MonoBehaviour
{
    [SerializeField]
    private GameObject messageController;
    [SerializeField]
    private GameObject messagePc;

    void Start()
    {
        messageController.SetActive(false);
        messagePc.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(FindObjectOfType<PlayerInput>().currentControlScheme == "Gamepad")
                messageController.SetActive(true);
            else
                messagePc.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (FindObjectOfType<PlayerInput>().currentControlScheme == "Gamepad")
                messageController.SetActive(false);
            else
                messagePc.SetActive(false);
        }
    }
}

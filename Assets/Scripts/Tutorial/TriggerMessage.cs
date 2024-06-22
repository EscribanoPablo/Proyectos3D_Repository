using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerMessage : MonoBehaviour, IRestartLevelElement
{
    [SerializeField]
    private GameObject messageController;
    [SerializeField]
    private GameObject messagePc;

    private bool soundNotPlayed = true;

    public FMODUnity.EventReference TutorialSound;

    void Start()
    {
        GameController.GetGameController().AddRestartLevelElement(this);
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

            if (soundNotPlayed)
            {
                FindObjectOfType<AudioManager>().SetPlaySfx(TutorialSound);
                soundNotPlayed = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (FindObjectOfType<PlayerInput>().currentControlScheme == "Gamepad")
            {
                messagePc.SetActive(false);
                messageController.SetActive(true);
            }
            else
            {
                messageController.SetActive(false);
                messagePc.SetActive(true);
            }
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

    public void Restart()
    {
        soundNotPlayed = true;
    }
}

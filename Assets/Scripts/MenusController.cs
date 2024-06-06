using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    private PlayerInput playerInputs;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField] 
    private GameObject controlMenu;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "BetaLevel01" || SceneManager.GetActiveScene().name == "BetaLevel02")
        {
            playerInputs = FindObjectOfType<PlayerInput>();
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "BetaLevel01" || SceneManager.GetActiveScene().name == "BetaLevel02")
        {
            if (playerInputs.actions["PauseGame"].WasPressedThisFrame())
            {
                FindObjectOfType<AudioManager>().ReduceVolume();
               pauseMenu.SetActive(true);
                playerInputs.enabled = false;
                Time.timeScale = 0;
            }
        }
    }

    public void StartButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Time.timeScale = 1;

            GameObject.FindObjectOfType<PlayTransition>().GoBlack(true);

            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(SceneManager.GetActiveScene().name == "BetaLevel01" || SceneManager.GetActiveScene().name == "BetaLevel02")
        {
            FindObjectOfType<AudioManager>().AugmentVolume();
            pauseMenu.SetActive(false);
            playerInputs.enabled = true;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SettingsButtonPressed()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void ExitButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Application.Quit();
        }
        else
        {
            if(SceneManager.GetActiveScene().name != "SettingsMenu")
            {
                FindObjectOfType<AudioManager>().AugmentVolume();
                playerInputs.enabled = true;
                Time.timeScale = 1;

                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceCrowdNoise);
                FindObjectOfType<AudioManager>().StopMusic(FindObjectOfType<AudioManager>().instanceGameSong);
                FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceMenuSong);
            }
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ControlsButtonPressed()
    {
        pauseMenu.SetActive(false);
        controlMenu.SetActive(true);
    }

    public void ReturnButtonPressed()
    {
        pauseMenu.SetActive(true);
        controlMenu.SetActive(false);
    }
}

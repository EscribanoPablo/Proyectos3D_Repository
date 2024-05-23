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

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "BetaLevel01")
        {
            playerInputs = FindObjectOfType<PlayerInput>();
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "BetaLevel01")
        {
            if (playerInputs.actions["PauseGame"].WasPressedThisFrame())
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void StartButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("BetaLevel01");
        }
        else if(SceneManager.GetActiveScene().name == "BetaLevel01")
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void SettingsButtonPressed()
    {
        SceneManager.LoadScene("SettingsMenu");
        Time.timeScale = 0;
    }

    public void ExitButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

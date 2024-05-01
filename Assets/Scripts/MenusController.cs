using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    private void Update()
    {
        
    }

    public void StartButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("Alpha_Level");
        }
        else
        {
            //FindObjectOfType<PauseMenuController>().UnpauseGame();
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
            SceneManager.LoadScene("MainMenu");
        }
    }
}

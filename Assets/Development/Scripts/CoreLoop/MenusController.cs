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

    [SerializeField]
    private GameObject settingsPauseMenu;

    [SerializeField]
    private GameObject skipCutsceneController;
    [SerializeField]
    private GameObject skipCutscenePC;
    private bool watchingCutscene = true;

    [SerializeField]
    private GameObject creditsText;
    Animator creditsAnimator;
    float currentTime = 0;
    float clipDuration;

    public void DisableCutseceMessage()
    {
        skipCutsceneController.SetActive(false);
        skipCutscenePC.SetActive(false);
        watchingCutscene = false;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "BetaLevel01_Cat" || SceneManager.GetActiveScene().name == "BetaLevel02_Cat" || SceneManager.GetActiveScene().name == "TutorialLevel_Cat")
        {
            playerInputs = FindObjectOfType<PlayerInput>();
        }
        if (SceneManager.GetActiveScene().name == "CreditsScene_Cat")
        {
            if (creditsText != null)
            {
                creditsAnimator = creditsText.GetComponent<Animator>();
                AnimatorClipInfo[] clipInfo = creditsAnimator.GetCurrentAnimatorClipInfo(0);
                clipDuration = clipInfo.Length > 0 ? clipInfo[0].clip.length : 0f;
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "BetaLevel01_Cat" || SceneManager.GetActiveScene().name == "BetaLevel02_Cat" || SceneManager.GetActiveScene().name == "TutorialLevel_Cat")
        {
            if (playerInputs.actions["PauseGame"].WasPressedThisFrame())
            {
                pauseMenu.SetActive(true);
                playerInputs.enabled = false;
                Time.timeScale = 0;
            }

            if (SceneManager.GetActiveScene().name != "TutorialLevel_Cat" && watchingCutscene)
            {
                if (playerInputs.currentControlScheme == "Gamepad")
                {
                    skipCutscenePC.SetActive(false);
                    skipCutsceneController.SetActive(true);
                }
                else
                {
                    skipCutsceneController.SetActive(false);
                    skipCutscenePC.SetActive(true);
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "CreditsScene_Cat")
        {
            currentTime += Time.deltaTime;
            if (currentTime > clipDuration)
            {
                currentTime = 0;
                FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.MainMenu);
            }
        }
    }

    public void StartButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu_Cat")
        {
            Time.timeScale = 1;

            GameObject.FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.Level01);

            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (SceneManager.GetActiveScene().name == "BetaLevel01_Cat" || SceneManager.GetActiveScene().name == "BetaLevel02_Cat" || SceneManager.GetActiveScene().name == "TutorialLevel_Cat")
        {
            pauseMenu.SetActive(false);
            playerInputs.enabled = true;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void TutorialButtonPressed()
    {
        GameObject.FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.TutorialLevel);
    }

    public void SettingsButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu_Cat")
            GameObject.FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.Settings);
        else
        {
            pauseMenu.SetActive(false);
            settingsPauseMenu.SetActive(true);
        }
    }

    public void ExitButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu_Cat")
        {
            Application.Quit();
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "SettingsMenu_Cat")
            {
                Time.timeScale = 1;
            }
            GameObject.FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.MainMenu);
        }
    }

    public void ControlsButtonPressed()
    {
        if(SceneManager.GetActiveScene().name != "SettingsMenu_Cat")
            pauseMenu.SetActive(false);
        controlMenu.SetActive(true);
    }

    public void ReturnButtonPressed()
    {
        if (SceneManager.GetActiveScene().name != "SettingsMenu_Cat")
        {
            pauseMenu.SetActive(true); 
            settingsPauseMenu.SetActive(false);
        }
        controlMenu.SetActive(false);
    }
}

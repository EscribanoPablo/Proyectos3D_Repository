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
    private GameObject skipCutsceneController;
    [SerializeField]
    private GameObject skipCutscenePC;

    [SerializeField]
    private GameObject creditsText;
    Animator creditsAnimator;
    float currentTime = 0;
    float clipDuration;

    public void DisableCutseceMessage()
    {
        skipCutsceneController.SetActive(false);
        skipCutscenePC.SetActive(false);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "BetaLevel01" || SceneManager.GetActiveScene().name == "BetaLevel02" || SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            playerInputs = FindObjectOfType<PlayerInput>();

            if (SceneManager.GetActiveScene().name != "TutorialLevel")
            {
                if (playerInputs.currentControlScheme == "Gamepad")
                    skipCutsceneController.SetActive(true);
                else
                    skipCutscenePC.SetActive(true);
            }
        }
        if (SceneManager.GetActiveScene().name == "CreditsScene")
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
        if (SceneManager.GetActiveScene().name == "BetaLevel01" || SceneManager.GetActiveScene().name == "BetaLevel02" || SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            if (playerInputs.actions["PauseGame"].WasPressedThisFrame())
            {
                FindObjectOfType<AudioManager>().ReduceVolume();
                pauseMenu.SetActive(true);
                playerInputs.enabled = false;
                Time.timeScale = 0;
            }
        }
        if (SceneManager.GetActiveScene().name == "CreditsScene")
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
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Time.timeScale = 1;

            GameObject.FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.Level01);

            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (SceneManager.GetActiveScene().name == "BetaLevel01" || SceneManager.GetActiveScene().name == "BetaLevel02" || SceneManager.GetActiveScene().name == "TutorialLevel")
        {
            FindObjectOfType<AudioManager>().AugmentVolume();
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
        GameObject.FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.Settings);
    }

    public void ExitButtonPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Application.Quit();
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "SettingsMenu")
            {
                Time.timeScale = 1;
            }
            GameObject.FindObjectOfType<PlayTransition>().GoBlack(true, SceneToGo.MainMenu);
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

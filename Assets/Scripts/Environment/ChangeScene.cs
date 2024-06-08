using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            FindObjectOfType<PlayerInput>().enabled = false;
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().transitionsSound);

            if (SceneManager.GetActiveScene().name == "BetaLevel01")
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.Level02);

            else if (SceneManager.GetActiveScene().name == "BetaLevel02")
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.Credits);

            else if (SceneManager.GetActiveScene().name == "TutorialLevel")
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.MainMenu);

        }
    }

}

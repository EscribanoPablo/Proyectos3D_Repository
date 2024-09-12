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

            if (SceneManager.GetActiveScene().name == "BetaLevel01_Cat")
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.Level02);
            else if (SceneManager.GetActiveScene().name == "BetaLevel02_Cat")
            {
                FindObjectOfType<AudioManager>().PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceSecondStageEndEscapeSound);
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.FinalCinematic);
            }
            else if (SceneManager.GetActiveScene().name == "TutorialLevel_Cat")
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.MainMenu);

        }
    }

}

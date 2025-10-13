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

            if (SceneManager.GetActiveScene().name == "BetaLevel02_Cat") //Aqui se podria el nivel final, el cual activa creditos
            {
                FindObjectOfType<LevelsController>().SetSceneCompleted();
                FindObjectOfType<AudioManager>().PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceSecondStageEndEscapeSound);
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.FinalCinematic);
            }
            else if (SceneManager.GetActiveScene().name == "TutorialLevel_Cat")
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.MainMenu);
            else
            {
                //Aqui es donde llegan todos los niveles del juego, para al completarlos se vaya al level selector
                FindObjectOfType<LevelsController>().SetSceneCompleted();
                FindObjectOfType<GameController>().PlayerExtraLifes = FindObjectOfType<PlayerHealth>().GetActualExtraLifes();
                GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.LevelSelector);
            }

        }
    }

}

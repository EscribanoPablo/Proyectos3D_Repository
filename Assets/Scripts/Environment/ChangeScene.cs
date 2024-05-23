using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private GameObject videoTransition;
    [SerializeField]
    private GameObject videoGameEnded;

    [SerializeField]
    private float transitionTime = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(SceneManager.GetActiveScene().name == "BetaLevel01")
            {
                FindObjectOfType<PlayerInput>().enabled = false;
                videoTransition.SetActive(true);

                StartCoroutine(waitToChangeLevel());
            }
            else
            {
                FindObjectOfType<PlayerInput>().enabled = false;
                videoGameEnded.SetActive(true);

                StartCoroutine(waitToChangeLevel());
            }
        }
    }

    IEnumerator waitToChangeLevel()
    {
        yield return new WaitForSeconds(transitionTime);

        if (SceneManager.GetActiveScene().name == "BetaLevel01")
        {
            GameController.GetGameController().EmptyRestartList();
            SceneManager.LoadScene("BetaLevel02");
        }
        else
            Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : Obstacles
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLAYER_TAG)
        {
            GameController.GetGameController().EmptyRestartList();
            SceneManager.LoadScene("AlphaLevel_Prove01");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController gameController = null;

    public PlayerController player;

    public CameraController cameraController;

    public GameObject destroyObjects;

    static bool alreadyInitializated = false;

    List<IRestartLevelElement> restartLevelElements = new List<IRestartLevelElement>();

    public static GameController GetGameController()
    {
        if (gameController == null && !alreadyInitializated)
        {
            GameObject l_GameObject = new GameObject("GameController");
            gameController = l_GameObject.AddComponent<GameController>();
            //m_GameController.m_DestroyObjects = new GameObject("DestroyObjects");
            //m_DestroyObjects.transform.SetParent(l_GameObject.transform);
            GameController.DontDestroyOnLoad(l_GameObject);
            alreadyInitializated = true;
        }
        return gameController;
    }

    public PlayerController GetPlayer()
    {
        return player;
    }

    public CameraController GetCamera()
    {
        return cameraController;
    }

    public void AddRestartLevelElement(IRestartLevelElement element)
    {
        restartLevelElements.Add(element);
    }

    public void RestartLevelElment()
    {
        foreach (IRestartLevelElement element in restartLevelElements)
        {
            element.Restart();
        }
    }
}
    
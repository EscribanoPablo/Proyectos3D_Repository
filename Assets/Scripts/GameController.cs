using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool firstTimeInMenu = true;

    public int qualityValue = 1;

    public static GameController gameController = null;

    public PlayerController player;

    public AudioManager audioManager;

    public CameraController cameraController;

    public GameObject destroyObjects;

    public static bool alreadyInitializated = false;

    List<IRestartLevelElement> restartLevelElements = new List<IRestartLevelElement>();

    public static GameController GetGameController()
    {
        if (gameController == null && !alreadyInitializated)
        {
            GameObject gameObject = new GameObject("GameController");
            gameController = gameObject.AddComponent<GameController>();
            GameController.DontDestroyOnLoad(gameObject);
            alreadyInitializated = true;
        }
        return gameController;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public PlayerController GetPlayer()
    {
        return player;
    }

    public void SetCurrentPlayer(PlayerController player)
    {
        this.player = player;
    }

    public CameraController GetCamera()
    {
        return cameraController;
    }

    public void AddRestartLevelElement(IRestartLevelElement element)
    {
        restartLevelElements.Add(element);
    }
    public void EmptyRestartList()
    {
        restartLevelElements = new();
    }
    public void RemoveRestartLevelElement(IRestartLevelElement element)
    {
        restartLevelElements.Remove(element);
    }

    

    public void RestartLevelElement()
    {
        for (int index = 0; index < restartLevelElements.Count; index++)
        {
            if(restartLevelElements[index] != null)
                restartLevelElements[index].Restart();
        }
    }
}
    
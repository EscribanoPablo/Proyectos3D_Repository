using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    static GameController m_GameController = null;

    public PlayerMovement m_Player;

    public CameraController m_Camera;

    //public GameObject m_DestroyObjects;

    static bool m_AlreadyInitalized = false;

    public static GameController GetGameController()
    {
        if (m_GameController == null && !m_AlreadyInitalized)
        {
            GameObject l_GameObject = new GameObject("GameController");
            m_GameController = l_GameObject.AddComponent<GameController>();
            //m_GameController.m_DestroyObjects = new GameObject("DestroyObjects");
            //m_DestroyObjects.transform.SetParent(l_GameObject.transform);
            GameController.DontDestroyOnLoad(l_GameObject);
            m_AlreadyInitalized = true;
        }
        return m_GameController;
    }

    public PlayerMovement GetPlayer()
    {
        return m_Player;
    }

    public CameraController GetCamera()
    {
        return m_Camera;
    }
}
    
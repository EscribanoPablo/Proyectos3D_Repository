using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    CharacterController m_CharacterController;

    [Header("Inputs")]
    [SerializeField] KeyCode m_LeftKeyCode;
    [SerializeField] KeyCode m_RightKeyCode;
    [SerializeField] KeyCode m_LeftKeyCode;
    [SerializeField] KeyCode m_LeftKeyCode;


    private void Awake()
    {
        //Si no hay ningun player en la escena, el player va a ser este
        if (GameController.GetGameController().GetPlayer() == null)
        {
            m_CharacterController = GetComponent<CharacterController>();
            //GameController.GetGameController().AddRestartGameElement(this);
            GameController.GetGameController().m_Player = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else // Y si lo hay, destruyemelo y te quedas con el otro player
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    CharacterController m_CharacterController;
    Rigidbody m_Rb;

    [Header("Inputs")]
    [SerializeField] KeyCode m_LeftKeyCode;
    [SerializeField] KeyCode m_RightKeyCode;
    [SerializeField] KeyCode m_UpKeyCode;
    [SerializeField] KeyCode m_DownKeyCode;

    [Header("Movement Variables")]
    [SerializeField] float m_SpeedMovement;
    Vector3 m_Movement;


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
        m_CharacterController = GetComponent<CharacterController>();
        m_Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void HandleMovement()
    {
        Vector3 l_Forward = transform.forward;
        Vector3 l_Right = transform.right;

        l_Right.y = 0;
        l_Right.Normalize();

        l_Forward.y = 0;
        l_Forward.Normalize();

        if (Input.GetKey(m_LeftKeyCode))
        {
            
        }
    }
}

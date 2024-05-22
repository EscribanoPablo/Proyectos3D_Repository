using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    [Range(-1, 1)]
    [SerializeField] float minDotCameraVsWorld = -0.60f;
    PlayerController player;

    private void Awake()
    {

        //if (GameController.GetGameController().GetCamera() == null)
        //{

        //    GameController.GetGameController().cameraController = this;
        //    GameObject.DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    GameObject.Destroy(this.gameObject);
        //}
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {

        if (CameraTooInclined())
        {
            virtualCamera.m_LookAt = null;
        }
        else
        {
            virtualCamera.m_LookAt = player.transform;
        }


        //Debug.Log(transform.forward.normalized);
    }

    private bool CameraTooInclined()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.Normalize();
        return Vector3.Dot(Vector3.up, directionToPlayer) < minDotCameraVsWorld;
    }
}

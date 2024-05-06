using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    [SerializeField] float minPitchCam;

    private void Awake()
    {

        if (GameController.GetGameController().GetCamera() == null)
        {

            GameController.GetGameController().cameraController = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        //CinemachineComposer cinemachineComposer = virtualCamera.AddCinemachineComponent<CinemachineComposer>();

        //if (cinemachineComposer.m_ScreenY <= minPitchCam)
        //{
        //    cinemachineComposer.m_ScreenY = minPitchCam;
        //}

    }
    private void FixedUpdate()
    {
        CinemachineComposer cinemachineComposer = virtualCamera.AddCinemachineComponent<CinemachineComposer>();

        if (cinemachineComposer.m_ScreenY <= minPitchCam)
        {
            cinemachineComposer.m_ScreenY = minPitchCam;
        }
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform m_LookAt;
    public float m_MinDistance = 4.0f;
    public float m_MaxDistance = 7.0f;
    public float m_RotationalYawSpeed=180;
    public float m_RotationalPitchSpeed;
    public float m_MaxPitch=-40;
    public float m_MinPitch=80;

    public LayerMask m_AvoidObstaclesLayerMask;
    public float m_OffsetAvoidObstacles;

    private float inactiveTimer = 0f;
    public  const float inactiveThreshold = 5f;
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
    private void LateUpdate()
    {
        transform.LookAt(m_LookAt.position);
        float l_Distance = Vector3.Distance(transform.position, m_LookAt.position);
        Vector3 l_EulerAngles = transform.rotation.eulerAngles;
        float l_Yaw = l_EulerAngles.y*Mathf.Deg2Rad;
        float l_Pitch = l_EulerAngles.x * Mathf.Deg2Rad;
        if (l_Pitch>Mathf.PI)
        {
            l_Pitch -= 2.0f * Mathf.PI;
        }

        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = -Input.GetAxis("Mouse Y");


        l_Yaw = l_Yaw + l_MouseX * (m_RotationalYawSpeed * Mathf.Deg2Rad) * Time.deltaTime;
        l_Pitch= l_Pitch+ l_MouseY * (m_RotationalYawSpeed * Mathf.Deg2Rad) * Time.deltaTime;
        l_Pitch = Mathf.Clamp(l_Pitch, m_MinPitch*Mathf.Rad2Deg, m_MaxPitch*Mathf.Rad2Deg);
        Vector3 l_Forward=new Vector3(Mathf.Sin(l_Yaw)*Mathf.Cos(-l_Pitch), Mathf.Sin(-l_Pitch), Mathf.Cos(l_Yaw)*Mathf.Cos(-l_Pitch));
        l_Distance=Mathf.Clamp(l_Distance,m_MinDistance,m_MaxDistance);
        Vector3 l_DesiredPosition = m_LookAt.position - l_Forward*l_Distance;
        transform.position = l_DesiredPosition;
        transform.LookAt(m_LookAt.position);

        Ray l_Ray = new(m_LookAt.position, -l_Forward);
        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, l_Distance, m_AvoidObstaclesLayerMask.value))
        {
            l_DesiredPosition = l_RaycastHit.point + l_Forward * m_OffsetAvoidObstacles;
        }
        transform.position = l_DesiredPosition;
        transform.LookAt(m_LookAt.position);


        if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0)
        {
            inactiveTimer += Time.deltaTime;

            if (inactiveTimer >= inactiveThreshold)
            {
                Vector3 playerForward = m_LookAt.forward;
                Vector3 behindPlayerPosition = m_LookAt.position - playerForward * m_MinDistance;
                transform.position = behindPlayerPosition;
                transform.LookAt(m_LookAt.position);
            }
        }
        else
        {
            inactiveTimer = 0f;
        }
    }

}

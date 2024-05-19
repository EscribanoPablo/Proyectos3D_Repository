using UnityEngine;

public class FollowingPlayerAsLight : MonoBehaviour
{

    [SerializeField] float heightOffset = 10;
    //[SerializeField] float rightOffset;
    [SerializeField] float cameraSpeed = 1;

    private GameObject player;
    //private GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        Vector3 abovePlayerPosition = new Vector3(player.transform.position.x, player.transform.position.y + heightOffset, player.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, abovePlayerPosition, Time.deltaTime * cameraSpeed);
    }

    private void UpdateRotation()
    {
        Vector3 directionToLook = player.transform.position - transform.position;
        transform.forward = Vector3.Lerp(transform.forward.normalized, directionToLook.normalized, Time.deltaTime * cameraSpeed);
        //transform.LookAt(player.transform);
    }
}

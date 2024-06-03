using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowingPlayerAsLight : MonoBehaviour
{

    [SerializeField] float heightOffset = 10;
    //[SerializeField] float rightOffset;
    [SerializeField] float lightMovementSpeed = 1;
    [SerializeField] float lightRotateSpeed = 3;

    private GameObject player;
    private GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void OnEnable()
    {
        //FindObjectOfType<PlayerInput>().SwitchCurrentActionMap("PlayerActions");
        FindObjectOfType<PlayerInput>().currentActionMap.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 abovePlayerPosition = new Vector3(player.transform.position.x, player.transform.position.y + heightOffset, player.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, abovePlayerPosition, Time.deltaTime * lightMovementSpeed);
    }

    private void UpdateRotation()
    {
       //transform.rotation = Quaternion.Euler( transform.rotation.eulerAngles.x , mainCamera.transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
       //Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
       //transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lightRotateSpeed);
        //Vector3 directionToLook = player.transform.position - transform.position;
        //transform.forward = Vector3.Lerp(transform.forward.normalized, directionToLook.normalized, Time.deltaTime * cameraSpeed);
        //transform.LookAt(player.transform);
    }

    public void ResetLight()
    {
        Vector3 abovePlayerPosition = new Vector3(player.transform.position.x, player.transform.position.y + heightOffset, player.transform.position.z);
        transform.position = abovePlayerPosition;
    }
}

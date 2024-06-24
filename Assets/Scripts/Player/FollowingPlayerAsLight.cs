using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowingPlayerAsLight : MonoBehaviour
{

    [SerializeField] float heightOffset = 10;
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TutorialLevel")
            FindObjectOfType<PlayerInput>().transform.GetChild(1).gameObject.SetActive(true);
        else
            FindObjectOfType<PlayerInput>().transform.GetChild(1).GetComponent<Animator>().SetTrigger("IsInTutorial");

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "BetaLevel01")
            FindObjectOfType<AudioManager>().PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceFirstStageJackieSound);
        else
            FindObjectOfType<AudioManager>().PlayCircusMasterAudio(FindObjectOfType<AudioManager>().instanceSecondStageJackieSound);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TutorialLevel")
            FindObjectOfType<AudioManager>().PlayMusic(FindObjectOfType<AudioManager>().instanceGameSong);
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
        Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lightRotateSpeed);
    }

    public void ResetLight()
    {
        Vector3 abovePlayerPosition = new Vector3(player.transform.position.x, player.transform.position.y + heightOffset, player.transform.position.z);
        transform.position = abovePlayerPosition;
    }
}

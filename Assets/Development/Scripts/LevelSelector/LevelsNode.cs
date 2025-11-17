using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class AbilitiesEnabled
{
    public AbilityState abilitySO;
    public bool isEnabled;
}

public class LevelsNode : MonoBehaviour
{
    [SerializeField] private string levelSceneName;
    [SerializeField] private bool isUnlocked = false;
    public int GetLevelNumber() { return levelNumber; }
    [SerializeField] private int levelNumber = 0;

    private LevelInfoController levelInfoController;
    private bool isCompleted = false;
    [SerializeField] private int levelLinked = 0;

    public int collectiblesGrabbed { get; set;}
    public List<bool> collectiblesList = new List<bool>();

    [SerializeField] private bool doesUnlockArea = false; 
    [SerializeField] private OpenNextArea nextAreaEnabler;

    public void SetPlayerSpawnPos() 
    {
        Transform playerTransform = FindObjectOfType<LevelSelectorMovement>().GetComponent<Transform>();
        playerTransform.position = playerSpawnPostion.position;
        playerTransform.rotation = playerSpawnPostion.rotation;
    }
    [SerializeField] private Transform playerSpawnPostion;

    private PlayerInput playerInput;

    [SerializeField] private List<AbilitiesEnabled> abilitiesList = new List<AbilitiesEnabled>();

    public void SetLevelStatus(bool ifUnlocked, bool ifCompleted) 
    {
        isUnlocked = ifUnlocked;
        isCompleted = ifCompleted;

        if (isCompleted && doesUnlockArea)
            UnlockLinkedArea();
    }

    void Start()
    {
        levelInfoController = gameObject.GetComponent<LevelInfoController>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isUnlocked)
            {
                FindObjectOfType<LevelsController>().SetActualLevel(levelNumber);
                levelInfoController.EnableLevelInfo();
            }
            else
                levelInfoController.EnableLevelLockedUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isUnlocked && playerInput.actions["Jump"].IsPressed())
        {
            foreach(AbilitiesEnabled abilities in abilitiesList)
            {
                abilities.abilitySO.isUnlocked = abilities.isEnabled;
            }

            //UnityEngine.SceneManagement.SceneManager.LoadScene(levelSceneName);
            GameObject.FindObjectOfType<PlayTransition>().GoBlack(false, SceneToGo.Level01); 
            //Se necesitara mirar el play transition para poder hacer transiciones desde level selector,
            //para no tener que poner el valor del enum directamente y usar el nombre de la escena o algo
        }

        levelInfoController.WhileLevelInfoEnabled();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isUnlocked)
                levelInfoController.DisableLevelInfo();
            else
                levelInfoController.DisableLevelLockedUI();
        }
    }

    public void LevelCompleted(ref List<bool> LevelUnlockmentStatus)
    {
        if (doesUnlockArea)
            UnlockLinkedArea();

        LevelUnlockmentStatus[levelLinked - 1] = true;
    }

    public void UnlockLinkedArea()
    {
        nextAreaEnabler.EnableNewArea();
    }
}

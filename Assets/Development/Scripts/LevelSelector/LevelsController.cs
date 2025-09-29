using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsController : MonoBehaviour
{
    [SerializeField] List<bool> LevelsUnlockmentStatus = new List<bool>(); //Marca en orden los niveles para saber si estan desbloqueados o no
    [SerializeField] List<bool> LevelsCompletementStatus = new List<bool>(); //Marca en orden los niveles para saber si estan completados o no
    public void SetActualLevel(int levelNum) 
    {
        actualLevel = levelNum - 1; //Se le resta uno para poder pillar bien en la lista
    }
    private int actualLevel = 0;
    
    private bool levelCompleted = false;

    public List<bool> CollectiblesCount() { return LevelsCollectiblesStatus[actualLevel]; }
    List<List<bool>> LevelsCollectiblesStatus = new List<List<bool>>(); //Marca cuantos coleccionables ha cogido del nivel

    private LevelInfoSeter levelInfoSeter;

    private void Awake()
    {
        if (GameController.GetGameController().levelsController == null)
        {
            GameController.GetGameController().levelsController = this;
            GameObject.DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

            LevelInfoSeter levelInfoSeter = FindObjectOfType<LevelInfoSeter>();
            for (int i = 0; i < levelInfoSeter.LevelNodesList.Count; i++)
            {
                LevelsCollectiblesStatus.Add(levelInfoSeter.LevelNodesList[i].collectiblesList);
            }
        }
        else
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

    public void SetSceneCompleted() 
    {
        LevelsCollectiblesStatus[actualLevel] = FindObjectOfType<HudController>().GetCollectiblesTaken();
        levelCompleted = true; 
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LevelSelector_TestScene")
        {
            levelInfoSeter = FindObjectOfType<LevelInfoSeter>();

            if (levelCompleted)
            {
                foreach (LevelsNode node in FindObjectsOfType<LevelsNode>())
                {
                    if (node.GetLevelNumber() - 1 == actualLevel)
                    {
                        if (!LevelsCompletementStatus[actualLevel]) 
                        {
                            LevelsCompletementStatus[actualLevel] = true;
                            node.LevelCompleted(ref LevelsUnlockmentStatus);
                        }
                        
                        node.SetPlayerSpawnPos();
                        
                        levelCompleted = false;
                        break;
                    }
                }
            }

            levelInfoSeter.SetLevelsStatus(LevelsUnlockmentStatus, LevelsCompletementStatus, LevelsCollectiblesStatus);
        }
    }
}

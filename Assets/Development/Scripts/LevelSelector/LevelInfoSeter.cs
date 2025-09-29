using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfoSeter : MonoBehaviour
{
    public List<LevelsNode> LevelNodesList = new List<LevelsNode>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetLevelsStatus(List<bool> levelsUnlockmentStatus, List<bool> levelsCompletmentStatus, List<List<bool>> levelsCollectibleStatus) 
    {
        int levelIndex = 0;

        foreach (LevelsNode levelNodes in LevelNodesList) 
        {
            levelNodes.SetLevelStatus(levelsUnlockmentStatus[levelIndex], levelsCompletmentStatus[levelIndex]);
            levelNodes.collectiblesList = levelsCollectibleStatus[levelIndex];

            levelIndex++;
        }
    }
}

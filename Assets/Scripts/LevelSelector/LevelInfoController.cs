using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LevelInfoController : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private GameObject levelInfoUI;
    [SerializeField] private GameObject levelLockedUI;
    [SerializeField] private Transform PositionUI;

    [SerializeField] private TMP_Text levelNameText;

    [SerializeField] private GameObject collectibleNotTakenPrefab;
    [SerializeField] private GameObject collectibleTakenPrefab;
    [SerializeField] private Transform parentContainer;
    [SerializeField] private int collectibleQuantity = 3;

    [SerializeField] private GameObject messageController;
    [SerializeField] private GameObject messagePc;

    public void EnableLevelInfo() 
    {
        levelNameText.text = levelName;
        GenerateCollectibles();
        levelInfoUI.transform.position = PositionUI.position;

        if (FindObjectOfType<PlayerInput>().currentControlScheme == "Gamepad")
            messageController.SetActive(true);
        else
            messagePc.SetActive(true);

        levelInfoUI.SetActive(true);
    }

    public void WhileLevelInfoEnabled() 
    {
        if (FindObjectOfType<PlayerInput>().currentControlScheme == "Gamepad")
        {
            messagePc.SetActive(false);
            messageController.SetActive(true);
        }
        else
        {
            messageController.SetActive(false);
            messagePc.SetActive(true);
        }
    }

    public void DisableLevelInfo()
    {
        levelInfoUI.SetActive(false);
    }

    public void EnableLevelLockedUI() 
    {
        levelLockedUI.transform.position = PositionUI.position;
        levelLockedUI.SetActive(true);
    }

    public void DisableLevelLockedUI()
    {
        levelLockedUI.SetActive(false);
    }

    private void GenerateCollectibles()
    {
        foreach (Transform child in parentContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < collectibleQuantity; i++)
        {
            GameObject newIcon;
            if (FindObjectOfType<LevelsController>().CollectiblesCount()[i]) 
            { 
                newIcon = Instantiate(collectibleTakenPrefab, parentContainer);
                newIcon.name = collectibleTakenPrefab.name + (i + 1);
            }
            else
            {
                newIcon = Instantiate(collectibleNotTakenPrefab, parentContainer);
                newIcon.name = collectibleNotTakenPrefab.name + (i + 1);
            }
        }
    }
}

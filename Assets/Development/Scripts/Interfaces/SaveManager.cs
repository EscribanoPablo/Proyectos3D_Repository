using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public List<string> levelsData;  
    //public List<bool> abilitiesUnlocked;
    public string graphicsSettings;
    public List<float> audioVolume;
    public List<bool> achievementsUnlocked;
    public bool tutorialCompleted;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;
    public SaveData data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "save.json");
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Juego guardado en: " + savePath);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Datos cargados");
        }
        else
        {
            data = new SaveData(); // valores por defecto
            Debug.Log("No existía archivo, creado SaveData nuevo");
        }
    }
}

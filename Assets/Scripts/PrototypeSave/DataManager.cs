using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static string saveFilePath = Application.persistentDataPath + "/save.json";
    public static GameData gameData = new GameData();

    public static void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/save.json";
    }

    public static void ReadFile()
    {
        if (File.Exists(saveFilePath))
        {
            string savedData = File.ReadAllText(saveFilePath);
            gameData = JsonUtility.FromJson<GameData>(savedData);
        }
    }

    public static void WriteFile()
    {
        string gameDataToJson = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFilePath, gameDataToJson);
    }
}

[System.Serializable]
public class GameData
{
    public int level = -1;
    public bool checkpointed = false;
    public Vector3 position = Vector3.zero;
}

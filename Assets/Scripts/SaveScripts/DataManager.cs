using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Save files for game data (not for settings)
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

    public static void DeleteFile()
    {
        File.Delete(saveFilePath);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
}

[System.Serializable]
public class GameData
{
    public int level;
    public bool checkpointed = false;
    public CheckpointData[] checkpointDatas = new CheckpointData[System.Enum.GetValues(typeof(Levels)).Length];
    public bool[] levelCompletion = new bool[System.Enum.GetValues(typeof(Levels)).Length];
}

[System.Serializable]
public class CheckpointData
{
    public int room = -1;
    public Vector3 position = Vector3.zero;
}


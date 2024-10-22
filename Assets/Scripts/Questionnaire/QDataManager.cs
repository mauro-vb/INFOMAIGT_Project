using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QDataManager : MonoBehaviour
{
    public static QDataManager Instance;

    public string CurrentSceneName;
    public string NextSceneName;
    
    
    public GameLogger.InGameData inGameData;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
        var a = SceneManager.GetSceneByName(QDataManager.Instance.CurrentSceneName);
    }

    public void SetLoggerData(GameLogger.InGameData data)
    {
        inGameData = data;
    }

    public void ResetLoggerData()
    {
        inGameData = new GameLogger.InGameData();
    }

    public void UpdateScenes(string currentScene, string nextScene)
    {
        CurrentSceneName = currentScene;
        NextSceneName = nextScene;
    }
}
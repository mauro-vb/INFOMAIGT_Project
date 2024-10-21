using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QDataManager : MonoBehaviour
{
    public static QDataManager Instance;

    public string SceneName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            Instance.SceneName = SceneManager.GetActiveScene().name;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        Instance.SceneName = SceneManager.GetActiveScene().name;
    }
}

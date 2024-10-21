using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameLogger logger;
    public GameObject player;
    private string logFilePath = "log.json";
    public float startTime;

    void Start()
    {
        if (logger == null)
        {
            logger = GameObject.Find("GameLogger").GetComponent<GameLogger>();
        }

        // Start timer
        startTime = Time.time;

        // start coroutine for saving stats every x second
        StartCoroutine(SaveStatsCoroutine());
    }

    private void Update()
    {
        LoadNextScene();
    }

    void OnEnable()
    {
        Application.quitting += EndLevel;
    }

    IEnumerator SaveStatsCoroutine()
    {
        while (true)
        {
            SaveStatsToJSON(logFilePath);
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void EndLevel()
    {
        float timeElapsed = Time.time - startTime;
        logger.totalTimeToClearLevel = timeElapsed;
        logger.healthLeftAtEnd = player.GetComponent<ResourceController>().currentResource;
        SaveStatsToJSON(logFilePath);
    }

    void SaveStatsToJSON(string filePath)
    {
        string jsonData = JsonUtility.ToJson(logger, true);  // 'true' for pretty-printing
        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Player stats saved to " + filePath);
    }
    
    public void LoadNextScene()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            // Get the current scene index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Calculate the next scene index
            int nextSceneIndex = currentSceneIndex + 1;

            // Check if next scene index is within range of total scenes
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                // Load the next scene
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                // Optionally, if you're at the last scene, you can loop back to the first
                Debug.Log("No more scenes to load, you're at the last scene!");
            }
        }
    }
}

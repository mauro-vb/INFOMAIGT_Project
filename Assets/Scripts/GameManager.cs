using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

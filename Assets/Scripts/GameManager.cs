using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameLogger logger;
    public GameObject player;
    private string logFilePath = "log.json";
    public float startTime;

    private TextMeshProUGUI timerDisplay; // Reference to the timer display

    void Start()
    {
        if (logger == null)
        {
            logger = GameObject.Find("GameLogger").GetComponent<GameLogger>();
        }

        GameObject inGameUI = GameObject.Find("InGameUI");
        if (inGameUI != null)
        {
            timerDisplay = inGameUI.transform.Find("Canvas").transform.Find("Timer").transform.Find("TimerDisplay").gameObject.GetComponent<TextMeshProUGUI>();
        }

        // Start timer
        startTime = Time.time;

        // start coroutine for saving stats every x second
        StartCoroutine(SaveStatsCoroutine());
    }

    void Update()
    {
        float timeElapsed = Time.time - startTime;

        timerDisplay.text = "TIMER: " + Mathf.Floor(timeElapsed);
        Debug.Log(timerDisplay.text);
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
        if (player != null)
        {
            logger.healthLeftAtEnd = player.GetComponent<ResourceController>().currentResource;
        }
        else
        {
            logger.healthLeftAtEnd = 0;
        }
        SaveStatsToJSON(logFilePath);
    }

    void SaveStatsToJSON(string filePath)
    {
        string jsonData = JsonUtility.ToJson(logger, true);  // 'true' for pretty-printing
        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Player stats saved to " + filePath);
    }
}

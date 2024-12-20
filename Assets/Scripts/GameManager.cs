using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<float> OnIdleStop;
    public static event Action<float> OnLevelEndTimer;
    public static event Action<int> OnLevelEndPlayerResource;

    private bool idle = true;

    public GameObject player;
    public float startTime;

    private TextMeshProUGUI timerDisplay; // Reference to the timer display

    private TextMeshProUGUI levelNameDisplay;

    void Start()
    {
        levelNameDisplay = GameObject.Find("LevelName")?.GetComponent<TextMeshProUGUI>();
        if (levelNameDisplay)
        {
            string n = SceneManager.GetActiveScene().name;
            levelNameDisplay.text = n.Substring(0, n.Length - 1) + ' ' + n.Substring(n.Length - 1);
        }

        GameObject inGameUI = GameObject.Find("Canvas");
        if (inGameUI != null)
        {
            timerDisplay = inGameUI.transform.Find("Timer").transform.Find("TimerDisplay").gameObject
                .GetComponent<TextMeshProUGUI>();
        }

        // Start timer
        startTime = Time.time;

        player = GameObject.Find("Player");
    }

    void Update()
    {
        float timeElapsed = Time.time - startTime;

        timerDisplay.text = "TIMER: " + Mathf.Floor(timeElapsed);

        if (idle && Input.anyKey)
        {
            idle = false;
            OnIdleStop?.Invoke(Time.time - startTime);
        }
    }

    public void EndLevel()
    {
        float timeElapsed = Time.time - startTime;
        OnLevelEndTimer?.Invoke(timeElapsed);

        var r = player.GetComponent<ResourceController>();
        int health = r ? r.currentResource : 0;
        OnLevelEndPlayerResource?.Invoke(health);
    }
}
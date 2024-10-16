using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : MonoBehaviour
{
    private GameManager gameManager;
    private GameLogger logger;
    
    private bool idle = true;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (logger == null)
        {
            logger = GameObject.Find("GameLogger").GetComponent<GameLogger>();
        }
    }
    void Update()
    {
        if (Input.anyKey && idle)
        {
            idle = false;
            logger.idleTimeBeforeLevel = Time.time - gameManager.startTime;
        }
    }
}

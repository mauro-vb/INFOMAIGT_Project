using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogger : MonoBehaviour
{
    [Serializable]
    public struct InGameData
    {
        public float totalTimeToClearLevel;
        public float healthLeftAtEnd;
        public int totalShotsFired;
        public int totalShotsHit;
        public float shotsWhileMoving;
        public float idleTimeBeforeLevel;
        public int numberOfTries;
        public int resourceCollected; /* Missed shots that were recollected by the player */
        public float damageTaken;

        public float accuracy;
        
        public string GetStringHTML()
        {
            return "In Game Data" + "<br/>" +
                   "totalTimeToClearLevel = " + totalTimeToClearLevel + "<br/>" +
                   "healthLeftAtEnd = " + healthLeftAtEnd + "<br/>" +
                   "totalShotsFired = " + totalShotsFired + "<br/>" +
                   "totalShotsHit = " + totalShotsHit + "<br/>" +
                   "accuracy = " + accuracy + "<br/>" +
                   "shotsWhileMoving = " + shotsWhileMoving + "<br/>" +
                   "idleTimeBeforeLevel = " + idleTimeBeforeLevel + "<br/>" +
                   "numberOfTries = " + numberOfTries + "<br/>" +
                   "resourceCollected = " + resourceCollected + "<br/>" +
                   "damageTaken = " + damageTaken + "<br/>";
        }
    }

    public GameObject player;
    public GameObject dataManager;

    public InGameData data;


    // public float averageDistanceFromTarget; // cumulative, you need to divide this by totalShotsHit to get the average
    // public int healthPacksPickedUp;
    // public Vector2 movementTicks;  // Vector2 to store movement ticks (x: left/right, y: up/down)
    // public float timeSpentBehindWalls;
    // public float timeSpentInCorners;
    // public float timeSpentFighting;
    // public int powerUpsPickedUp;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        dataManager = GameObject.Find("QDataManager");

        data.numberOfTries = dataManager.GetComponent<QDataManager>().inGameData.numberOfTries;
        data.totalTimeToClearLevel = dataManager.GetComponent<QDataManager>().inGameData.totalTimeToClearLevel;
        data.idleTimeBeforeLevel = dataManager.GetComponent<QDataManager>().inGameData.idleTimeBeforeLevel;
    }

    void OnEnable()
    {
        // Subscribe to the shooting event
        WeaponController.OnPlayerShoot += HandlePlayerShoot;
        ProjectileController.OnEnemyHit += HandleEnemyHit;
        ProjectileController.OnProjectileCollected += HandleProjectileCollection;
        GameManager.OnLevelEndTimer += HandleTimeToCompleteLevel;
        GameManager.OnLevelEndPlayerResource += HandlePlayerHealth;
        GameManager.OnIdleStop += HandleIdleTimeBeforelevel;
        LevelManager.OnRestartLevel += HandleNumberOfRetries;
        UICommands.OnRestartLevel += HandleNumberOfRetries;
        PlayerController.OnDamageTaken += HandlePlayerDamageTaken;
    }

    void OnDisable()
    {
        // Unsubscribe from the event to avoid memory leaks
        WeaponController.OnPlayerShoot -= HandlePlayerShoot;
        ProjectileController.OnEnemyHit -= HandleEnemyHit;
        ProjectileController.OnProjectileCollected -= HandleProjectileCollection;
        GameManager.OnLevelEndTimer -= HandleTimeToCompleteLevel;
        GameManager.OnLevelEndPlayerResource -= HandlePlayerHealth;
        GameManager.OnIdleStop -= HandleIdleTimeBeforelevel;
        LevelManager.OnRestartLevel -= HandleNumberOfRetries;
        UICommands.OnRestartLevel -= HandleNumberOfRetries;
        PlayerController.OnDamageTaken -= HandlePlayerDamageTaken;

        // // Transfer collected data from the level to the QDataManager
        // if (dataManager != null)
        // {
        //     data.accuracy = data.totalShotsFired != 0 ? (float)data.totalShotsHit / data.totalShotsFired : 0;
        //     dataManager.GetComponent<QDataManager>().SetLoggerData(data);
        // }
    }

    private void OnDestroy()
    {
        // Transfer collected data from the level to the QDataManager
        if (dataManager != null)
        {
            data.accuracy = data.totalShotsFired != 0 ? (float)data.totalShotsHit / data.totalShotsFired : 0;
            dataManager.GetComponent<QDataManager>().SetLoggerData(data);
        }
    }

    // This method is called whenever the player shoots
    void HandlePlayerShoot()
    {
        data.totalShotsFired++;

        // check if the player is moving
        if (player.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            data.shotsWhileMoving++;
        }
    }

    // This method is called whenever the enemy hits the player
    void HandleEnemyHit()
    {
        data.totalShotsHit++;
    }

    // This method is called whenever the player collects a health pack
    void HandleProjectileCollection()
    {
        data.resourceCollected++;
    }

    void HandleTimeToCompleteLevel(float time)
    {
        data.totalTimeToClearLevel += time;
    }

    void HandleNumberOfRetries()
    {
        data.numberOfTries++;
    }

    void HandlePlayerHealth(int health)
    {
        data.healthLeftAtEnd = health;
    }

    void HandleIdleTimeBeforelevel(float time)
    {
        data.idleTimeBeforeLevel += time;
    }

    void HandlePlayerDamageTaken(int damage)
    {
        data.damageTaken += damage;
    }
}
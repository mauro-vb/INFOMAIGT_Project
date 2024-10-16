using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogger : MonoBehaviour
{
    
    public GameObject player;
    
    public float totalTimeToClearLevel;
    public int totalShotsFired;
    public int totalShotsHit;
    // public float accuracy;
    public float healthLeftAtEnd;
    public int healthPacksPickedUp;
    public float shotsWhileMoving;
    // public Vector2 movementTicks;  // Vector2 to store movement ticks (x: left/right, y: up/down)
    // public float idleTimeBeforeLevel;
    // public float timeSpentBehindWalls;
    // public float timeSpentInCorners;
    // public float timeSpentFighting;
    // public float averageDistanceFromTarget;
    // public int powerUpsPickedUp;
    // public float damageTaken;
    // public int numberOfTries;



    void OnEnable()
    {
        // Subscribe to the shooting event
        WeaponController.OnPlayerShoot += HandlePlayerShoot;
        EnemyController.OnEnemyHit += HandleEnemyHit;
        ProjectileController.OnProjectileCollected += HandleProjectileCollection;
    }

    void OnDisable()
    {
        // Unsubscribe from the event to avoid memory leaks
        WeaponController.OnPlayerShoot -= HandlePlayerShoot;
        EnemyController.OnEnemyHit -= HandleEnemyHit;
        ProjectileController.OnProjectileCollected -= HandleProjectileCollection;
    }

    // This method is called whenever the player shoots
    void HandlePlayerShoot()
    {
        totalShotsFired++;

        // check if the player is moving
        if (player.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            shotsWhileMoving++;
        }
    }

    // This method is called whenever the enemy hits the player
    void HandleEnemyHit()
    {
        totalShotsHit++;
    }

    // This method is called whenever the player collects a health pack
    void HandleProjectileCollection()
    {
        healthPacksPickedUp++;
    }
}

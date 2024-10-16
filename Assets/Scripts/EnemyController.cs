using System;
using Data;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // defining the event for when an enemy is hit
    public static event Action OnEnemyHit;
    public GameLogger logger;
    
    public EnemyData enemyData;
    public ResourceController resource; /* To be set in the editor */

    void Start()
    {
        if (logger == null)
        {
            logger = GameObject.Find("GameLogger").GetComponent<GameLogger>();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PROJECTILES)
        {
            var projectile = other.gameObject.GetComponent<ProjectileController>();
            
            // Trigger the shooting event
            OnEnemyHit?.Invoke();

            // passing the shooting distance to the logger
            logger.averageDistanceFromTarget += Vector3.Distance(projectile.launchPosition, projectile.transform.position);
            
            
            resource.currentResource -= projectile.damage;
        }
    }
}
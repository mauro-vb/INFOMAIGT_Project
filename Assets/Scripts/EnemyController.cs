using System;
using Data;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // defining the event for when an enemy is hit
    public static event Action OnEnemyHit;
    
    public EnemyData enemyData;
    public ResourceController resource; /* To be set in the editor */
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PROJECTILES)
        {
            // Trigger the shooting event
            OnEnemyHit?.Invoke();
            
            var projectile = other.gameObject.GetComponent<ProjectileController>();
            resource.currentResource -= projectile.damage;
        }
    }
}
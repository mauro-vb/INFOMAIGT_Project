using System;
using Data;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    public ResourceController resource; /* To be set in the editor */

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.PROJECTILES)
        {
            var projectileResource = other.gameObject.GetComponent<ResourceController>();
            resource.currentResource -= projectileResource.currentResource;
        }
    }
}

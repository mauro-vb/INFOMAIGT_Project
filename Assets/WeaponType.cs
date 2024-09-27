using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponType: ScriptableObject
{

    public int resourceCost = 1;
    public GameObject projectilePrefab ;
    [Range(1,20)]
    public int projectilesPerShot = 1;
    [Range(0f, 360f)]
    public float spreadAngle = 0f; // Angle to spread projectiles when firing multiple shots
    public float fireForce = 20f;         // The force with which the projectile is fired
    [Range(.15f,2.0f)]
    public float shootCooldown = 0.5f;    // Cooldown time between each shot
}

using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData_", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public int resourceCost = 1;
    [Range(1, 20)] public int projectilesPerShot = 1;
    [Range(0f, 360f)] public float spreadAngle = 0f; // Angle to spread projectiles when firing multiple shots
    public float projectileSpeed = 20f; // The initial velocity of the projectile
    [Range(.15f, 2.0f)] public float shootCooldown = 0.5f; // Cooldown time between each shot
}
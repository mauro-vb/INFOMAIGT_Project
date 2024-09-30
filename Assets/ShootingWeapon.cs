using UnityEngine;

public class ShootingWeapon : MonoBehaviour
{
    public WeaponType weaponType;
    public ResourceComponent resourceComponent; // Reference to the ResourceComponent

    private float cooldownTimer = 0f;
    private bool canShoot = true;
    private CircleCollider2D circleCollider;

    void Start()
    {
        // Get the ResourceComponent if not set manually
        if (resourceComponent == null)
        {
            resourceComponent = GetComponent<ResourceComponent>();
        }
        if (weaponType == null)
        {
          weaponType = Resources.Load<WeaponType>("WeaponTypes/BlobShooter");
        }
        // Find the CircleCollider2D for the blob size reference
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
        {
            Debug.LogWarning("CircleCollider2D is missing on this object.");
        }
    }

    void Update()
    {
        HandleCooldown();
    }

    private void HandleCooldown()
    {
        if (!canShoot)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canShoot = true; // Cooldown is over, can shoot again
            }
        }
    }

    public void Shoot()
    {
        if (!canShoot || resourceComponent.CurrentResource <= weaponType.resourceCost) return; // Prevent shooting during cooldown

        // Check if there's enough resource (ammo/health) to shoot
        if (resourceComponent.UseResource(weaponType.resourceCost))
        {
            FireProjectiles();
            canShoot = false;
            cooldownTimer = weaponType.shootCooldown;
        }
        else
        {
            Debug.Log("Not enough resource (ammo/health) to shoot!");
        }
    }

    private void FireProjectiles()
    {
        if (weaponType.projectilePrefab == null)
        {
            Debug.LogWarning("ProjectilePrefab is missing.");
            return;
        }

        ShootInSpread();
    }

    private void ShootInSpread()
    {
        // Calculate the position to shoot from, using the CircleCollider2D radius


        for (int i = 0; i < weaponType.projectilesPerShot; i++)
        {
            float angle = GetSpreadAngle(i);
            Quaternion rotation = gameObject.transform.rotation * Quaternion.Euler(0, 0, angle);
            Vector3 shootOrigin = GetShootOrigin();
            GameObject projectile = Instantiate(weaponType.projectilePrefab, shootOrigin, rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForce(rotation * Vector3.up * weaponType.fireForce, ForceMode2D.Impulse);
            }
        }
    }

    // Calculate the origin of the shot based on the CircleCollider2D's radius and blob's direction
    private Vector3 GetShootOrigin()
    {
        if (circleCollider == null) return transform.position; // Fallback in case there's no collider

        // Calculate the direction the blob is facing (based on its rotation)
        Vector3 shootDirection = transform.up.normalized; // Up is the forward direction in 2D

        // Calculate the position to shoot from (circle collider edge in the forward direction)
        Vector3 shootOrigin = transform.position + shootDirection * (circleCollider.radius * (2*weaponType.resourceCost)/5);

        return shootOrigin;
    }

    private float GetSpreadAngle(int projectileIndex)
    {
        float halfSpread = weaponType.spreadAngle / 2f;
        float step = weaponType.spreadAngle / Mathf.Max(weaponType.projectilesPerShot - 1, 1);
        return -halfSpread + (step * projectileIndex);
    }
}

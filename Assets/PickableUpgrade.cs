using UnityEngine;

public class UpgradePickable : MonoBehaviour
{
    public enum UpgradeType { Resource, WeaponChange } // Different types of upgrades
    public UpgradeType upgradeType;  // Choose the type in the Inspector

    public int value = 1;  // The value of the upgrade (e.g., +10 health or +1 ammo)
    public WeaponType weaponType;
    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    // Define colors for each upgrade type
    public Color resourceColor = Color.green;
    public Color weaponChangeColor = Color.red;

    void Start()
    {
        // Get the SpriteRenderer component attached to the same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the color based on the upgrade type
        switch (upgradeType)
        {
            case UpgradeType.Resource:
                spriteRenderer.color = resourceColor;
                break;
            case UpgradeType.WeaponChange:
                spriteRenderer.color = weaponChangeColor;
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player is the one colliding with the upgrade
        if (collision.gameObject.CompareTag("Blob"))
        {
          ShootingWeapon shootingWeapon = collision.gameObject.GetComponent<ShootingWeapon>();
          ResourceComponent resource = collision.gameObject.GetComponent<ResourceComponent>();
            {
                switch (upgradeType)
                {
                    case UpgradeType.Resource:
                        resource.Heal(value);
                        break;
                    case UpgradeType.WeaponChange:
                        shootingWeapon.weaponType = weaponType;
                        break;
                }
            }
            // Destroy the upgrade object after being collected
            Destroy(gameObject);
        }
    }
}

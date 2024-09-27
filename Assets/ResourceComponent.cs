using UnityEngine;

public class ResourceComponent : MonoBehaviour
{
    public int MaximumResource = 100;    // Represents both health and ammo
    public int CurrentResource = 100;    // Start with full resource

    public delegate void ResourceDepleted();


    // Handles taking damage (which reduces health and ammo)
    public void Damage(int amount)
    {
        CurrentResource = Mathf.Max(CurrentResource - amount, 0);
        if (CurrentResource <= 0)
        {
            // Destroy the GameObject when resource is depleted
            Destroy(gameObject);
        }
    }

    // Handles healing (which can increase both health and ammo)
    public void Heal(int amount)
    {
        CurrentResource = Mathf.Min(CurrentResource + amount, MaximumResource);
    }

    // Use resource (ammo) when shooting
    public bool UseResource(int amount)
    {
        if (CurrentResource >= amount)
        {
            CurrentResource -= amount;
            return true; // Successful use of resource
        }
        return false; // Not enough resource
    }
}

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageAmount = 5;
    private Rigidbody2D rb;
    [SerializeField]

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = transform.up * speed; // Set initial velocity
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ResourceComponent resourceComponent = collision.gameObject.GetComponent<ResourceComponent>();

        // Check if the collided object has a ResourceComponent and is not the Player
        if (resourceComponent != null && !collision.gameObject.CompareTag("Player"))
        {
            resourceComponent.Damage(damageAmount);
            Destroy(gameObject); // Destroy the projectile after applying damage
        }
        else
        {
            // Handle wall collision
        }
    }

}

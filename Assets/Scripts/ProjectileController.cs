using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // defining the event for when a projectile (health) is collected by player
    public static event Action OnProjectileCollected;
    
    public int resourceCost = 10;
    public float speed = 2.5f;
    public float rotationSpeed = 2.5f;
    public Vector2 dir = Vector2.up;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public int damage = 5;

    public Vector3 launchPosition;

    /* Kinda hacky but meh */
    // private bool canCollidePlayer

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);

        launchPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.ENVIRONMENT_ABSORBING || other.gameObject.layer == Layers.ENEMIES)
        {
            Destroy(gameObject);
        }

        if (other.gameObject.layer == Layers.PLAYER)
        {
            var playerResource = other.gameObject.GetComponent<ResourceController>();
            if (playerResource)
            {
                // triggering the collection event
                OnProjectileCollected?.Invoke();
                
                playerResource.currentResource += GetComponent<ResourceController>().currentResource;
            }
            Destroy(gameObject);
        }

        if (other.gameObject.layer == Layers.PROJECTILES)
        {
            // Ensure that only one of the two projectiles handles the collision.
            if (GetInstanceID() < other.gameObject.GetInstanceID())
            {
                var bulletResource = other.gameObject.GetComponent<ResourceController>();
                if (bulletResource)
                {                    
                    bulletResource.currentResource += GetComponent<ResourceController>().currentResource;
                }
                Destroy(gameObject);
            }
        }

        if (other.gameObject.layer == Layers.ENVIRONMENT_BOUNCING)
        {
            /* Bounce at a 45 degrees angles */
            Vector2 collisionNormal = other.contacts[0].normal;
            Vector2 reflectedVelocity = Vector2.Reflect(dir, collisionNormal);

            dir = reflectedVelocity;
            rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
        }

        if (other.gameObject.layer == Layers.ENVIRONMENT_WALL)
        {
            rb.velocity = Vector2.zero;
        }
    }
}

using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int resourceCost = 10;
    public float speed = 2.5f;
    public Vector2 dir = Vector2.up;

    private Rigidbody2D rb;

    public int damage = 5;

    /* Kinda hacky but meh */
    // private bool canCollidePlayer
    private PlayerControlsManager playerControlsManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerControlsManager = player.GetComponent<PlayerControlsManager>();
            if (playerControlsManager != null)
            {
                playerControlsManager.RegisterResourceController(GetComponent<ResourceController>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* Move */
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.ENVIRONMENT_ABSORBING || other.gameObject.layer == Layers.ENEMIES)
        {
            playerControlsManager.UnregisterResourceController(GetComponent<ResourceController>());
            Destroy(gameObject);
        }

        if (other.gameObject.layer == Layers.PLAYER)
        {
            var playerResource = other.gameObject.GetComponent<ResourceController>();
            if (playerResource)
            {
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
                    playerControlsManager.RegisterResourceController(bulletResource);
                }
                playerControlsManager.UnregisterResourceController(GetComponent<ResourceController>());
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
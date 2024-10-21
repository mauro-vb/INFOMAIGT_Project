using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // defining the event for when a projectile (health) is collected by player
    public static event Action OnProjectileCollected;

    public int resourceCost = 10;
    public float speed = 0;
    public float rotationSpeed = 0;
    public Vector2 dir = Vector2.up;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public int damage = 5;

    public Vector3 launchPosition;

    /* Kinda hacky but meh */
    // private bool canCollidePlayer
    private PlayerControlsManager playerControlsManager;

    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerControlsManager = player.GetComponent<PlayerControlsManager>();
            // if (playerControlsManager != null)
            // {
            //     playerControlsManager.RegisterResourceController(GetComponent<ResourceController>());
            // }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            //transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotationSpeed);
        }
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
                var rigidBody = other.gameObject.GetComponent<Rigidbody2D>();
                if (bulletResource)
                {
                    bulletResource.currentResource += GetComponent<ResourceController>().currentResource;
                    playerControlsManager.RegisterResourceController(bulletResource);
                }
                if (rigidBody)
                {
                  rigidBody.velocity = new Vector2(0,0);
                }
                playerControlsManager.UnregisterResourceController(GetComponent<ResourceController>());
                Destroy(gameObject);
                playerControlsManager.FindLargestResourceController();
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

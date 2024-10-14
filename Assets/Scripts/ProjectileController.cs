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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
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
            Destroy(gameObject);
        }

        if (other.gameObject.layer == Layers.PLAYER)
        {
            var playerResource = other.gameObject.GetComponent<ResourceController>();
            if (playerResource)
            {
                playerResource.currentResource += resourceCost;
            }
            Destroy(gameObject);
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
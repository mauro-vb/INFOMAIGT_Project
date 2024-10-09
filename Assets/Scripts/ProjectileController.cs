using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 2.5f;
    public Vector2 dir = Vector2.up;

    private Rigidbody2D rb;

    public int damage = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /* Move */
        rb.velocity = new Vector2(dir.x * speed, dir.y * speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == Layers.ENVIRONMENT || other.gameObject.layer == Layers.ENEMIES)
        {
            Destroy(gameObject);
        }

        if (other.gameObject.layer == Layers.ENVIRONMENT_BOUNCING)
        {
            /* Bounce at a 45 degrees angles */
            Vector2 collisionNormal = other.contacts[0].normal;
            Vector2 reflectedVelocity = Vector2.Reflect(dir, collisionNormal);

            dir = reflectedVelocity;
        }
    }
}